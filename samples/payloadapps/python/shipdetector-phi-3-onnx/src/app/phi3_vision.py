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
import json
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

        prompt_responses = self.single_image_process(input_image_path, model, processor, tokenizer_stream)


        # Validate the response
        output_responses = []
        for prompt_response in prompt_responses:
            response_data = self.response_validation(prompt_response)
            if response_data is None:
                logger.error(f"Invalid response: {prompt_response}")
                return
            output_responses.append(response_data)

        logger.info(f"Finished processing {input_image_path}")
        return output_responses


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
            response = ""
            while not generator.is_done():
                generator.compute_logits()
                generator.generate_next_token()

                new_token = generator.get_next_tokens()[0]
                decoded_token = tokenizer_stream.decode(new_token)
                response += str(decoded_token)
                print(decoded_token, end="", flush=True)
            
            responses.append(response)
            current_prompt += f"{response}<|end|>\n<|user|>"

            # Delete the generator to free the captured graph before creating another one
            del generator

        return responses

    def image_prompt(self, image_num):
        return f"<|image_{image_num}|>"
    
    def response_validation(self,response):
        """
        Validate the that the SLM response
        """
        try:

            response_data = json.loads(response)
            response_template = self.app_config.RESPONSE_TEMPLATE
            valid_json = True
            for field in response_template.keys():
                logger.info(f"Looking for field: {field}")
                if field not in response_data:
                    logger.error(f"Missing field in response: {field}")
                    valid_json = False
                else:
                    logger.info(f"Found field: {field}")
                    logger.info(f"Checking type for field: {field}, type: {response_template[field]['type']}")
                    expected_type = eval(response_template[field]["type"])
                    if not isinstance(response_data[field], expected_type):
                        logger.warning(f"Incorrect type for field {field}: expected {response_template[field]['type']}, got {type(response_data[field])}")
                        try:
                            response_data[field] = expected_type(response_data[field])
                            logger.info(f"Updated field {field} to correct type: {expected_type}")
                        except (ValueError, TypeError) as e:
                            logger.error(f"Failed to cast field {field} to {expected_type}: {e}")
                            valid_json = False
            if valid_json:
                return response_data
            else:
                return None
        except json.JSONDecodeError as e:
            logger.info(f"Invalid JSON response: {e}")
            return None
      
    
