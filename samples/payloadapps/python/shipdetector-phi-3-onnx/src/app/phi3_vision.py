import argparse
import os
import readline
import glob

import onnxruntime_genai as og

default_model_path = "../model/cpu-int4-rtn-block-32-acc-level-4"
default_user_prompt = "This is a overhead image of a the ground. Please provide a short description, of no longer than 100 words, of the content of this image. Make note of special geographic features in the image. Also, if the image includes water, please provide an accurate count of the number of boats in the image."

class Phi3Vision:
    """
    Runs the Phi-3-vision-instruct-cpu model on the provided imagery
    """

    def __init__(self, model_path=default_model_path):
        self.model_path = model_path

        self.model = og.Model(model_path)
        self.processor = self.model.create_multimodal_processor()
        self.tokenizer_stream = self.processor.create_stream()

        self.single_image_prompt_prefix = "<|user|>\n<|image_1|>"
        self.prompt_suffix = "<|end|>\n<|assistant|>\n"



    def single_image_process(self, image_path,user_prompt=default_user_prompt):
        """
        Process a single image with phi-3-vision-instruct-cpu model
        """
        # Load Image
        print(f"Loading image: {image_path}")
        image_paths = [image_path]
        images = og.Images.open(*image_paths)

        # Construct the full prompt
        current_prompt = self.single_image_prompt_prefix + "\n" + user_prompt + "\n" + self.prompt_suffix
        print(f"Using Prompt: {current_prompt}")

        inputs = processor(prompt, images=images)

        print("Generating response...")
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
