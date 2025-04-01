
import spacefx
import time
import os
import json
import logging

import sys
sys.path.append(os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))), ".protos", "datagenerator", "planetary_computer"))
sys.path.append(os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))), ".protos", "imagine"))
from PlanetaryComputer_pb2 import EarthImageRequest, GeographicCoordinates
from ThalesSEC_pb2 import ThalesCamera,ThalesAPIAction,CameraRequest,CameraType,PictureParams

logger = spacefx.logger(level=logging.INFO)


class SensorConfig:

    # This is used to check to make sure the deisred sensor is available before
    # tasking it. This is the name of the sensor as it appears in the list of
    # available sensors.
    sensor_name: str = None

    def __init__(self,file_path:str=None):
        if file_path is None:
            raise ValueError("SensorConfig Init: file_path argument must be provided")

        try:
            self.check_file_exists(file_path)
        except FileNotFoundError as e:
            raise FileNotFoundError(f"SensorConfig Init: {e}")
        
        logger.info(f"SensorConfig Init: Reading config from {file_path}")
        with open(file_path, 'r') as f:
            data = json.load(f)
        for key, value in data.items():
            setattr(self, key, value)

    def check_file_exists(self,path):
        """
        Checks if a file exists at the given path.

        Args:
            path (str): Path to the file.

        Raises:
            FileNotFoundError: If the file does not exist.
        """
        # Wait for the file to appear if it's not available immediately
        file_available_timeout = time.time() + 120  # seconds
        while (not os.path.isfile(path)) and (time.time() < file_available_timeout):
            time.sleep(1)

        if not os.path.isfile(path):
            raise FileNotFoundError(f"The file {path} does not exist.")

    def ensure_dir_exists(self,path):
        """
        Ensures that a directory exists at the given path. If the directory does not exist, it will be created.

        Args:
            path (str): Path to the directory.
        """
        if not os.path.isdir(path):
            os.makedirs(path, exist_ok=True)
    

class PlanetaryComputerConfig(SensorConfig):

    def __init__(self,file_path:str=None):
        
        if file_path is None:
            raise ValueError("PlanetaryComputerConfig Init: file_path argument must be provided")
        super().__init__(file_path)

    def create_tasking_request(self):
        earth_image_request = EarthImageRequest()
        line_of_sight = GeographicCoordinates()
        line_of_sight.latitude = self.latitude
        line_of_sight.longitude = self.longitude
        earth_image_request.collection = self.collection
        bands = self.asset
        for band in bands:
            earth_image_request.asset.append(band)
        earth_image_request.geographicCoordinates.CopyFrom(line_of_sight)
        return earth_image_request

    def get_sensor_response(self):
        pass

class IMAGINeCameraConfig(SensorConfig):

    def __init__(self,file_path:str=None):
        if file_path is None:
            raise ValueError("IMAGINeCameraConfig Init: file_path argument must be provided")
        super().__init__(file_path)

    def create_tasking_request(self):
        # Create a new camera request
        imagine_camera_request = CameraRequest()

        # Set the camera type
        camera = CameraType()
        if self.camera == 'rgb':
            camera.CameraID = ThalesCamera.RGB
        elif self.camera == 'hyp':
            camera.CameraID = ThalesCamera.HYP
        else:
            raise ValueError(f"Invalid camera type {self.camera}")
        
        # Set the picture parameters
        picture_params = PictureParams()
        picture_params.nframes = self.n_frames
        picture_params.vnir_pga_gain = self.gain

        # Assign picture params to the camera
        camera.pictureParams = picture_params

        imagine_camera_request.task = ThalesAPIAction.CAPTURE
        imagine_camera_request.parameters.cameras.append(camera)

        return imagine_camera_request

    def get_sensor_response(self):
        pass

class ImageFromFileConfig(SensorConfig):    
        def __init__(self,file_path:str=None):
            if file_path is None:
                raise ValueError("ImageFromFile Init: file_path argument must be provided")
            super().__init__(file_path)
    
        def create_tasking_request(self):
            pass
    
        def get_sensor_response(self):
            pass