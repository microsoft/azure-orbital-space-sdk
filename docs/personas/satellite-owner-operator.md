# Satellite Service Providers

A satellite service provider is any person or entity that integrates the Azure Orbital Space SDK runtime framework into satellites that are ultimately deployed into orbit.

Satellite service providers create plugins for Azure Orbital Space SDK's host and platform services. These plugins contain the business logic needed

Satellite service providers create data generators and develop plugins for the Azure Orbital Space SDK Virtual Test Harness (VTH) to test their plugins. As their system matures, they will continue to use the VTH verify and validate the behavior of their spacecraft through ongoing software-in-the-loop and hardware-in-the-loop testing.

Satellite service providers make the service plugins, data generators, and VTH plugins they create available to the application developers that will be running applications on their satellites. This allows payload application developers to use the same satellite virtualization as the satellite service provider to design, implement, and test their applications.

<!-- TODO: Add links to:
- host, platform, and core services
- host, platform, and core service plugins
- virtual test harness 
- virtual test harness plugins
- data generator samples -->