# What does the Azure Orbital Space SDK architecture look like for communication

Now that we've resolved many of the core challenges, what does it look like for our applications to communicate from the payload application, the whole way down to the data generator and back.

The flow is the following (you can also run the demo described here using our walkthrough for [dotnet](https://github.com/microsoft/Azure-Orbital-Space-SDK-QuickStarts/blob/main/tutorials/quick-start-tutorials/e2e-eo-sample-dotnet.md) or [python](https://github.com/microsoft/Azure-Orbital-Space-SDK-QuickStarts/blob/main/tutorials/quick-start-tutorials/e2e-eo-sample-python.md):

```mermaid
sequenceDiagram
    Sample App->>hostsvc-sensor: Make a Sensor Tasking Request
    hostsvc-sensor->>platform-mts: Make a sensor request
    platform-mts->>MtsPluginVth: The call is routed to the MTS plugin
    MtsPluginVth->>vth: Sensor request is forwarded
    vth->>PlanetaryComputerGeotiffPluginVth: Sensor request is forwarded
    PlanetaryComputerGeotiffPluginVth->>PlanetaryComputerGeotiffPluginVth: converted SensorRequest to a EarthImageRequest
    PlanetaryComputerGeotiffPluginVth->>tool-planetary-computer-geotiff: EarthImageRequest is forwarded
    tool-planetary-computer-geotiff->>tool-planetary-computer-geotiff: store output image /var/spacedev/tmp/vth/output shared volume
    tool-planetary-computer-geotiff->>PlanetaryComputerGeotiffPluginVth: Send EarthImageResponse with filename of output
    PlanetaryComputerGeotiffPluginVth->>vth: send Sensor Response / Data
    vth->>MtsPluginVth: Send Sensor Response / Data
    MtsPluginVth->>platform-mts: Send Sensor Response / Data
    platform-mts->>hostsvc-sensor: Send Sensor Response / Data
    hostsvc-sensor->>Sample App: Send back sensor response / data
```

# What are plugins?

To faciliate the level of abstraction required to build our SDK, we created host services, which are docker container applications that our client libraries make requests of to interact with the hardware. Recognizing that our Satellite Owner / Operator (SOO) customers have many different requirements and needs, we make all of our services extensible by plugins.

Plugins in this case can overload the operations within the host services, platform services, and even the Virtual Test Harness (VTH), to allow for the SOO to customize what the operations inside those host services do. This provides a high level of flexibility to our SOO customers to expose functionality to their customers.

# What is Message Translation Service (MTS)?

The key element of communication between the payload app computer, and the underlying CnDH is the implementation of the Message Translation Service (MTS). The MTS is the point by which the messages coming from the payload app, and then host services can communicate with the spacecraft. By funnelling requests through the MTS we create an easy point for the Satellite Owner / Operators (SOOs) to interact with their own APIs for the CnDH. This interaction is done via a plugin.

# What is Helm?

Helm is the application package manager for Kubernetes that you use to standardize and simplify the deployment of cloud-native applications on Kubernetes. It can also be descirbed as the package manager for Kubernetes. Helm makes it easier for us to package and deploy software on a Kubernetes cluster using charts.

Overall Helm allows for us to be more flexible and tailor it to the enviornment as needed. All while becoming more advanced.

### Kubernetes Namespace Configuration and Traffic

![TODO: ADD ALT TEXT!](../img/kubernetes_namespace_traffic.png)

The key takeaways for the above diagram:

- Each microservice and app has a dedicated SMB share via Core-FileServer (Xfer and Plugins)
  - Hostsvc-Link has an additional admin share in Core-FileServer to faciliate file transfers between apps
- Payload Apps access the Host Services via MQTT
  - Payload Apps are blocked from accessing Platform Services
- Host Services access Platform Services via MQTT
- All MQTT exchanges are facilitated by dapr via Core-Switchboard
  - Dapr is auto-injected and configured by the Microsoft Azure Orbital Space SDK
- A Local Container Registry called "core-registry" is deployed and configured to act as an image source for Kubernetes
  - Core-BuildService can be used to locally build container images with apps and functionality
    - i.e. Dotnet, Python v3.9, Python v3.10, OpenCV, TensorFlow, etc. (SOO Dependent)
- VTH is only available in Development and Integration Testing.  VTH is not available in orbit

<!-- TODO: Move this content into appropriate locations elsewhere in this repo and delete this file -->