# Azure Orbital Space SDK .NET Client Library

## Using the .NET Client Library

```xml
// Add a reference to the Microsoft Azure Orbital Space SDK Nuget Package
<PackageReference Include="Microsoft.Azure.SpaceFx.SDK" Version="0.10.0-a" />
```

```csharp
using Microsoft.Azure.SpaceFx.SDK;

namespace MySampleApp {
  public class Program {
    public static void Main(string[] args){
      // Initialize the Microsoft Azure Orbital Space SDK Client
      Client.Build();

      // Use the client to query available sensors
      var availableSensors = Microsoft.Azure.SpaceFx.SDK.Sensor.GetAvailableSensors().Result;
      foreach (var sensor in availableSensors.Sensors) {
            logger.LogInformation("...Sensor ID '{0}' available", sensor.SensorID);
      }
    }
  }
}
```