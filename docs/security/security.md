# Azure Orbital Space SDK Security Overview:
A key area of focus for this team has always been ensuring that our platform is secure, and can provide both Satellite Owner Operators, and Payload Application Developers with confidence that our platform is providing the appropriate level of security to the spacecraft.  

# Approach to Trust Levels and Pods:
Another element that is a critical part of our security strategy, a focus on Trust Levels for the different elements of the cluster to ensure security is implemented in a consistant manner.  With any kubernetes cluster based system, it is important to designate the trust levels for the pods that are executed on the cluster.  To support this, the Azure Orbital Space SDK separates our pods into the following trust levels.  

- **Privileged:** These pods have privileged access and are secured separately due to the additional permissions they require to function.
- **Trusted:** These pods/container images that are internal to the Azure Orbital Space SDK, and have security features implemented by our process to ensure the validity and security of the code and the platform.
- **Untrusted:**  These pods are not internal to the Azure Orbital Space SDK, and it is ultimatley up to the the Satellite Owner Operator to ensure the security of these container images, and these pods have greater restrictions placed upon them.  

From a perspective of trust level, we recognize the classification of services based on the following matrix:

| Service Type | Description | Trust Level |
|--------------|-------------|-------------|
| [Core Services](./docs/architecture/runtime-framework/core-services/core-services.md) | Include outside components that support the operation of the cluster, and the pods within the cluster. | Privileged |
| [Platform Services](./docs/architecture/runtime-framework/platform-services/platform-services.md) | Platform services are a classification of service that is responsible for interacting with the underlying spacecraft.  This includes our [Message Translation Service](./docs/architecture/runtime-framework/platform-services/message-translation-service.md) and [Deployment Service](./docs/architecture/runtime-framework/platform-services/deployment.md). | Privileged |
| [Host Services](./docs/architecture/runtime-framework/host-services/host-services.md) | These services support the communication between the payload application and the underlying spacecraft. | Trusted |
| [Payload Applications](./docs/developer-experience/developer-experience.md) | The application created with the intention of being run on the spacecraft. | Untrusted |

# Key Security Features implemented today:
Below are outlined key features to enhance the security of the platform, and the explanations on the importance.  

## Platform Level Security Features:
For Security, the first focus of the Azure Orbital Space SDK was to enforce key security features and considerations as a part of the cluster from the moment it is deployed.  Below you will find an outlining of several of the key security features that have been built into our platform.

### Restricted Container Images:
For our host and platform service images, we leverage a [build service] solution which enables us to build our service images on-orbit.  And as part of this, we have a focus on keeping our services-lean and minimize the required storage.  As part of this, we extended this goal to harden our container images by restricting our container images as far as possible.  For more details on our container image build service, you can find details [here](./todo_build_service.md).  

### Isolation of Platform Deployment:
From a security threat model, our [Platform-deployment](./docs/architecture/runtime-framework/platform-services/deployment.md) is the only service that has elevated permissions for access to the Kubernetes cluster.  Due to this, fact we have isolated platform deployment to minimize the attack surface.  All workflows associated with platform-deploy.  

### Isolation of Host Machine:
For the Azure Orbital Space SDK, there is only one service, [Platform-mts](./docs/architecture/runtime-framework/platform-services/message-translation-service.md), that has the ability to access the host machine.  This is done to access, but the Message Translation Service requires this access so that it can access the underlying spacecraft's API or endpoints.  

### Multi-Node Hypervisor Support:
Another solution for security comes in terms on an implementation option, the Azure Orbital Space SDK supports the ability to handle multi-node clusters, and we have validated this using virtual machines on the linux operating system.  This provides the capability that should a Sattellite Owner Operator chose to enable multiple virtual machines as nodes on their cluster, you can use labels and kubernetes own node affinity and anti-affinity policies to support enforcement of hypervisor level security by isolating both payload apps away from core, host and platform services, and/or isolation of customer workloads from each other.  

### Use of Core-Registry:
Additionally, there is another feature whereby we provide a configuration option by which the cluster can be deployed with [Core-Registry](./docs/architecture/runtime-framework/core-services/registry.md).  This pod functions as the container registry for our kubernetes cluster.  Satellite Owner Operators can choose to deploy with or without Core-Registry (the alternative being a docker registry behind kubernetes), but the intention here being that container registry makes it easier to stage / deploy the registry on the spacecraft host machine.  

Being that our cluster manages and secures its own registry to isolate from other registries on the spacecraft.  This helps secure our registry from being access from outside the cluster, and additionally prevents a compromised payload app from accessing the spacecraft's registry.  

### Use of core-fileserver and hostsvc-link:
For our transfer of files between not just payload app to payload app, but also sending files between the payload apps and the host / platform services, the Azure Orbital Space SDK leverages the [Link Service](./docs/architecture/runtime-framework/host-services/link.md), and it is given access to the [FileServer](./docs/architecture/runtime-framework/core-services/fileserver.md) to be able to move files only between the inbox / outbox for the different pods.  This improves our security model by ensuring that traffic and access between pods are limited.  Additionally, the use of [FileServer](./docs/architecture/runtime-framework/core-services/fileserver.md) ensure that we are not accessing the disk.  Satellite Owner Operators can choose to not leverage [FileServer](./docs/architecture/runtime-framework/core-services/fileserver.md), in that scenario, the [Link Service](./docs/architecture/runtime-framework/host-services/link.md), would need to be given access to the host machine, or the location of the xfer directory, but it would be the only service, which is isolated from Untrusted code.  

### Code Scanning:
Azure Orbital Space SDK has implemented CodeQL scanning as part of the normal workflow for the development and deployment of the artifacts that support the platform.  To accomplish this, our repos contain workflows that support CodeQL, and we execute our scans on before any PR can be approved.  The intention behind this being to validate the quality of the code before it can be committed to main.  Additionally the Azure Orbital Space SDK, executes these scans on a nightly basis to ensure that we catch any new queries that are added to the query packs.  As of right now we are leveraging the githubsecuritylab.  

## Payload App Security Features:
The following are the security features around pod communication.  

### Network Isolation of Payload Applications:
TODO:  Explain the policies that restrict Payload App Access on the cluster for core, host and platform services.
Aligning with the trust level outlined above, one of the key features that has been implemented is the network policies to isolate communication between the different trust levels of pods to support 

### Topic Restriction of Payload Applications:
TODO:  Explain the policies that restrict the payload app from publishing to, or listening on topics it shouldn't

### Message Validation by Host Services:
TODO: Talk about how our host services throw away messages that don't conform to expectation.

# Key Security Features on our roadmap.
The following are key security features that are on the roadmap to further support the Azure Orbital Space SDK platform:

## Container Signing:
TODO: Talk about plans for container signing and validation.  

## Hypervisor Isolation at the Pod Level:
TODO: Build out isolation at the pod level using Katana.

## Increase Robustness of Certificate Management:
TODO: Moving away from self-signed certificates to integrate with a certificate authority between orbit and ground. 

## Hash Validation for Plugins: 
TODO: Building out ability to validate hash of plugins to ensure supply chain.