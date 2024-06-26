# Azure Orbital Space SDK - Core Services

The Azure Orbital Space SDK provides a suite of core services designed to facilitate the development, deployment, and management of space-based applications. Below is an overview of each core service, with links to detailed documentation for further information.

## Core Services Overview

- **[Core Service](https://github.com/microsoft/azure-orbital-space-sdk-core)**: The foundational service that provides essential runtime services and payload application support. [Learn more](./core-services.md).

- **[Fileserver Service](https://github.com/microsoft/azure-orbital-space-sdk-coresvc-fileserver)**: A service optimized for high-throughput file transfers, supporting the efficient exchange of payload data and configuration files. [Learn more](./fileserver.md).

- **[Registry Service](https://github.com/microsoft/azure-orbital-space-sdk-coresvc-registry)**: Manages container images for on-orbit applications, facilitating secure storage, management, and deployment of software components. [Learn more](./registry.md).

- **[Switchboard Service](https://github.com/microsoft/azure-orbital-space-sdk-coresvc-switchboard)**: A secure MQTT message router for reliable and secure message routing between space-based application components. [Learn more](./switchboard.md).

Each of these services plays a crucial role in ensuring the reliability, security, and efficiency of space missions. By leveraging these core services, developers can focus on building innovative and robust space applications, confident in the support provided by the Azure Orbital Space SDK infrastructure.

## Extensibility and Customizations

The Azure Orbital Space SDK's plugin system offers extensive opportunities for customizing and extending the functionalities of Core Services to cater to specific mission requirements.

For detailed information on plugins, see **[Plugins](../plugins.md)**.