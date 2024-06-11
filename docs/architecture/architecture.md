# Azure Orbital Space SDK Architecture Overview

The Azure Orbital Space SDK consists of a runtime framework and a software development kit. Payload application developers use the software development kit to create applications... The runtime framework provides ...

## Runtime Framework

The runtime framework is a collection of microservices that run in a Kubernetes cluster. We provide the infrastructure to create and manage this cluster along with other supporting tools.

```mermaid
flowchart LR
    subgraph "Azure Orbital Space SDK - On Orbit"
        subgraph "Deployed Azure Orbital Space SDK Resources"
            direction LR
            subgraph "Payload Applications"
                direction TB
                Application-1(Application 1)
                Application-2(Application 2)
                ...
                Application-N(Application N)
            end
            subgraph "Host Services"
                direction TB
                Link
                Logging
                Position
                Sensor
            end
            subgraph "Platform Services"
                direction TB
                Deployment
                Message-Translation-Service(Message Translation Service)
            end
            subgraph "Core Services"
                direction TB
                Fileserver
                Registry
                Switchboard
            end
        end
        subgraph "On Orbit Architecture"
            subgraph "Satellite Payload"
                subgraph "General Compute Payload"
                    Payload-Applications(Payload Applications)
                    Host-Services(Host Services)
                    Platform-Services(Platform Services)
                    Payload-Applications  <-. Dapr PubSub .-> Host-Services
                    Host-Services <-. Dapr PubSub .-> Platform-Services
                end
                subgraph "Command and Data Handling Payload"
                    Command-and-Data-Handler(Command and Data Handler)
                    Command-and-Data-Handler <--> Platform-Services
                end
                subgraph "Satellite Subsystem Payloads"
                    Satellite-Subsystems(Satellite Subsystems)
                    Satellite-Subsystems <--> Command-and-Data-Handler
                end
            end
        end
    end
```

[Azure Orbital Space SDK Runtime Framework](./runtime-framework/runtime-framework.md)

## Software Development Kit

The Azure Orbital Space SDK software development kit is a comprehensive collection of tools, libraries, code samples, and other resources to facilitate development on the Azure Orbital Space SDK runtime framework. Additionally, the software development kit provides extensive documentation with API references, tutorials, and samples so that developers can quickly and efficiently build applications for the Azure Orbital Space SDK runtime framework.The SDK is designed to support developers at every stage, making it easier to create and deploy space-based applications.

[Azure Orbital Space SDK Software Development Kit](./software-development-kit/software-development-kit.md)