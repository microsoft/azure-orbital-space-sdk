# Azure Orbital Space SDK

The Azure Orbital Space SDK is a software development kit and runtime framework that makes it easy to develop and deploy applications to space.

As an application developer, the Azure Orbital Space SDK abstracts complex satellite systems and operations into simple components with clear, standardized actions and interfaces. This allows you to focus on what matters - developing the applications you need on orbit.

As a satellite service provider, the Azure Orbital Space SDK provides a lightweight, secure runtime framework that allows your satellites to be treated as a generic compute platform. Through interface standardization satellites become reusable assets that can be modified on orbit to execute different missions through payload applications. Payload applications can be deployed to any of your satellites with zero downtime and no code modifications required.

![This image provides a high-level architectural overview of the Azure Orbital Space SDK. On the left side of this image, an application kit is shown. This app kit virtualizes satellite subsystems to empower developers to create satellite agnostic applications in a development environment, such as an Azure virtual machine. On the right side of this image, a host platform is shown onboard spacecraft hardware. This host platform abstracts the same satellite subsystems that were virtualized on the left side of the image. Multiple payload applications developed from the left side of the image are seen being deployed to the spacecraft on the right side of the image. Each communicate to the satellite's subsystems through a shared host platform.](./docs/img/AzureOrbitalSDK-Overview.png)

## Getting Started

First time working with the Azure Orbital Space SDK? Check out our [Getting Started Guide](./docs/getting_started.md).

## Overview

Want to learn more about what the Azure Orbital Space SDK is and how it works? Start with these resources:

- [About the Azure Orbital Space SDK](./docs/overview/about-space-sdk.md)
- [Detailed Architecture Documentation](./docs/architecture/README.md)
- [System Requirements](./docs/overview/requirements.md)

## Quick Starts and Tutorials

Ready to become a space software developer? Get started with our starter projects and guides:

- [Quick Starts and Tutorials](./docs/quick-starts/README.md)

## Sample Applications

- [Sample ONNX Application](./samples/payloadapps/python/shipdetector-onnx/placeholder)
- [Sample Tensorflow Application](./samples/payloadapps/python/shipdetector-tf/placeholder)
- [Starter .NET Application](./samples/payloadapps/dotnet/starter-app/placeholder)
- [Starter Python Application](./samples/payloadapps/python/starter-app/placeholder)

## Contributing

Find something you'd like to improve? See how on our [Contributing Guide](./CONTRIBUTING.md).

## Legal

Looking for our software license and other legal information? View our [Legal Guide](./LEGAL.md).

## Additional Resources

Searching for something else? Find it quickly in our [Table of Contents](./docs/table_of_contents.md).
