"""
Processes entire image with prompts
"""

import os
import queue
import threading
from pathlib import Path
from ph3_vision_app_config import Phi3_AppConfig

import onnxruntime_genai as og

PHI3_IMAGE_QUEUE: queue.Queue = queue.Queue()

import logging
import spacefx
logger = spacefx.logger(level=logging.INFO)

class Phi3Vision:
    """
    Runs Inference on a visual image using ONNX
    """

    def __init__(self):
        print("Starting Phi3 Vision..", end=" ")
        self.app_config = Phi3_AppConfig()
        self.prompt_header = "<|user|>"
        self.prompt_suffix = "<|end|>\n<|assistant|>\n"


        for i in range(0, self.app_config.NUM_OF_WORKERS):
            try:
                phi3_processor = threading.Thread(target=self.monitor_queue)
                phi3_processor.daemon = True
                phi3_processor.start()
            except Exception as e:
                logger.error(f"Failed to start thread {i+1}: {e}", exc_info=True)

        print ("success")

    @staticmethod
    def add_image_to_queue(imagefile:str):
        """
        Add an image to the queue for processing
        """
        PHI3_IMAGE_QUEUE.put(imagefile)


    def monitor_queue(self):
        """
        Monitors the image queue, processes each image, and saves the results.
        """
        # Initialize the Phi-3 Vision Model
        logger.info("Phi3 Vision Model Loading...")
        model = og.Model(str(Path(self.app_config.INBOX_FOLDER, self.app_config.MODEL_FOLDER)))
        processor =self.model.create_multimodal_processor()
        tokenizer_stream = self.processor.create_stream()


        # Start monitoring the image queue
        while True:
            # Get the next image from the queue
            input_image_path = Path(PHI3_IMAGE_QUEUE.get())
            logger.info(f"Processing {input_image_path}")

            prompt_response = self.single_image_process(input_image_path, model, processor, tokenizer_stream)
            print(prompt_response)

            logger.info(f"Finished processing {input_image_path}")
    

    def single_image_process(self, image_path, model, processor, tokenizer_stream):
        """
        Process a single image with phi-3-vision-instruct-cpu model
        """
        # Load Image
        logger.info(f"Loading image: {image_path}")
        image_paths = [image_path]
        images = og.Images.open(*image_paths)

        image_entry = self.image_prompt(1)

        # Construct the full prompt
        current_prompt = "\n".join(self.prompt_header, image_entry)


        for prompt in self.app_config.PROMPTS:
            current_prompt += "\n" + prompt

        current_prompt += "\n" + self.prompt_suffix

        logger.info(f"Prompting image {image_path} with prompt: {current_prompt}")

        inputs = processor(prompt, images=images)

        logger.info("Generating response...")
        params = og.GeneratorParams(model)
        params.set_inputs(inputs)
        params.set_search_options(max_length=7680)

        generator = og.Generator(model, params)
        response=""
        while not generator.is_done():
            generator.compute_logits()
            generator.generate_next_token()

            new_token = generator.get_next_tokens()[0]
            decoded_token = tokenizer_stream.decode(new_token)
            response += decoded_token
            print(decoded_token, end="", flush=True)

        for _ in range(3):
            print()

        # Delete the generator to free the captured graph before creating another one
        del generator

        return response

    def image_prompt(self, image_num):
        return f"<|image_{image_num}|>"
    
