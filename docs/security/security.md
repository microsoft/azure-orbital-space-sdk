# Azure Orbital Space SDK Security Overview:
A key area of focus for this team has always been ensuring that our platform is secure, and can provide both Satellite Owner Operators, and Payload Application Developers with confidence that our platform is providing the appropriate level of security to the spacecraft.  

## Trust Levels and Pods:
With any kubernetes cluster based system, it is important to designate the trust levels for the pods that are executed on the cluster.  To support this, the Azure Orbital Space SDK separates our pods into the following trust levels.

- Trusted: These pods/container images that are internal to the Azure Orbital Space SDK, and have security features implemented by our process to ensure the validity and security of the code and the platform.
- Untrusted:  These pods are not internal to the Azure Orbital Space SDK, and it is ultimatley up to the the Satellite Owner Operator to ensure the security of these container images, and these pods have greater restrictions placed upon them.  

From a perspective of trust level, we recognize the classification of services based on the following matrix:

| Service Type | Description | Trust Level |
|--------------|-------------|-------------|
| [Core Services](./docs/architecture/runtime-framework/core-services/core-services.md) | Include outside components that support the operation of the cluster, and the pods within the cluster. | Trusted |
| [Platform Services](./docs/architecture/runtime-framework/platform-services/platform-services.md) | Platform services are a classification of service that is responsible for interacting with the underlying spacecraft.  This includes our [Message Translation Service](./docs/architecture/runtime-framework/platform-services/message-translation-service.md) and [Deployment Service](./docs/architecture/runtime-framework/platform-services/deployment.md). | Trusted |
| [Host Services](./docs/architecture/runtime-framework/host-services/host-services.md) | These services support the communication between the payload application and the underlying spacecraft. | Trusted |
| [Payload Applications](./docs/developer-experience/developer-experience.md) | The application created with the intention of being run on the spacecraft. | Untrusted |



## Key Security Features implemented today:
Below are outlined key features to enhance the security of the platform:

### Network Isolation of Payload Applications:
The Azure Orbital Space SDK leverages n
### Topic Restriction of Payload Applications:

### Restricted Container Images:

### Isolation of Platform Deployment:

### Multi-Node Hypervisor Support:

### Use of Core-Registry:

### Use of core-fileserver and hostsvc-link:


### Security Scanning:
Azure Orbital Space SDK has implemented CodeQL scanning as part of the normal workflow for the development and deployment of the artifacts that support the platform.  To accomplish this, our repos contain workflows that support the 

## Key Security Features on our roadmap.
The following are key security features that are on the roadmap to further support the Azure Orbital Space SDK platform:

### Container Signing:

### Increase Robustness of Certificate Management:

### Hash Validation for Plugins: 