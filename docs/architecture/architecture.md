# Azure Orbital Space SDK Architecture

The Azure Orbital Space SDK consists of two primary components - a software development kit and a runtime framework.

## Software Development Kit

The software development kit...

## Runtime Framework

The runtime framework is a collection of microservices that run in a Kubernetes cluster. We provide the infrastructure to create and manage this cluster along with other supporting tools.

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