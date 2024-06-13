# `Microsoft.Azure.SpaceFx.SDK.Logging`

The `Microsoft.Azure.SpaceFx.SDK.Logging` module provides functionality to log information and send telemetry. The logging host service has integrations with the link host service which facilitates the transfer of this data to the ground via downlinks.

## `Logging` Class

### Public Methods

#### `SendLogMessage(string logMessage)`

Sends a message to the Logging Host Service. The message is sent as a string along with an identifier for the logging level.

##### **Arguments**

- `string logMessage`: A `string` containing the message to be logged.
- `MessageFormats.Common.LogMessage.Types.LOG_LEVEL logLevel` (optional): A `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessage.Types.LOG_LEVEL` indicating the logging level. Defaults to `Info`.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `LogMessageResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.
- `bool waitForResponse` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `false`.

##### **Returns**

- `Task<MessageFormats.Common.LogMessageResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessageResponse` result.

---

#### `SendLogMessage(MessageFormats.Common.LogMessage logMessage)`

Sends a message to the Logging Host Service. The message is sent as a string along with an identifier for the logging level.

##### **Arguments**

- `MessageFormats.Common.LogMessage logMessage`: A full `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessage` to be logged.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `LogMessageResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.
- `bool waitForResponse` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `false`.

##### **Returns**

- `Task<MessageFormats.Common.LogMessageResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessageResponse` result.

---

#### ` SendTelemetry(string metricName, int metricValue)`

Sends a telemetry message to the Logging Host Service.

##### **Arguments**

- `string metricName`: A `string` containing the name of the metric being emitted.
- `metricValue`: An `int` containing the value of the metric being emitted.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `TelemetryMetricResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.
- `bool waitForResponse` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `false`.

#### **Returns**

- `Task<MessageFormats.Common.TelemetryMetricResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.Common.TelemetryMetricResponse` result.

---

#### ` SendTelemetry(MessageFormats.Common.TelemetryMetric telemetryMessage)`

Sends a telemetry message to the Logging Host Service.

##### **Arguments**

- `MessageFormats.Common.TelemetryMetric telemetryMessage`: A `MessageFormats.Common.TelemetryMetric` message to be emitted.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `TelemetryMetricResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.
- `bool waitForResponse` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `false`.

#### **Returns**

- `Task<MessageFormats.Common.TelemetryMetricResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.Common.TelemetryMetricResponse` result
