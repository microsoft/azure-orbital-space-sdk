# Azure Orbital Space SDK Virtual Test Harness (VTH)

The Azure Orbital Space SDK Virtual Test Harness (VTH) is an advanced abstraction tool that virtually represents satellite hardware and subsystem capabilities. This allows application developers to develop and test their space-based applications against an environment that closely mirrors the actual satellite systems they will interact with in orbit. By abstracting satellite hardware into a virtual environment, the VTH enables developers to validate application functionality, ensure compatibility with satellite systems, and fine-tune performance without the need for physical hardware. This approach significantly accelerates the development cycle, reduces costs, and increases the reliability of applications destined for space operations.

## Overview

The Azure Orbital Space SDK Virtual Test Harness (VTH) virtualizes the operational capabilities of a physical satellite platform. Like the Azure Orbital Space SDK Runtime Framework, the VTH relies on a plugin architecture so that developers can extend its functionality, giving developers the customization and flexibility they need to define their virtual environments. These plugins facilitate interaction with data generators that provide diverse satellite data for application testing and development.

Data generators are tools that produce synthetic data that mimics the output of satellite sensors and systems. This could include satellite position, orientation, and sensor imagery, as examples. This capability is crucial for developers, as it allows them to test how their applications interact with the satellite payload and process its data without needing actual satellite hardware. By interfacing with these data generators, the VTH enables developers to create realistic and complex scenarios, testing applications under a wide range of conditions to ensure robustness, accuracy, and reliability before deployment in space.

To bridge the gap between virtual testing and real-world application, the Azure Orbital Space SDK Virtual Test Harness (VTH) can be used in both Simulation in the Loop (SITL) and Hardware in the Loop (HITL) configurations. These approaches represent a continuum of testing environments, from fully virtualized simulations to integrations with actual hardware components. SITL focuses on simulating the entire satellite system within a virtual environment, allowing developers to test applications against a broad spectrum of simulated data and scenarios without the need for physical hardware. This method is invaluable for early-stage development and debugging, offering a cost-effective and flexible testing solution.

Transitioning from SITL, HITL incorporates real hardware components into the testing loop, providing a more accurate representation of operational conditions. This hybrid approach enables developers to validate their applications against the physical characteristics and constraints of actual satellite hardware, ensuring compatibility and performance under real-world conditions. By leveraging both SITL and HITL, developers can comprehensively test and refine their space-based applications, ensuring they are robust, reliable, and ready for deployment in the challenging environment of space.

### Simulation in the Loop (SITL)

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

### Hardware in the Loop (HITL)

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

## Components

The Azure Orbital Space SDK Virtual Test Harness (VTH) comprises several key components designed to offer a comprehensive and realistic testing environment for space-based applications. These components are crucial for ensuring that applications are robust, reliable, and ready for deployment in the challenging environment of space.

### Virtual Test Harness (VTH)

The Virtual Test Harness (VTH) serves as the cornerstone of the Azure Orbital Space SDK's testing framework. It is a sophisticated simulation tool that abstracts the satellite hardware and subsystem capabilities into a virtual environment. This abstraction allows developers to test and validate their applications against a highly accurate representation of satellite systems without the need for physical hardware. The VTH supports a wide range of testing scenarios, from simple unit tests to complex integration and performance testing, providing developers with the flexibility to tailor their testing approach to their specific needs.

#### Key Features:

- **Dop-In Replacement for Hardware**: Payload Applications and Host Services can communicate with either the VTH or production hardware with zero code changes required.
- **Plugin Architecture**: Allows for the extension and customization of the VTH, enabling developers to simulate specific satellite models and configurations.
- **Integration with Data Generators**: Facilitates testing under various conditions by providing synthetic data that replicates the output of satellite sensors and systems.

#### How it Works - Image Acquisition Example

Here's a simplified example showing the component-level interactions of a payload application requesting an image of the Microsoft campus from a spacecraft's camera sensor on orbit:

```mermaid
sequenceDiagram
    participant app as MySampleApp
    participant sensor as Sensor Service
    participant mts as MTS
    participant mtsPlugin as MTS Plugin
    participant c&dh as C&DH
    participant camera as Camera

    app           ->> sensor:    Give me an image for <br/> (47.6405, -122.1468, 47.6477, -122.1269)
    sensor        ->> mts:       Give MySampleApp an image for <br/> (47.6405, -122.1468, 47.6477, -122.1269)
    mts           ->> c&dh:      MTS Plugin translates Request <br/> from SDK format to Camera format
    c&dh          -> camera:     (spacecraft internals)
    c&dh          ->> mts:       MTS Plugin translates Response <br/> from Camera format to SDK format  
    mts           ->> sensor:    Return this image to MySampleApp
    sensor        ->> app:       Here's your image for <br/> (47.6405, -122.1468, 47.6477, -122.1269)
```

Here's what that same request would look like using the Virtual Test Harness with a data generator that mimics the behavior of the on orbit camera sensor:

```mermaid
sequenceDiagram
    participant app as MySampleApp
    participant sensor as Sensor Service
    participant mts as MTS
    participant mtsPlugin as MTS Plugin
    participant vth as VTH
    participant vthPlugin as VTH Plugin
    participant datagenerator as Data Generator

    app           ->> sensor:        Give me an image for <br/> (47.6405, -122.1468, 47.6477, -122.1269)
    sensor        ->> mts:           Give MySampleApp an image for <br/> (47.6405, -122.1468, 47.6477, -122.1269)
    mts           ->> vth:           MTS Plugin translates Request <br/> from SDK format to Camera format
    vth           ->> datagenerator: VTH Plugin Translates Request <br/> from Camera format to Data Generator format
    datagenerator ->> vth:           VTH Plugin Translates Response <br/> from Data Generator format to Camera format
    vth           ->> mts:           MTS Plugin Translates Response <br/> from Data Generator format to SDK format  
    mts           ->> sensor:        Return this image to MySampleApp
    sensor        ->> app:           Here's your image for <br/> (47.6405, -122.1468, 47.6477, -122.1269)
```

By replacing the spacecraft and its sensors with a data generator and the VTH, the application can be tested in a predictable and repeatable fashion. The application has know knowledge that it's communicating with a data generator instead of a spacecraft sensor.

### Data Generators

Data Generators are an integral part of the Virtual Test Harness, providing the synthetic data necessary for simulating real-world satellite operations. These tools generate a wide range of data, including satellite telemetry, sensor outputs, and environmental conditions, enabling developers to test how their applications will perform in orbit.

#### Key Features:

- **Diverse Data Production**: Generates a wide variety of data types, from simple telemetry to complex sensor imagery, to support comprehensive testing.
- **Customizable Scenarios**: Allows developers to create specific testing scenarios, including failure modes and edge cases, to ensure thorough application validation.
- **Seamless Integration**: Works in conjunction with the VTH to provide a cohesive and realistic testing environment.

By leveraging the capabilities of the Virtual Test Harness and Data Generators, developers can ensure that their space-based applications are thoroughly tested and validated, reducing the risk of failure and ensuring compatibility with satellite systems. This comprehensive testing framework is essential for the development of reliable and robust applications capable of withstanding the rigors of space operations.

To learn more about data generators, including data generators officially created by the Azure Orbital Space SDK team, see our [Azure Orbital Space SDK Data Generators Documentation](./data-generators/data-generators.md).