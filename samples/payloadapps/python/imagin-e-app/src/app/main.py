import logging
import sys
import os
import sys
import time

from app_config import AppConfig
from image_processor import ImageProcessor
from sensor_config import PlanetaryComputerConfig, IMAGINeCameraConfig, ImageFromFileConfig

from PlanetaryComputer_pb2 import EarthImageResponse
from ThalesSEC_pb2 import CameraResponse
from google.protobuf.wrappers_pb2 import StringValue as gpb_str


import rasterio


import spacefx
from spacefx.protos.common.Common_pb2 import StatusCodes
from spacefx.protos.sensor.Sensor_pb2 import SensorData

logger = spacefx.logger(level=logging.INFO)

# Add the protos and data generator paths
sys.path.append(os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__)))), ".protos", "datagenerator", "planetary_computer"))


def process_sensor_data(sensor_data):
    """
    Event handler for Sensor Updates
    """
    logger.info(f"Received sensor data:")
    logger.info(f"TrackingId: {sensor_data.responseHeader.trackingId}")

    if sensor_data.sensorID == "PlanetaryComputer":
        logger.info(f"Parsing PlanetaryComputer Sensor Data...")

        sensor_payload = EarthImageResponse()
        sensor_payload.ParseFromString(sensor_data.data.value)
        logger.info("Planetary Computer Sensor Data: %s", sensor_payload)

        logger.info(f"Asset: {sensor_payload.imageFiles[0].asset}")
        logger.info(f"Filename: {sensor_payload.imageFiles[0].fileName}")
        image_filename = sensor_payload.imageFiles[0].fileName  
    elif sensor_data.sensorID == "Thales-SEC-Cameras":
        logger.info("Attempting to parse message into object...")
        gpb_filename = gpb_str.ParseFromString(sensor_data.data.value)
        gpb_str()
        gpb_filename = gpb_filename.value
        logger.info(f"IMAGINe Sensor Data Filename:\n{gpb_filename.value}")
        image_filename = gpb_filename.value
    if sensor_data.sensorID != "PlanetaryComputer":
        logger.info(f"SensorID: {sensor_data.sensorID} is not PlanetaryComputer. Ignoring...")
        return

    logger.info('Retrieving sensor data...')
    inbox_path = str(spacefx.link.get_xfer_directories()["inbox"])
    sensor_data_file_path = f"{inbox_path}/{image_filename}"

    # Wait for the file to appear, using the associated linkResponse to indicate that the file is ready to use
    sensor_data_link_response_file_path = f"{inbox_path}/{sensor_payload.imageFiles[0].fileName}.linkResponse"

    logger.info(f"Waiting for {sensor_data_link_response_file_path}...")
    file_available_timeout = time.time() + 300  # seconds

    while (not os.path.isfile(sensor_data_link_response_file_path)) and (time.time() < file_available_timeout):
        time.sleep(1)

    if not os.path.isfile(sensor_data_link_response_file_path):
        logger.error(f"Failed to receive {sensor_data_link_response_file_path}")
        raise TimeoutError(f"Failed to receive {sensor_data_link_response_file_path}")


    logger.info(f"Sensor Image Received: {sensor_data_file_path}.  Processing...")
    ImageProcessor.add_image_to_queue(sensor_data_file_path)


def main():
    """
    Initialize SpaceFx, subscribe to sensor and heartbeat, and submit a request to sensor
    """

    print("Building Client...")
    spacefx.client.build()

    app_config = AppConfig()
    image_processor = ImageProcessor()
    logger.info("Image Processor Initialized")

    app_inbox = str(spacefx.link.get_xfer_directories()["inbox"])

    # Check to make sure sensor config file exists
    if os.path.isfile(f"{app_inbox}/{app_config.SENSOR_CONFIG_FILE}") is False:  
        raise FileNotFoundError(f"Sensor Config File {app_config.SENSOR_CONFIG_FILE} does not exist")

    if app_config.CONFIG_TYPE == "file":
        sensor_config = ImageFromFileConfig(file_path=f"{app_inbox}/{app_config.SENSOR_CONFIG_FILE}")
    elif app_config.CONFIG_TYPE == "test":
        sensor_config = PlanetaryComputerConfig(file_path=f"{app_inbox}/{app_config.SENSOR_CONFIG_FILE}")
    elif app_config.CONFIG_TYPE == "prod":
        sensor_config = IMAGINeCameraConfig(file_path=f"{app_inbox}/{app_config.SENSOR_CONFIG_FILE}")
    else:  
        raise ValueError(f"Invalid CONFIG_TYPE {app_config.CONFIG_TYPE}")

    for key, value in vars(app_config).items():
        logger.info(f"AppConfig {key} : {value}")
    for key, value in vars(sensor_config).items():
        logger.info(f"\tSensor Configuraiton {key}: {value}")

    if app_config.config_type == "file":
        # This is a special case where we don't use the sensor tasking capabilities of the space sdk;
        # Instead we just retrieve the test image from inbox directory
        ImageProcessor.add_image_to_queue(f"{app_inbox}/{sensor_config.filename}")
        spacefx.client.keep_app_open()
    else:

        logger.info("Subscribing to SensorData...")
        spacefx.sensor.subscribe_to_sensor_data(callback_function=process_sensor_data)

        logger.info("Querying sensors...")
        sensors_avaliable = spacefx.sensor.get_available_sensors()
        for sensor in sensors_avaliable.sensors:
            logger.info("Sensors Found: %s", sensor.sensorID)

        if sensor_config.sensor_name not in [sensor.sensorID for sensor in sensors_avaliable.sensors]:
            logger.error(f"{sensor_config.sensor_name} not found. Exiting...")
            raise ValueError(f"Sensor {sensor_config.sensor_name} not found in available sensors.")
        
        logger.info(f"Sensor Config specified sensor ({sensor_config.sensor_name}) found in available sensors.")
        logger.info(f"Tasking {sensor_config.sensor_name}...")
        sensor_tasking_request = sensor_config.create_tasking_request()

        # Use spacefx client to get the name of the current app
        payload_metadata = {"SOURCE_PAYLOAD_APP_ID": spacefx.client.get_app_id()}

        sensor_response = spacefx.sensor.sensor_tasking(
            sensor_config.sensor_name, 
            sensor_tasking_request, 
            metadata=payload_metadata
        )
        sensor_response_status = sensor_response.responseHeader.status
        logger.info(f"Sensor Tasking Request Status: {sensor_response_status}")
        
        if sensor_response.responseHeader.status in [StatusCodes.PENDING, StatusCodes.SUCCESSFUL]:
            logger.info("Waiting patiently for sensor data...")
            spacefx.client.keep_app_open()
        else:
            logger.info("Exiting Application: Sensor PlanetaryComputer tasking was not successful")


if __name__ == "__main__":
    logging.basicConfig(level=logging.DEBUG)
    main()
