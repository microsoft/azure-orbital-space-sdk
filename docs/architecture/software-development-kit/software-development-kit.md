# Azure Orbital Space SDK Software Development Kit

The Azure Orbital Space SDK software development kit is a collection of tools, libraries, code samples, and other resources to facilitate development on the Azure Orbital Space SDK runtime framework.

## Client Library Overview

```mermaid
flowchart TB
  subgraph "Azure Orbital Space SDK Software Development Kit"
    subgraph "Client"
      Build("Build()")
      GetAppID("GetAppID()")
      GetConfigDir("GetConfigDir()")
      GetConfigSetting("GetConfigSetting")
      KeepAppOpen("KeepAppOpen()")
      ServicesOnline("ServicesOnline()")
      WaitForSidecar("WaitForSidecar()")
    end
    subgraph "Link Service"
      CrosslinkFile("CrosslinkFile()")
      DownlinkFile("DownlinkFile()")
      GetXferDirectories("GetXferDirectories()")
      SendFileToApp("SendFileToApp()")
    end
    subgraph "Logging Service"
      SendComplexLogMessage("SendComplexLogMessage()")
      SendLogMessage("SendLogMessage()")
      SendTelemetry("SendTelemetry()")
    end
    subgraph "Position Service"
      RequestPosition("RequestPosition()")
    end
    subgraph "Sensor Service"
      GetAvailableSensors("GetAvailableSensors()")
      GetXferDirectories("GetXferDirectories()")
      SensorTasking("SensorTasking()")
      SensorTaskingPreCheck("SensorTaskingPreCheck()")
    end
  end
```

[Azure Orbital Space SDK Software Development Kit](./software-development-kit/software-development-kit.md)