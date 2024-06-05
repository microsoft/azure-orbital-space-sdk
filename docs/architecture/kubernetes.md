# Kubernetes

Our Microsoft Azure Orbital Space SDK is a series of microservices running as pods in Kubernetes (k3s).  We use 4 namespaces:

- Core (core)
- Platform Services (platformsvc)
- Host Services (hostsvc)
- Payload Apps (payloadapp)

Traffic flow between the microservices is depicted below:

![kubernetes_namespace_traffic.png](/docs/img/kubernetes_namespace_traffic.png)

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
