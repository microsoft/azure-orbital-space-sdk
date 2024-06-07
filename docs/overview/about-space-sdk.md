# About the Azure Orbital Space SDK

- [What is the Azure Orbital Space SDK?](#what-is-the-azure-orbital-space-sdk)
- [What problems does the Azure Orbital Space SDK address?](#what-problems-does-the-azure-orbital-space-sdk-address)
  - [The space industry is a difficult environment for modern software development.](#the-space-industry-is-a-difficult-environment-for-modern-software-development)
  - [Software development in the space industry is expensive.](#software-development-in-the-space-industry-is-expensive)
- [Who is the Azure Orbital Space SDK for?](#who-is-the-azure-orbital-space-sdk-for)
  - [Application Developers](#application-developers)
  - [Satellite Service Providers](#satellite-service-providers)
  - [Framework Developers](#framework-developers)
- [How does the Azure Orbital Space SDK work?](#how-does-the-azure-orbital-space-sdk-work)
- [How do I use the Azure Orbital Space SDK?](#how-do-i-use-the-azure-orbital-space-sdk)

## What is the Azure Orbital Space SDK?

The Azure Orbital Space SDK is a software development kit and runtime framework that makes it easy to develop and deploy applications to space.

As an application developer, the Azure Orbital Space SDK abstracts complex satellite systems and operations into simple components with clear, standardized interfaces. This allows you to focus on what matters - developing the applications you need on orbit. You don't need to know complicated avionics or have expensive custom hardware to be a space developer.

As a satellite service provider, the Azure Orbital Space SDK provides a lightweight, secure runtime framework that empowers adaptive satellite without impacting your critical systems and components. By shifting mission specification from low-level system hardware and firmware to high-level ephemeral software applications, satellites become generic, reuseable assets that can be modified on-orbit to meet the needs of you customers.

![This image provides a high-level architectural overview of the Azure Orbital Space SDK. On the left side of this image, an application kit is shown. This app kit virtualizes satellite subsystems to empower developers to create satellite agnostic applications in a development environment, such as an Azure virtual machine. On the right side of this image, a host platform is shown onboard spacecraft hardware. This host platform abstracts the same satellite subsystems that were virtualized on the left side of the image. Multiple payload applications developed from the left side of the image are seen being deployed to the spacecraft on the right side of the image. Each communicate to the satellite's subsystems through a shared host platform.](../img/AzureOrbitalSDK-Overview.png)

The Azure Orbital Space SDK Host Platform is a collection of microservices built on a common framework that's powered by open source technologies such as [Kubernetes](https://kubernetes.io) and [Dapr](https://dapr.io). The Azure Orbital Space SDK provides client libraries for application developers to interface with spacecraft in a standardized and predictable way. Host services abstract interactive logical components of a satellite to these developers, which satellite system providers can extend through plugins to translate the standardized requests from payload applications into the system-specific series of events needed to fulfil that request.

The Azure Orbital Space SDK runs on most linux platforms, and provides a number of tools and deployment options to make development, test, and integration efforts seamless for payload app developers and satellite service providers alike.

<!-- TODO: Add More Info Here? -->

For more details on these topics, please refer to our [Azure Orbital Space SDK Architectural Overview](../architecture/README.md).

## What problems does the Azure Orbital Space SDK address?

### The space industry is a difficult environment for modern software development.

As the world becomes more interconnected, modern software development has leaned into practices that increase speed, agility, and responsiveness. This often includes relying on cloud infrastructure and open source tools.

**Modern software development**

- An emphasis on agile design, iterative development, and continuous delivery
- Leverages automated testing to catch and fix issues early
- Prioritizes collaboration and communication between team members, partners, and stakeholders
- Utilizes open source software and tools
- Relies heavily on cloud computing and high-bandwidth, persistent connectivity
- Uses up-to-date tools and dependencies

Space, however, is not interconnected in the same way as is on Earth. Satellites only have connectivity to the rest of the world while in contact with a ground station, and are limited by the capabilities of their on-board antennae their distance from everything else. As a result satellites are inconsistently connected at best, and those connections are highly latent with limited bandwidth.

Infrastructure is also much more difficult to operate in space than on the Earth. If a machine isn't working right on the ground it's a simple process to reset it. If that doesn't work, it can be replaced quickly. In space, even the most basic of operations require careful planning, coordination, and monitoring. You can't send someone up to a satellite to get it working again, and replacing a broken satellite with a new one takes a long time and a lot of money. Space is a limited resource too, and there are many regulations outlining its use.

As a result, traditional space software development uses different practices than other software domains to accommodate for these unique needs and concerns.

**Traditional space software development**

- Emphasizes waterfall design, phase-gate development, and infrequent delivery
- May not perform significant testing until later phases of development
- Developers don't typically communicate directly with other teams, partners, or stakeholders
- Often uses proprietary software and tools
- Relies on specialized on-premises infrastructure
- Often uses legacy tools and dependencies

<!-- TODO: Explain how the SDK addresses this -->

### Software development in the space industry is expensive.

Traditional software development in the space industry is expensive for a number of reasons:

- High cost of specialized hardware and infrastructure requires to support development efforts
- High level of risk associated with space systems
- Long-term maintenance and support
- Complexity of space systems
- extensive testing and verification requirements
- extensive regulatory environment

<!-- TODO: Finish this section -->

## Who is the Azure Orbital Space SDK for?

The Azure Orbital Space SDK is for everyone! In general, we consider there to be three broad categories of users of the Azure Orbital Space SDK:
- Application Developers
- Satellite Service Providers
- Framework Developers

### Application Developers

An application developer is any person or entity that contributes to the development of a payload application with the Azure Orbital Space SDK. This could be a software engineer or data scientist that's developing a payload application to go into orbit for their organization. This could be a participant in a hackathon or a group leading an educational program that creates an application with the SDK. Individual hobbyists tinkering with SDK applications deployed to their homelabs would also be considered application developers.

Application developers work primarily with the Azure Orbital Space SDK client libraries. They may also create data generators and develop plugins for the Azure Orbital Space SDK Virtual Test Harness (VTH) to accelerate their development efforts and test their solutions.

<!-- TODO: Add links to:
- the client libraries
- data generator samples
- the virtual test harness -->

### Satellite Service Providers

A satellite service provider is any person or entity that integrates the Azure Orbital Space SDK runtime framework into satellites that are ultimately deployed into orbit. This may be as part of a satellite as a service offering to other customers, or as part of a satellite's technical stack used for their own internal purposes.

Satellite Service providers work primarily with plugins for Azure Orbital Space SDK's Host Services and Platform Services. They may also create data generators and develop plugins for the Azure Orbital Space SDK Virtual Test Harness (VTH) to accelerate their development efforts and test their solutions.

The service plugins, data generators, and VTH plugins created by these satellite service providers will likely be made available to the application developers that will be using the satellite service provider's satellites. This allows payload application developers to design, implement, and test their applications using the same satellite virtualization as the satellite service provider is using to design, implement, and validate their production satellite systems.

<!-- TODO: Add links to:
- host, platform, and core services
- host, platform, and core service plugins
- virtual test harness 
- virtual test harness plugins
- data generator samples-->

### Framework Developers

A framework developer is any person or entity that actively develops and maintains any component of the Azure Orbital Space SDK. As an example, the Azure Orbital Space SDK team under Azure Space and its individual members would all be considered framework developers. The same would be the case for any individual or group that contributed to any official Azure Orbital Space SDK repository.

<!-- TODO: Finish this section -->

## How does the Azure Orbital Space SDK work?

Want to know how the Azure Orbital Space SDK works? See our [Detailed Architecture Documentation](../architecture/README.md).

## How do I use the Azure Orbital Space SDK?

Ready to use the Azure Orbital Space SDK? Check out our [Getting Started Guide](../getting_started.md).
