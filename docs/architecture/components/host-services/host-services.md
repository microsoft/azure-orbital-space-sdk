# Host Services

Host Services are microservices responsible for providing the payload application with the ability to interact with spacecraft telemetry and sensor data and coordinating communication with the ground and abstracting implementation details about the spacecraft and its hardware.

The Azure Orbital Space SDK exposes the Host Services via client libraries in [.NET](https://github.com/microsoft/Azure-Orbital-Space-SDK-Client-Library-dotnet) and [python](https://github.com/microsoft/Azure-Orbital-Space-SDK-Client-Library-python) to provide payload applications:

- the **[Sensor Service](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/tree/main/hostsvc-sensor)** to subscribe to spacecraft telemetry for sensor data
- the **[Position Service](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/tree/main/hostsvc-position)** to subscribe to spacecraft telemetry for positioning
- the **[Link Service](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/tree/main/hostsvc-link)** to send and receive messages to and from the ground
- the **[Logging Service](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/tree/main/hostsvc-logging)** to write to logs

## Source

See the [Azure Orbital Space SDK Host Services](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/) repository to get started with the Host Services.

## Extensibility and Customizations

To extend and customize behavior of the Host Services, the Azure Orbital Space SDK provides a plugin system.

See **[Plugins](./plugins.md)** for more information.
