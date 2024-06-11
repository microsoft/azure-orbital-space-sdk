# Azure Orbital Space SDK - Runtime Framework Architecture

In the mermaid diagram below, you'll see three components that illustrate the utility of the Azure Orbital Space SDK.

## On Orbit

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

## Simulation in the Loop (SITL)

```mermaid
flowchart LR
    subgraph "Azure Orbital Space SDK - Simulation in the Loop (SITL)"
        subgraph "Deployed Azure Orbital Space SDK Resources"
            direction LR
            subgraph "Payload Applications"
                    direction TB
                    Application-1(Application 1)
                    Application-2(Application 2)
                    ...(...)
                    Application-N(Application-N)
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
            subgraph "Virtual Test Harness"
                direction LR
                Virtual-Test-Harness(Virtual Test Harness)
                subgraph "Data Generators"
                    direction TB
                    Data-Generator-1(Data Generator 1)
                    Data-Generator-2(Data Generator 2)
                    ....(...)
                    Data-Generator-N(Data Generator N)
                end
            end
        end
        subgraph "Simulation in the Loop"
            subgraph "Virtual Satellite Payload"
                subgraph "Virtual General Compute Payload"
                    Payload-Applications(Payload Applications)
                    Host-Services(Host Services)
                    Platform-Services(Platform Services)
                    Payload-Applications  <-. Dapr PubSub .-> Host-Services
                    Host-Services <-. Dapr PubSub .-> Platform-Services
                end
                subgraph "Virtual Test Harness"
                    Data-Generators(Data Generators)
                    VTH(Virtual Test Harness)
                    VTH <--> Platform-Services
                    Data-Generators <--> VTH
                end
            end
        end
    end
```

## Hardware in the Loop (HITL)

```mermaid
flowchart LR
    subgraph "Azure Orbital Space SDK - Hardware in the Loop (HITL)"
        subgraph "Deployed Azure Orbital Space SDK Resources"
            direction LR
            subgraph "Payload Applications"
                    direction TB
                    Application-1(Application 1)
                    Application-2(Application 2)
                    ...(...)
                    Application-N(Application-N)
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
            subgraph "Virtual Test Harness"
                direction LR
                Virtual-Test-Harness(Virtual Test Harness)
                subgraph "Data Generators"
                    direction TB
                    Data-Generator-1(Data Generator 1)
                    Data-Generator-2(Data Generator 2)
                    ....(...)
                    Data-Generator-N(Data Generator N)
                end
            end
        end
        subgraph "Hardware in the Loop"
            subgraph "Virtual Satellite Payload"
                subgraph "Virtual General Compute Payload"
                    Payload-Applications(Payload Applications)
                    Host-Services(Host Services)
                    Platform-Services(Platform Services)
                    Payload-Applications  <-. Dapr PubSub .-> Host-Services
                    Host-Services <-. Dapr PubSub .-> Platform-Services
                end
                subgraph "Virtual Test Harness"
                    Data-Generators(Data Generators)
                    VTH(Virtual Test Harness)
                    VTH <--> Platform-Services
                    Data-Generators <--> VTH
                end
            end
            subgraph "Satellite Payload"
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

## Kubernetes Services

In production, on orbit contexts we deploy the following services.

### Production Services

| Service Name                          | Service Type          | Kubernetes Pod Name    | Kubernetes Namespace |
| :------------------------------------ | :-------------------- | :--------------------- | :------------------- |
| **Filserver**                         | Core Service          | core-fileserver        | core                 |
| **Registry**                          | Core Service          | core-registry          | core                 |
| **Switchboard**                       | Core Service          | core-switchboard       | core                 |
| **Message Translation Service (MTS)** | Platform Service      | platformsvc-mts        | platformsvc          |
| **Deployment**                        | Platform Service      | platformsvc-deployment | platformsvc          |
| **Sensor**                            | Host Service          | hostsvc-sensor         | hostsvc              |
| **Link**                              | Host Service          | hostsvc-link           | hostsvc              |
| **Logging**                           | Host Service          | hostsvc-logging        | hostsvc              |
| **Position**                          | Host Service          | hostsvc-position       | hostsvc              |
| _One or More Payload Applications_    | _Payload Application_ | _payloadapp-*_         | _payloadapp_         |

<!-- TODO: Replace bold with links to repos -->

Most of these services are also deployed with a [Dapr Sidecar (daprd)](https://docs.dapr.io/concepts/dapr-services/sidecar/) so that they may communicate with one another. One or more payload application may also be deployed to this cluster via the deployment service.

In development and tests contexts, we additionally deploy the Azure Orbital Space SDK Virtual Test Harness (VTH).

### Development and Test Services

| Service Name                   | Service Type     | Kubernetes Pod Name    | Kubernetes Namespace |
| :----------------------------- | :--------------- | :--------------------- | :------------------- |
| **Virtual Test Harness (VTH)** | VTH              | vth                    | ???                  |
| _One or More Data Generators_  | _Data Generator_ | ???                    | ???                  |