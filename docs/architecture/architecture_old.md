# What does the Azure Orbital Space SDK architecture look like for communication

Now that we've resolved many of the core challenges, what does it look like for our applications to communicate from the payload application, the whole way down to the data generator and back.

The flow is the following (you can also run the demo described here using our walkthrough for [dotnet](https://github.com/microsoft/Azure-Orbital-Space-SDK-QuickStarts/blob/main/tutorials/quick-start-tutorials/e2e-eo-sample-dotnet.md) or [python](https://github.com/microsoft/Azure-Orbital-Space-SDK-QuickStarts/blob/main/tutorials/quick-start-tutorials/e2e-eo-sample-python.md):

```mermaid
sequenceDiagram
    Sample App->>hostsvc-sensor: Make a Sensor Tasking Request
    hostsvc-sensor->>platform-mts: Make a sensor request
    platform-mts->>MtsPluginVth: The call is routed to the MTS plugin
    MtsPluginVth->>vth: Sensor request is forwarded
    vth->>PlanetaryComputerGeotiffPluginVth: Sensor request is forwarded
    PlanetaryComputerGeotiffPluginVth->>PlanetaryComputerGeotiffPluginVth: converted SensorRequest to a EarthImageRequest
    PlanetaryComputerGeotiffPluginVth->>tool-planetary-computer-geotiff: EarthImageRequest is forwarded
    tool-planetary-computer-geotiff->>tool-planetary-computer-geotiff: store output image /var/spacedev/tmp/vth/output shared volume
    tool-planetary-computer-geotiff->>PlanetaryComputerGeotiffPluginVth: Send EarthImageResponse with filename of output
    PlanetaryComputerGeotiffPluginVth->>vth: send Sensor Response / Data
    vth->>MtsPluginVth: Send Sensor Response / Data
    MtsPluginVth->>platform-mts: Send Sensor Response / Data
    platform-mts->>hostsvc-sensor: Send Sensor Response / Data
    hostsvc-sensor->>Sample App: Send back sensor response / data
```

<!-- TODO: Move this content into appropriate locations elsewhere in this repo and delete this file -->