# About the Azure Orbital Space SDK

- [What is the Azure Orbital Space SDK?](#what-is-the-azure-orbital-space-sdk)
- [What problems does the Azure Orbital Space SDK address?](#what-problems-does-the-azure-orbital-space-sdk-address)
- [Who is the Azure Orbital Space SDK for?](#who-is-the-azure-orbital-space-sdk-for)
- [How does the Azure Orbital Space SDK work?](#how-does-the-azure-orbital-space-sdk-work)
- [How do I use the Azure Orbital Space SDK?](#how-do-i-use-the-azure-orbital-space-sdk)

## What is the Azure Orbital Space SDK?

The Azure Orbital Space SDK is a software development kit and runtime framework that makes it easy to develop and deploy applications to space.

As an application developer, the Azure Orbital Space SDK abstracts complex satellite systems and operations into simple components with clear, standardized actions and interfaces. This allows you to focus on what matters - developing the applications you need on orbit.

As a satellite service provider, the Azure Orbital Space SDK provides a lightweight, secure runtime framework that allows your satellites to be treated as a generic compute platform. Through interface standardization satellites become reusable assets that can be modified on orbit to execute different missions through payload applications. Payload applications can be deployed to any of your satellites with zero downtime and no code modifications required.

![This image provides a high-level architectural overview of the Azure Orbital Space SDK. On the left side of this image, an application kit is shown. This app kit virtualizes satellite subsystems to empower developers to create satellite agnostic applications in a development environment, such as an Azure virtual machine. On the right side of this image, a host platform is shown onboard spacecraft hardware. This host platform abstracts the same satellite subsystems that were virtualized on the left side of the image. Multiple payload applications developed from the left side of the image are seen being deployed to the spacecraft on the right side of the image. Each communicate to the satellite's subsystems through a shared host platform.](../img/AzureOrbitalSDK-Overview.png)

Unlike traditional space software frameworks, which are often proprietary and built on legacy tools, the Azure Orbital Space SDK runtime framework is built on modern open source and cloud technologies such as [Kubernetes](https://kubernetes.io) and [Dapr](https://dapr.io). Its source code is publicly available through [GitHub](https://github.com), and is developed using [Visual Studio Code](https://code.visualstudio.com) on [GitHub CodeSpaces](https://github.com/features/codespaces) or [Azure Virtual Machines](https://azure.microsoft.com/en-us/products/virtual-machines). We also provide guidance on how to deploy the Azure Orbital Space SDK on commodity single-board computers such as a [Raspberry Pi](https://www.raspberrypi.com). The Azure Orbital Space SDK is written in [.NET](https://dotnet.microsoft.com/en-us/learn/dotnet/what-is-dotnet) and exposes both .NET and [Python](https://www.python.org) client libraries so that application developers can use their favorite libraries such as [TensorFlow](https://www.tensorflow.org) or [ONNX](https://onnx.ai) to run new AI models in space or easily onboard existing legacy applications to our framework.

The Azure Orbital Space SDK Host Platform is a collection of microservices built on a common framework. We client libraries for application developers to interface with spacecraft in a standardized and predictable way. Host services abstract interactive logical components of a satellite to these developers, which satellite system providers can extend through the use of plugins.

These plugins implement the systems logic needed to convert generic requests from payload applications into their meaningful system-specific series of events. For example, a sensor service plugin could define how a payload application would interact with an imaging sensor onboard your satellite. A position service plugin could define how a payload application understands your satellite's location and orientation on orbit.

The Azure Orbital Space SDK runs on most linux platforms, and provides a number of tools and to make development, test, and deployment efforts seamless for payload app developers and satellite service providers alike.

The Azure Orbital Space SDK Virtual Test Harness (VTH) provides a means of replicating environments seen on orbit. The Virtual Test Harness (VTH) is logically separated from the Azure Orbital Space SDK runtime so that payload applications and service plugins run the exact same way in a test environment as they would on orbit. Through the use of data generators and VTH plugins developers can flexibly choose the fidelity and functionality needed in their simulation environment. 

Core and Platform Services provide a number of capabilities to satellite service providers to simplify on orbit operations. In general, these services manage communications between system components, secure access to the filesystem, and the lifecycle of kubernetes pods and their underlying docker images.

Interested in learning more about any of these topics? Please refer to our [Azure Orbital Space SDK Architecture](../architecture/architecture.md) documentation.

## What problems does the Azure Orbital Space SDK address?

### The space industry is a difficult environment for modern software development.

As the world becomes more interconnected, modern software development has leaned into practices that increase speed, agility, and responsiveness. This often includes relying on cloud infrastructure and open source tools.

**Modern Software Development**

- An emphasis on agile design, iterative development, and continuous delivery
- Leverages automated testing to catch and fix issues early
- Prioritizes collaboration and communication between team members, partners, and stakeholders
- Utilizes open source software and tools
- Relies heavily on cloud computing and high-bandwidth, persistent connectivity
- Uses up-to-date tools and dependencies

Space, however, is not interconnected in the same way as is on Earth. Satellites only have connectivity to the rest of the world while in contact with a ground station, and are limited by the capabilities of their on-board antennae their distance from everything else. As a result satellites are inconsistently connected at best, and those connections are highly latent with limited bandwidth.

Infrastructure is also much more difficult to operate in space than on the Earth. If a machine isn't working right on the ground it's a simple process to reset it. If that doesn't work, it can be replaced quickly. In space, even the most basic of operations require careful planning, coordination, and monitoring. You can't send someone up to a satellite to get it working again, and replacing a broken satellite with a new one takes a long time and a lot of money. Space is a limited resource too, and there are many regulations outlining its use.

As a result, traditional space software development uses different practices than other software domains to accommodate for these unique needs and concerns.

**Traditional Space Software Development**

- Emphasizes waterfall design, phase-gate development, and infrequent delivery
- May not perform significant testing until later phases of development
- Developers may have limited communication outside of their direct team
- Uses proprietary software and tools
- Relies on specialized on-premises infrastructure
- Satellites have low-bandwidth, latent connections to the ground for limited periods of time
- Uses legacy tools and dependencies

The Azure Orbital Space SDK addresses these differing needs through abstraction and standardization.

In traditional space software development satellite are bespoke assets that are built to achieve a specific mission. As a result, high-level software that executes this mission can not be tested until the low-level software that allows a satellite to function has been implemented. Futhermore, these two are tightly coupled. They will not function without the other, and they only work on the specific platform for which they were built.

Using the Azure Orbital Space SDK, the high-level software the executes a mission is logically separated from the low-level software that operates a satellite. This allows both to be developed and tested iteratively and independently. Low-level software is not designed with any specific mission in mind. High-level software is written to run on a generic compute platform in the form of a payload application. These are joined on orbit by the Azure Orbital Space SDK runtime framework.

The Azure Orbital Space SDK runtime framework is built on modern open source and cloud technologies. We provide tools and infrastructures for application developers and satellite service providers alike to develop using modern practices and design principles.

### Software development in the space industry is expensive.

Software development in the space industry is expensive for a number of reasons:

- Development required specialized hardware and infrastructure
- Space systems carry a higher level of risk than those on the ground
- Long-term maintenance and support is more expensive to maintain
- Space systems are very complex to manage and operate
- Space systems require extensive testing and verification requirements
- Space is a limited resource with strict regulatory requirements

<!-- TODO: Finish this section -->

The Azure Orbital Space SDK seeks to address these concerns through generalization, reuse, and leveraging open source and cloud technologies.

The Azure Orbital Space SDK provides a software development kit and runtime environment that generalizes and standardizes software development, deployment, and operations in space. This limits the risks that come with bespoke systems that get redesigned and reimplemented for each unique mission. By adhering to [fundamental principles of microservice design](https://learn.microsoft.com/en-us/azure/architecture/microservices/), the Azure Orbital Space SDK provides a framework that is flexible, resilient, and cost-efficient to develop and maintain.

Development with the Azure Orbital Space SDK is paired best with Azure's cloud infrastructure, which can provide many [cost saving benefits](https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/benefits-of-cloud-migration) to your team or organization. We also provide native support for [GitHub CodeSpaces](https://github.com/features/codespaces) and pair all components with [Visual Studio Code Dev Containers](https://code.visualstudio.com/docs/devcontainers/containers) so that development infrastructure is ephemeral and easily replaced. We work to get you started with Azure Orbital Space SDK with the bare minimum you need to be productive while giving you the tools and support to scale your infrastructure when you're ready.

## Who is the Azure Orbital Space SDK for?

The Azure Orbital Space SDK is intended for the following personas. Please refer to the links below to learn more.
- [Application Developers](../personas/application-developer.md)
- [Satellite Service Providers](../personas/satellite-owner-operator.md)
- [Framework Developers](../personas/framework-developer.md)

## How does the Azure Orbital Space SDK work?

Want to know how the Azure Orbital Space SDK works? See our [Architecture Documentation](../architecture/architecture.md).

## How do I use the Azure Orbital Space SDK?

Ready to use the Azure Orbital Space SDK? Check out our [Getting Started Guide](../getting_started.md).
