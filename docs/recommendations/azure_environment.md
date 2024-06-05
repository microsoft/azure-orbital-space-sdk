# Azure Orbital Space SDK - Azure Environment Recommendations:
The following are the recommendations for building a secure development environment for developing and testing applications designed to run in Space:

## Azure Environment:
Currently the general environment we recommend that a customer would have to meet security requirements.  So this is in line with best practices would include:
- 3x Azure Virtual Networks:
    - 1 - Hub virtual network
    - Minimum 1 - Virtual Network for Developer Machines (could be more if you have geo-disperse developers).
    - Minimum 1 - Virtual Network for Virtual Satellites / Virtual Ground Stations (could be more if you have geo-disperse developers or need for multiple environments).
- VPN Gateway - For configuring a point-to-site vpn - Connect to a VNet using P2S VPN & multiple authentication types: portal - Azure VPN Gateway | Microsoft Learn
- Log Analytics Workspace for Azure Monitor - Recommended for capturing security logs on your dev machines, and all logs from virtual satellites and ground stations.  Log retention to meet your needs.  
- Azure Dev Ops - For your code development and project management, should be scaled to meet your needs.
- Azure Container Registry - Generally recommend a standard tier for production systems
- Azure Key Vault - ​Needed for maintaining the constellation key, and other secrets required for development activities and virtual satellites / ground stations.
- Storage Account - LRS Storage - To support any data storage needs of sample data, etc.  

## Developer Machines:
For each developer machine required, we recommend the following:
- Virtual Machine OS:  Linux, Ubuntu 22.04 Standard
- VM Size:  Standard_E8s_v4
- Data Disk(s):  S20 with 512 GB of Storage:  This is recommended for development work, as it is considered an anti-pattern to do your code work on the OS disk.
You would then add 1 of the above for each developer.

## Virtual Satellite / Ground Stations:
For each virtual satellite or virtual ground station you add, you would need to add the following:
- Virtual Machine OS:  Linux, Ubuntu 22.04 Standard
- VM Size:  Standard_E8s_v4
- Data Disk(s):  S20 with 512 GB of Storage:  This is recommended for development work, as it is considered an anti-pattern to do your code work on the OS disk.