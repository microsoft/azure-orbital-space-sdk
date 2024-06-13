# Azure Orbital Space SDK Python Client Library

The Azure Orbital Space SDK Python Client Library provides developers with a comprehensive set of interfaces for interacting with satellite subsystems. This library abstracts the complexities of satellite system operation and usage, allowing you as the developer to develop innovative application without needing to know the details of the underlying satellite technology.

## Components of the Azure Orbital Space SDK Python Client Library

Each component of the Azure Orbital Space SDK Python Client Library plays a crucial role in simplifying interactions with the satellite. From establishing communication links to handling sensor data, these modules work together to provide a seamless experience. Below, we highlight each component of the client library.

### `spacefx`

The `spacefx` module serves as the core of the Azure Orbital Space SDK Python Client Library and equips python developers with a broad array of tools to seamlessly interact with satellite platforms without needing to know the details of the satellite itself.

### `spacefx.client`

The [`spacefx.client`](./client/client.md) module is the core interface for instantiating and interacting with the Azure Orbital Space SDK runtime framework. It provides essential functionalities such as initializing the client itself and managing application your application's runtime configuration.

### `spacefx.link`

The [`spacefx.link`](./link/link.md) module provides functionalities for managing file transfers between your application host services, and other applications running within the Azure Orbital Space SDK runtime framework. It also provides methods to stage files to transfer to the ground via downlink or transfer to other satellites running the Azure Orbital Space SDK runtime framework via crosslink.

### `spacefx.logging`

The [`spacefx.logging`](./logging/logging.md) module provides functionality to log information and send telemetry. The logging host service has integrations with the link host service which facilitates the transfer of this data to the ground via downlinks. This allows applications to send log messages and telemetry data directly to the Azure Orbital Space SDK runtime framework, facilitating debugging and monitoring of applications and services running within the framework.

This module is essential for applications that require detailed logging and telemetry to ensure optimal performance and reliability. It supports various logging levels and telemetry metrics, enabling developers to capture and analyze detailed information about their application's behavior and performance.

### `spacefx.logger`

The [`spacefx.logger`](./logging/logging.md) module defines a subclass of Python's native `logging.Logger` class, integrated with the functionality provided by `spacefx.logging`. This integration allows application developers to log their applications as they would with any standard Python application, but with the added benefit of the logs being managed by the Azure Orbital Space SDK's logging services. This ensures that logs are automatically staged for downlink to the ground for further analysis and monitoring.

### `spacefx.position`

The [`spacefx.position`](./position/position.md) module provides functionality for payload applications to determine the current location of a satellite. This helps facilitate applications running position-sensitive operations, such as oceanic observation.

### `spacefx.sensor`

The [`spacefx.sensor`](./sensor/sensor.md) module is designed to interact with various sensors on a satellite platform, providing functionalities for querying available sensors, subscribing to sensor data, and tasking sensors for specific operations.

In our sample applications these sensors are typically imaging sensors, but any systems that collects and reports a measurement could be exposed as sensor service. This could include temperature readings, radiation monitors, or magnetometers for example.

### `spacefx.protos`

The [`spacefx.protos`](protos/protos.md) module exposes the [protocol buffer (protobuf)](https://protobuf.dev/overview/) Python language bindings for the Azure Orbital Space SDK. Protocol buffers are language-neutral, platform-neutral, extensible mechanisms for serializing structured data. They are similar to XML but smaller, faster, and simpler to use. In the context of the Azure Orbital Space SDK, protobufs are used to define the structure of data exchanged between your application and the Azure Orbital Space SDK runtime framework, ensuring strong typing and efficient data serialization.

## Using the Azure Orbital Space SDK Python Client Library

TODO: Fill this out once the devcontainer feature is complete and starter apps are available.

### Declaring Project Dependencies

The Azure Orbital Space SDK Python Client Library requires the following to be added to your python project's `pyproject.toml`. To make things simple, we expose an option in our [custom devcontainer feature](placeholder) to automatically inject these dependencies into your project's `pyproject.toml` if you so wish.

```toml
[tool.poetry.dependencies]
grpcio-tools = "^1.26.0"
grpcio = "^1.26.0"
protobuf = "^3.20.1"

[tool.poetry.dependencies.microsoftazurespacefx]
path = ".wheel/microsoftazurespacefx-0.10.0-py3-none-any.whl"
develop = false

[tool.poetry.group.spacefx-dev.dependencies]
pytest = "^7.2.1"
pip = "*"
setuptools = "*"
wheel = "*"
build = "*"
mypy = "*"
mypy-extensions = "*"
mypy-protobuf = "*"
debugpy = "*"
```

### Installing the Azure Orbital Space SDK Python Client Library Wheel

The Azure Orbital Space SDK Python Client Library has not yet been published as a package to PyPI. As such, the client library must be installed directly from a wheel, which we publish as one of our many SDK artifacts. If using our [custom devcontainer feature](placeholder), the process outlined below is automatically taken care of for you.

1. In you project's root directory, create the `.wheel` directory.
1. Download the [microsoftazurespacefx python wheel]().
1. Place the wheel in the `.wheel` directory.

### Updating the Azure Orbital Space SDK Python Client Library Wheel

To update the wheel to a never version, simply delete your existing wheel and either let our custom devcontainer feature download the latest wheel for you, or perform the above process manually with the wheel version of your choice.

## An Example Python Payload Application

```python
import logging
import os
import sys
import time

import spacefx

from spacefx.protos.sensor.Sensor_pb2 import SensorData
from spacefx.protos.common.Common_pb2 import StatusCodes

# Use the Azure Orbital Space SDK logging service
logger = spacefx.logger(level=logging.INFO)


def process_sensor_data(sensor_data: SensorData):
    """
    This function takes a SensorData object and prints its contents.
    """
    logger.info(f"Received sensor data:")
    logger.info(f"TrackingId: {sensor_data.responseHeader.trackingId}")
    logger.info(f"SensorID: {sensor_data.sensorID}")
    logger.info(f"Data: {sensor_data.data}")


def sensor_service():
    """
    This function queries sensor service for the list of available sensors,
    then performs both a SensorTaskingPreCheck and SensorTaskingRequest to
    the 'DemoTemperatureSensor'.
    """
    logger.info("----SENSOR SERVICE: START-----")

    # Subscribe to process_sensor_data function as a sensor data callback
    # so that the function will trigger each time a new SensorData message is received
    spacefx.sensor.subscribe_to_sensor_data(callback_function=process_sensor_data)

    # Query available sensors
    logger.info("Querying available sensors")
    sensor_response = spacefx.sensor.get_available_sensors()
    logger.info("Sensor Response Heard")
    logger.info(f"    AppID: {sensor_response.responseHeader.appId}")
    logger.info(f"    TrackingId: {sensor_response.responseHeader.trackingId}")
    logger.info(f"    Status: {StatusCodes.Name(sensor_response.responseHeader.status)}")
    logger.info(f"    Message: {sensor_response.responseHeader.message}")
    logger.info(f"    Sensors: {sensor_response.sensors}")

    # Send a SensorTaskingPrecheck to DemoTemperatureSensor
    logger.info("Triggering a Tasking PreCheck for DemoTemperatureSensor")
    request_data = SensorData()
    payload_metadata = {"SOURCE_PAYLOAD_APP_ID": "samplepythonapplication"}
    tasking_precheck_response = spacefx.sensor.sensor_tasking_pre_check(
        "DemoTemperatureSensor",
        request_data=request_data,
        metadata=payload_metadata
    )
    logger.info(f"Response: {StatusCodes.Name(tasking_precheck_response.responseHeader.status)}")

    # Send a SensorTaskingRequest to DemoTemoeratureSensor
    logger.info("Triggering a Tasking for DemoTemperatureSensor")
    request_data = SensorData()
    payload_metadata = {"SOURCE_PAYLOAD_APP_ID": "samplepythonapplication"}
    tasking_response = spacefx.sensor.sensor_tasking(
        "DemoTemperatureSensor", 
        request_data=request_data,
        metadata=payload_metadata
    )
    logger.info(f"Response: {StatusCodes.Name(tasking_response.responseHeader.status)}")
    logger.info("----SENSOR SERVICE: END-----")


def link_service():
    """
    This function queries link service for the application's transfer directories.
    It also uses link service to send a file to another application (itself in this case).
    """
    logger.info("----LINK SERVICE: START-----")

    # Query link service for the application's transfer directories
    logger.info("Querying for xfer Directories...")
    xfer_directory = spacefx.link.get_xfer_directories()
    logger.info("Outbox: %s" % xfer_directory['outbox'])
    logger.info("Inbox: %s" % xfer_directory['inbox'])
    logger.info("Root: %s" % xfer_directory['root'])

    # Use link service to have the app send a file to itself
    logger.info("Sending file this app...")
    link_response = spacefx.link.send_file_to_app(
        "samplepythonapplication",
        f"/a/path/to/some/file",
        overwrite_destination_file=True
    )
    logger.info(f"Result: {StatusCodes.Name(link_response.responseHeader.status)}")
    logger.info("----LINK SERVICE: END-----")


def logging_service():
    """
    This function sends a telemetry message to logging service, then sends
    1000 logs to the logging service.
    """
    logger.info("----LOGGING SERVICE: START-----")

    # Send a telemetry message to the logging service
    logger.info("Sending a telemetry message to the logging service...")
    telemetry_response = spacefx.logging.send_telemetry("test_metric", 12345)
    logger.info(f"Telemetry Response: {StatusCodes.Name(telemetry_response.responseHeader.status)}")

    # Send 1000 debug logs to the logging service
    logger.info("Triggering 1000 logs to the logging service at debug level...")
    for i in range(1000):
        logger.debug("Trigger log #%s" % i)

    logger.info("Successfully triggered 1000 logs to the logging service")
    logger.info("----LOGGING SERVICE: END-----")


def position_service():
    """
    This function queries for the current satellite position from the position service
    """
    logger.info("----POSITION SERVICE: START-----")

    # Query the satellite's current position
    logger.info("Querying for current position")
    current_pos = spacefx.position.request_position()
    logger.info(f"Status: {StatusCodes.Name(current_pos.responseHeader.status)}")
    logger.info(f"Current position: {current_pos.position.point}")
    logger.info("----POSITION SERVICE: END-----")


def main():
    """
    This method initializes the Azure Orbital Space SDK Client,
    then runs a number of methods that demonstrate's functionality of each component.
    """

    # Initialize the Azure Orbital Space SDK Client
    print("Building SpaceFX Client")
    spacefx.client.build()

    # Get the application ID for this application
    appid = spacefx.client.get_app_id()
    logger.info(f"AppID: {appid}")

    # Get the configuration directory for this application
    config_dir = spacefx.client.get_config_dir()
    logger.info(f"SpaceFx Configuration Directory: {config_dir}")

    # Listen for service heartbeats, then report which services are online
    logger.info("Sleeping for 5 seconds to allow for heartbeats to trickle in...")
    time.sleep(5)
    logger.info("Listing services online (heartbeats heard)")
    services_online = spacefx.client.services_online()
    for _, appId in enumerate(services_online):
        logger.info(f"    AppID: {appId.AppId}")

    # Run functionality from each of the host services
    position_service()
    sensor_service()
    link_service()
    logging_service()

    # Keep the application open indefinitely (useful for debugging)
    logger.info("Debugging complete! Keeping application open...")
    spacefx.client.keep_app_open()


if __name__ == '__main__':
    main()
```