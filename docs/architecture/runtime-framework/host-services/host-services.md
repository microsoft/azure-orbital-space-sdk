# Azure Orbital Space SDK  - Host Services

The Azure Orbital Space SDK provides a suite of host services that facilitate interactivity between payload applications and satellite subsystems and data. Through host services, payload applications can request spacecraft telemetry and sensor data, coordinate communication and file transfer with Earth, and emit detailed logging information for further analysis on the ground.

## Host Services Overview

- **[Link Service](./link.md)**: Offers a robust mechanism for transferring files between components of the runtime framework. It may also be used to stage files so they may be downlinked to the ground for further analysis.

- **[Logging Service](./logging.md)**: Captures detailed logs from payload applications and satellite subsystems, facilitating debugging, performance monitoring, and operational analysis.

- **[Position Service](./position.md)**: Reports a satellite's current position, orientation, and other navigational data to payload applications. This service is beneficial for applications that have position dependent processing, such as oceanic observation.

- **[Sensor Service](./sensor.md)**: Provides access to raw and processed data from onboard sensor. This service is critical for payload applications performing environmental monitoring, Earth observation, or other sensor-based missions.

Each of these host services is designed to enhance the capabilities of payload applications and improve the efficiency and reliability of satellite operations. By leveraging these services, developers can create more sophisticated and responsive space applications, capable of performing complex tasks and responding dynamically to on-orbit conditions.

## Extensibility and Customizations

The Azure Orbital Space SDK's plugin system offers extensive opportunities for customizing and extending the functionalities of Host Services to cater to specific mission requirements.

For detailed information on plugins, see **[Plugins](../plugins.md)**.