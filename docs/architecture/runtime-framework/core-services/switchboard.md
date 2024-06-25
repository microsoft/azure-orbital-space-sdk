# Azure Orbital Space SDK - Switchboard Service

The Switchboard service is the Azure Orbital Space SDK's secure MQTT message router. Switchboard is built from [rabbitMQ](https://github.com/rabbitmq/rabbitmq-server) from source with additional updates to remove rabbitMQ's dependency on docker hub. This service is designed to facilitate reliable and secure message routing within the runtime framework.

## Key Features

- **High Reliability**: Ensures message delivery even in the challenging communication conditions of space, with mechanisms to handle intermittent connections and variable latencies.
- **Security**: Implements advanced security features, including TLS/SSL for message encryption and ACLs (Access Control Lists) for fine-grained access control, ensuring that only authorized components can publish or subscribe to specific topics.
- **Scalability**: Designed to efficiently handle a large number of concurrent connections and a high throughput of messages

## Use Cases

- **Inter-Service Communication**: Enables communication between components of the Azure Orbital Space SDK runtime framework, supporting modular and scalable system designs.

<!-- TODO: Finish this documentation and provide links -->