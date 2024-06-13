# `Microsoft.Azure.SpaceFx.SDK.Sensor`

The `Microsoft.Azure.SpaceFx.SDK.Sensor` namespace provides a comprehensive interface to onboard satellite sensors.

## `Sensor` Class

### Public Methods

#### `GetAvailableSensors()`

Queries the sensor host service for the list of sensors that are available to the payload application.

##### **Arguments**

- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `SensorsAvailableResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Sensor.SensorsAvailableResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorsAvailableResponse` result.

---

#### `GetAvailableSensors(MessageFormats.HostServices.Sensor.SensorsAvailableRequest sensorsAvailableRequest)`

Queries the sensor host service for the list of sensors that are available to the payload application.

##### **Arguments**

- `MessageFormats.HostServices.Sensor.SensorsAvailableRequest sensorsAvailableRequest`: A full `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorsAvailableRequest` to be sent.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `SensorsAvailableResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Sensor.SensorsAvailableResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.SensorsAvailableResponse` result.

---

#### `SensorTaskingPreCheck()`

Performs a sensor tasking pre-check request on the specified sensor.

##### **Arguments**

- `string sensorId`: A `string` containing the ID of the sensor to be pre-checked.
- `Google.Protobuf.WellKnownTypes.Any? requestData = null`: A `Google.Protobuf.WellKnownTypes.Any` object containing relevant tasking request data. This typically takes the form as a sensor-specific protobuf object. Defaults to `null` if not provided.
- `Dictionary<string, string>? metaData = null`: A dictionary containing relevant request metadata, such as `SOURCE_PAYLOAD_APP_ID`. Defaults to `null` if not provided.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `TaskingPreCheckRequest`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Sensor.TaskingPreCheckRequest>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingPreCheckRequest` result.

---

#### `SensorTaskingPreCheck(MessageFormats.HostServices.Sensor.TaskingPreCheckRequest taskingPreCheckRequest)`

Performs a sensor tasking pre-check request on the specified sensor.

##### **Arguments**

- `MessageFormats.HostServices.Sensor.TaskingPreCheckRequest taskingPreCheckRequest`: A full `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingPreCheckRequest` to be sent.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `TaskingPreCheckRequest`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Sensor.TaskingPreCheckRequest>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingPreCheckRequest` result.

---

#### `SensorTasking()`

Performs a sensor tasking request on the specified sensor.

##### **Arguments**

- `string sensorId`: A `string` containing the ID of the sensor to be tasked.
- `Google.Protobuf.WellKnownTypes.Any? requestData = null`: A `Google.Protobuf.WellKnownTypes.Any` object containing relevant tasking request data. This typically takes the form as a sensor-specific protobuf object. Defaults to `null` if not provided.
- `Dictionary<string, string>? metaData = null`: A dictionary containing relevant request metadata, such as `SOURCE_PAYLOAD_APP_ID`. Defaults to `null` if not provided.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `TaskingRequest`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Sensor.TaskingRequest>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingRequest` result.

---

#### `SensorTasking(MessageFormats.HostServices.Sensor.TaskingRequest taskingRequest)`

Performs a sensor tasking request on the specified sensor.

##### **Arguments**

- `MessageFormats.HostServices.Sensor.TaskingRequest taskingRequest`: A full `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingRequest` to be sent.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `TaskingRequest`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Sensor.TaskingRequest>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Sensor.TaskingRequest` result.
