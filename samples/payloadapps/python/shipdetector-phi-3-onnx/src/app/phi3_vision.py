"""
Processes entire image with prompts
"""

import os
import queue
import threading
from pathlib import Path
from ph3_vision_app_config import Phi3_AppConfig

import onnxruntime_genai as og


import logging
import spacefx
logger = spacefx.logger(level=logging.INFO)

class Phi3VisionRunner:
    """
    Runs Inference on a visual image using ONNX
    """

    def __init__(self):
        print("Starting Phi3 Vision..", end=" ")
        self.app_config = Phi3_AppConfig()
        self.prompt_header = "<|user|>"
        self.prompt_suffix = "<|end|>\n<|assistant|>\n"

        print ("success")


    def process_image(self, input_image_path):
        """
        Monitors the image queue, processes each image, and saves the results.
        """
        # Initialize the Phi-3 Vision Model
        logger.info("Phi3 Vision Model Loading...")
        model = og.Model(str(Path(self.app_config.INBOX_FOLDER, self.app_config.MODEL_FOLDER)))
        processor = model.create_multimodal_processor()
        tokenizer_stream = processor.create_stream()

        logger.info(f"Processing {input_image_path}")

        prompt_response = self.single_image_process(input_image_path, model, processor, tokenizer_stream)
        logger.info(prompt_response)

        logger.info(f"Finished processing {input_image_path}")



    def single_image_process(self, image_path, model, processor, tokenizer_stream):
        """
        Process a single image with phi-3-vision-instruct-cpu model
        """
        # Load Image
        logger.info(f"Loading image: {image_path}")
        image_paths = [image_path]
        if not os.path.exists(image_path):
            raise FileNotFoundError(f"Image {image_path} not found")
        images = og.Images.open(*image_paths)

        image_entry = self.image_prompt(1)

        # Construct the full prompt
        current_prompt = "\n".join([self.prompt_header, image_entry])

        responses = []

        # <|user|>\n<|image_1|>\n{prompt_1}<|end|>\n<|assistant|>\n{response_1}<|end|>\n<|user|>\n{prompt_2}<|end|>\n<|assistant|>\n

        for prompt in self.app_config.PROMPTS:

            current_prompt += "\n" + prompt + self.prompt_suffix

            logger.info(f"Prompting image {image_path} with prompt:\n{current_prompt}")

            # input prompt and images
            inputs = processor(current_prompt, images=images)

            logger.info("Generating response...")
            params = og.GeneratorParams(model)
            params.set_inputs(inputs)
            params.set_search_options(max_length=7680)

            generator = og.Generator(model, params)
            while not generator.is_done():
                generator.compute_logits()
                generator.generate_next_token()

                new_token = generator.get_next_tokens()[0]
                decoded_token = tokenizer_stream.decode(new_token)
                response = str(decoded_token)
                print(decoded_token, end="", flush=True)
                responses.append(response)
                current_prompt += f"{response}<|end|>\n<|user|>"

            # Delete the generator to free the captured graph before creating another one
            del generator

        return response

    def image_prompt(self, image_num):
        return f"<|image_{image_num}|>"
    
