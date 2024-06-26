# Azure Orbital Space SDK - Sensor Service

The Sensor Service provides access to the wide array of sensors onboard on the spacecraft. This service enables the efficient collection, processing, and distribution of sensor data. Thanks to its highly abstracted and generalized design, the sensor service can interface with any type of onboard sensor, whether its an imaging sensor, an altimeter, an accelerometer, or beyond.

## Key Features

- **Data Collection and Management**: Automates the collection and management of data from multiple sensor types, ensuring efficient handling of large volumes of sensor data.
- **Flexible Data Access**: Enables versatile interaction modes with sensor data, allowing users to submit jobs for asynchronous data retrieval, perform immediate queries for direct responses, or subscribe to sensor data feeds for continuous, passive updates.
- **Data Provision for Further Onboard Processing**: Provides sensor data in standardized formats that facilitates additional processing and analysis, enabling payload applications to apply their own tools to achieve their mission.

## Use Cases

- **Image Capture**: Utilize imaging sensors to capture high-resolution imagery of Earth or celestial bodies, supporting applications in mapping, surveillance, and scientific observation.
- **Data Streaming**: Facilitates continuous data streaming from sensors like accelerometers, magnetometers, and spectrometers, enabling real-time monitoring of satellite health, environmental conditions, and scientific phenomena.
- **Event-Triggered Sampling**: Employs sensors to perform event-triggered sampling, such as capturing atmospheric data when specific conditions are met, enhancing the efficiency of data collection for climate research and disaster management.
- **Remote Sensing Analysis**: Supports remote sensing analysis by providing multispectral and hyperspectral imaging data, aiding in agriculture, forestry, and land use planning.

## Getting Started

### Deployment

### Configuration

## Sensor Data Routing

The sensor service orchestrates the flow of data from sensors to payload applications, leveraging the Message Translation Service (MTS) for seamless data translation and routing between the runtime framework and the satellite payload. Sensor data can be routed in one of two ways, direct and broadcast routing.

### Direct Sensor Data Routing

For specific tasking requests, such as capturing an image or conducting secondary processing, the sensor service ensures that data is directly routed to the requesting application. This is facilitated by the presence of `DestinationAppId` or `TaskingTrackingId` in the tasking request.

- **TaskingTrackingId**: When populated, the sensor service prioritizes this for routing, removing any `DestinationAppId` if present. The sensor service verifies the request against its local cache for a matching `TaskingResponse` with a successful status. If a match is found, sensor data is routed to the originating application. Sensor data is discarded of the originating application is no longer present in the runtime framework.

- **DestinationAppId**: If `TaskingTrackingId` is not present, sensor data is routed directly to this application ID. Sensor data is discarded of the application is not present in the runtime framework. This method is less secure than using `TaskingTrackingId` and is recommended only when the specific application ID is both known and static.

> **Note:** if both the DestinationAppId and TaskingTrackingId are populated, then `DestinationAppId` is removed to force the more stringent routing rules of `TaskingTrackingId` to take affect.  This is done as a security measure to prevent stale applications from accessing sensor data they are not permitted to interact with.

### Broadcast Sensor Data Routing

When neither `DestinationAppId` nor `TaskingTrackingId` is populated, the sensor service broadcasts the data to all applications that have subscribed to the data feed. The sensor service checks its cache for any matching `TaskingResponse` with a successful status, and broadcasts the sensor data to the corresponding applications. Sensor data is not forwarded to applications that are no longer present in the runtime framework.

> **Note:** `TaskingRequests` expire after 24 hours, or when another corresponding `TaskingResponse` is received with a status other than `Successful`. Long-running payload applications must create a new `TaskingRequest` every 24 hours to continue receiving sensor data. This is done to prevent data overflow and ensure that only active and relevant payload applications receive sensor data.

### Sensor Data Routing Flow Diagram

The routing logic is depicted in the diagram below, illustrating the decision-making process for directing sensor data to payload applications.

```mermaid
flowchart TD
    A[MTS] -->|Send SensorData| B(Sensor Service)
    B --> C{Direct or <br /> Broadcast?}
    C -->|TaskingRequestID or <br /> DestinationAppID populated| D[Direct Data]
    D --> F{TaskingRequestID <br /> populated}
    F -->|TaskingResponse with Matching <br /> Tracking ID and Successful Status| T{Is the App online?}
    D --> H{DestinationAppId <br /> populated}
    H --> T
    C -->|TaskingRequestID and <br /> DestinationAppID empty| E[Broadcast Data]
    E -->|TaskingResponse with Matching <br /> Sensor ID and Successful Status| T{Is the App online?}
    T -->|Yes| W[Send Sensor Data]
    T -->|No| X[Discard Message]
    W -->|SensorData| R[Payload Application]
```

### Practical Examples

#### Direct Sensor Data

A TaskingTrackingId is used to request a specific action, such as capturing imagery. The sensor service ensures the data is routed directly to the requesting application, provided it is online.

```csharp
SensorData sensorData = new() {
    TaskingTrackingId = 123-456-789,
    SensorID = "RGBCamera",
    Data = Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue() { Value = "PictureMetaData" })
};
```

#### Broadcast Sensor Data

Sensor data without a specific destination is broadcasted to all online applications, ensuring widespread data dissemination for general monitoring or analysis.

```csharp
SensorData sensorData = new() {
    SensorID = "TemperatureSensor",
    Data = Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue() { Value = "14" })
};
```

<!-- TODO: Finish this documentation -->