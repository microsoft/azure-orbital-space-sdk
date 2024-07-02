# Azure Orbital Space SDK - Developer Experience

This document serves as a comprehensive guide to the Azure Orbital Space SDK's developable components so that you can begin crafting efficient and reliable applications and plugins for space missions. With a focus on ease of use and comprehensive support, the SDK is designed to streamline the development process, allowing you to focus more on innovation and less on the complexities of space development.

## Payload Application Development

Developers can create payload applications using the Azure Orbital Space SDK in .NET and Python, two languages chosen for their robust ecosystems, versatility, and strong support for both high-level application logic and low-level system operations.

### Why .NET?

.NET is a versatile, open-source development platform maintained by Microsoft, making it a reliable choice for building high-performance applications. It offers:

- **Strong Type System**: Ensures more predictable code behavior and compile-time error checking, which is crucial for the reliability of space applications.
- **Extensive Libraries**: A vast collection of libraries and frameworks allows for the development of a wide range of applications within the space domain.
- **Legacy Code Integration**: .NET provides seamless interoperability with existing legacy code, including applications written in C or C++, enabling developers to leverage and extend previous investments in space application development.
- **Scalability and Performance**: High performance and easy scalability make it suitable for processing the large volumes of data typically generated in space missions.

### Why Python?

Python is known for its simplicity and readability, making it an excellent choice for both beginners and experienced developers. It is particularly favored for data-intensive applications due to:

- **Ease of Use**: Python's syntax is clear and concise, making it quick to learn and reducing the time required to develop and maintain applications.
- **Rich Ecosystem**: An extensive selection of libraries and frameworks, such as NumPy for numerical computations, Pandas for data analysis, and TensorFlow or ONNX for machine learning, supports complex data processing tasks common in space applications.
- **Flexibility**: Python's dynamic nature allows developers to experiment and iterate quickly, a valuable feature during the development of innovative space solutions.
- **Community Support**: A large and active community contributes to a wealth of resources, tools, and libraries, ensuring developers have access to the latest advancements in space application development.

### Getting Started with .NET Payload Application Development

Leverage the .NET framework to build efficient and reliable payload applications. The SDK provides a comprehensive set of .NET libraries tailored for space data processing, communication, and control systems.

<!-- TODO: Link to .NET documentation here -->

### Getting Started with Python Payload Application Development

Python's simplicity and vast ecosystem make it an ideal choice for payload application development, especially for data analysis and machine learning tasks.

<!-- TODO: Link to Python documentation here -->

## Plugin Development

Extend the functionality of the Azure Orbital Space SDK with custom plugins. Plugins can be developed to enhance the runtime framework, including core, host, and platform services. This modular approach allows developers to tailor the SDK to their specific mission needs, adding new features or integrating with external systems as required. Whether it's creating custom data processing algorithms, extending telemetry handling capabilities, or introducing new communication protocols, the plugin architecture ensures the SDK remains flexible and adaptable to meet the needs of the satellite payload.

<!-- TODO: Add links to each of these sections regarding plugin development -->

### Runtime Framework

The runtime framework serves as the backbone of your space applications, providing essential services and a runtime environment.

#### Core Services

Core services offer foundational capabilities such as authentication, logging, and data storage. Develop plugins to extend these services or integrate with external systems. Creating plugins for core services could enhance security through custom authentication methods, improve diagnostic capabilities with additional logging mechanisms, or offer tailored data storage solutions that meet specific mission requirements.

#### Host Services

Host services manage the interaction between your application and the spacecraft's hardware. Create plugins to support additional hardware components or custom data processing. Plugins for host services enable the integration of new or specialized hardware, enhancing the spacecraft's capabilities. They could also allow for the development of bespoke data processing algorithms. As an example, a sensor service plugin could be used to perform initial image correction prior to forwarding the captured data to the requesting payload application.

#### Platform Services

Platform services facilitate application deployment, configuration, and management. Plugins here can introduce new deployment strategies or configuration management tools. Developing plugins for platform services can modify the deployment process, introduce more additional configuration options, or tailor overall application management to the needs of the spacecraft, making it easier to adapt to changing mission demands and technological advancements.

### Virtual Test Harness (VTH)

Developing plugins for the Virtual Test Harness (VTH) enables developers to customize their testing environments by integrating custom data generators. This is vital for simulating specific data scenarios that applications will encounter in space, ensuring they are thoroughly tested and reliable. By connecting the VTH with tailored data generators, developers can accurately test and refine their applications against realistic conditions, improving the software's robustness and mission readiness.

<!-- TODO: Add links to VTH Plugin documentation -->