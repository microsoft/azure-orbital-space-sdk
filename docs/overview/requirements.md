# Azure Orbital Space SDK System Requirements

## Production

### Minimum Requirements for SDK deployment

- 1.2Ghz, Dual core (x64 / ARM64)
- 1Gb RAM
- 3Gb Hard Drive (Initial) / 1.5Gb Hard Drive (Running)
  - (if applicable) GPU Accelerated: +1Gb
- CIFS Kernel module available
- (if applicable) GPU Accelerated: Driver deployed (`nvidia-smi --list-gpus` shows GPU)

### Recommended Requirements for SDK Deployment

- 2Ghz+, Quad core (x64 / ARM64)
- 2GB+ RAM
- 3Gb Hard Drive (Initial) / 1.5Gb Hard Drive (Running)
  - (if applicable) GPU Accelerated: +1Gb
- CIFS Kernel module available
- (if applicable) GPU Accelerated: Driver deployed (`nvidia-smi --list-gpus` shows GPU)

## Long Term CPU and Memory Usage (30+ Days)

Below is the CPU and Memory statistics of a Virtual Machine running for ~30 days using Azure Orbital Space SDK 0.7.0.

- Machine Type: x64, Ubuntu 22.04, 4 vcpus @ 3.5Ghz, 32GB RAM.
- Measurements taken with [Azure Monitor](https://learn.microsoft.com/en-us/azure/aks/monitor-aks) and analyzed in [Log Analytics](https://learn.microsoft.com/en-us/azure/azure-monitor/logs/log-analytics-overview) with a 2 min average

Service | Namespace | CPU (cores) | Memory (bytes)
------ | ------ | ------ | ------
HostSvc-Link   | hostsvc | 8m | 85Mb
HostSvc-Logging   | hostsvc | 8m | 90Mb
HostSvc-Position   | hostsvc | 8m | 85Mb
HostSvc-Sensor   | hostsvc | 8m | 87Mb
FileServer   | core | 40m | 8Mb
Switchboard   | core | 15m | 85Mi
Platform-Deployment | platformsvc | 8m | 85Mi
Platform-MTS   | platformsvc | 8m | 85Mi
<br />**Total:**   |  | <br /> **~105m <br />(~1/10th of 1 core)** | <br /> ~**600Mb**

CPU and Memory Metrics for a 30 day run (x64, 4 cores, 2GB Ram)

- CPU
  ![CPU Allocation](/docs/img/sdk-cpu-usage.png)

- RAM
  ![RAM Allocation](/docs/img/sdk-ram-usage.png)

## Development Requirements

- 1.5Ghz+, Quad core+ (x64 / ARM64)
- 4Gb+ RAM
- 10Gb Hard Drive
- CIFS Kernel module available
- (if applicable) GPU Accelerated: Driver deployed (`nvidia-smi --list-gpus` shows GPU)
- Docker v24.0.7+
- DevContainer Extension v0.338.1+
- Ubuntu 20.04+
