# Azure Orbital Space SDK - Platform Services

Platform Services serve as the foundational microservices layer, directly interfacing with spacecraft hardware to abstract complex on-board operations such as data translation and scheduling. These services offer a unified interface for both payload applications, through Host Services, and Satellite Owner Operators, streamlining interactions with the spacecraft's systems.

Positioned closer to the spacecraft's hardware, Platform Services provide a critical layer of abstraction, simplifying the complexities of spacecraft operations for Satellite Owner Operators and payload applications.

Due to their integral role and the need for secure operations, Platform Services are exclusively managed by Satellite Owner Operators. They are segregated from payload applications and Host Services within Kubernetes namespaces to ensure logical isolation and operational integrity.

## Platform Services Overview

- **[Message Translation Service (MTS)](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/tree/main/platform-mts)**: Facilitates the conversion of telemetry and sensor data from spacecraft-specific formats to a standardized format.

- **[Deployment Service](https://github.com/microsoft/Azure-Orbital-Space-SDK-Host-Services/tree/main/platform-deployment)**: Manages the deployment, updating, and termination of payload applications on the spacecraft.

## Extensibility and Customizations

The Azure Orbital Space SDK's plugin system offers extensive opportunities for customizing and extending the functionalities of Platform Services to cater to specific mission requirements.

For detailed information on plugins, see **[Plugins](../plugins.md)**.
