# Azure Environment Recommendations

We recommend to the following setup to ensure a secure Azure Orbital Space SDK development environment within Azure.

## Azure Environment

| Item                                                     | Recommended Service                                                                                          | Number | Description |
| :------------------------------------------------------- | :----------------------------------------------------------------------------------------------------------: | -----: | :---------- |
| Virtual Network (Hub)                                    | [Azure Virtual Network](https://learn.microsoft.com/en-us/azure/virtual-network/)                            | 1      | Provides network connectivity to all resources. |
| Virtual Network (Developer VMs)                          | [Azure Virtual Network](https://learn.microsoft.com/en-us/azure/virtual-network/)                            | 1-Many | Isolated virtual network for developer virtual machines. You may need additional networks if you have a geographically disperse development team. |
| Virtual Network (Virtual Satellites and Ground Stations) | [Azure Virtual Network](https://learn.microsoft.com/en-us/azure/virtual-network/)                            | 1-Many | Isolated virtual network for virtual satellites and ground stations. You may need additional networks if you have a geographically disperese development team, or if you need to simulate multiple environments. |
| VPN Gateway                                              | [Azure VPN Gateway](https://learn.microsoft.com/en-us/azure/vpn-gateway/vpn-gateway-about-vpngateways)       | 1      | A single VPN gateway used to access all resources through the hub virtual network. |
| Log Analytics Workspace                                  | [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-workspace-overview) | 1      | Recommended for use with [Azure Monitor](https://azure.microsoft.com/en-us/products/monitor) for capturing performance, logs, and security information across all virtual machines. Set the log retention of your workspace conservatively to meet your needs and budget. |
| Container Registry                                       | [Azure Contianer Registry](https://azure.microsoft.com/en-us/products/container-registry)                    | 1      | A single container registry to contain your container images and artifacts. We recommend the standard storage tier for production systems. |
| Key Vault                                                | [Azure Key Vault](https://azure.microsoft.com/en-us/products/key-vault)                                      | 1      | Used to securely store, manage, and access secret required for developmental activities, virtual satellites, and virtual ground stations. |
| Storage Account                                          | [Azure Blob Storage](https://azure.microsoft.com/en-us/products/storage/blobs)                               | 1      | Used to support any additional data storage needs, such as sample data. We recommend [locally redundant storage (LRS)](https://learn.microsoft.com/en-us/azure/storage/common/storage-redundancy#locally-redundant-storage). |

## Developer Machines

We recommend the following configuration for Azure Orbital Space SDK developer virtual machines:

| Item                    | Value                                                                                                                                                |
| :---------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------: |
| Operating System        | [Ubuntu Server 22.04 LTS (Jammy)](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/canonical.0001-com-ubuntu-server-jammy?tab=overview) |
| Minimum VM Size         | [Standard_E8s_v4](https://learn.microsoft.com/en-us/azure/virtual-machines/ev4-esv4-series#ev4-series)                                               |
| Minimum Data Disk(s)    | [Standard HDD 512GB (S20)](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types#standard-hdd-size)                                   |
| Maximum Number of Users | 1                                                                                                                                                    |

Each developer should have their own virtual machine with the above configuration.

We don't recommend development to occur on a virtual machine's OS disk. OS disks are intended to be deleted alongside their parent virtual machine, and any data on those disks will be lost upon deletion. Data disks, however, can easily be removed and attached to other virtual machines. This allows you to create new virtual machines as needed without losing your development data.

## Virtual Satellites and Ground Stations

We recommend the folliwing configuration for Azure Orbital Space SDK virtual satellites and ground stations:

| Item                    | Value                                                                                                                                                |
| :---------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------: |
| Operating System        | [Ubuntu Server 22.04 LTS (Jammy)](https://azuremarketplace.microsoft.com/en-us/marketplace/apps/canonical.0001-com-ubuntu-server-jammy?tab=overview) |
| Minimum VM Size         | [Standard_E8s_v4](https://learn.microsoft.com/en-us/azure/virtual-machines/ev4-esv4-series#ev4-series)                                               |
| Minimum Data Disk(s)    | [Standard HDD 512GB (S20)](https://learn.microsoft.com/en-us/azure/virtual-machines/disks-types#standard-hdd-size)                                   |

Like developer machines, we don't recommend using a virtual machine's OS drive to store data. Instead, you should use a separate data disk that can safely be removed and attached to your virtual machines as needed.