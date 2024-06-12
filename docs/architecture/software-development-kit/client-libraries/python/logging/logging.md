# `spacefx.logging`

The `spacefx.logging` module provides functionality to log information and send telemetry. The logging host service has integrations with the link host service which facilitates the transfer of this data to the ground via downlinks.

## Methods

### `send_complex_log_message()`

Sends a message to the Logging Host Service. Allows the user to specify the full `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessage` object to be logged.

#### **Arguments**

- `message`: A `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessage` to be logged.
- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a successful `LogMessageResponse`. Defaults to `30` seconds.
- `wait_for_response` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `False`.

#### **Returns**

- A successful `LogMessageResponse` message, or the last `LogMessageResponse` message heard during the timeout period.

#### **Raises**

- `TimeoutError`: Returns a .NET `System.TimeoutException` if a `LogMessageResponse` message is not heard during the timeout period.

---

### `send_log_message()`

Sends a message to the Logging Host Service. The message is sent as a string along with an identifier for the logging level.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `log_level` (optional): A `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessage.Types.LOG_LEVEL` indicating the logging level. Defaults to `Trace`.
- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a successful `LogMessageResponse`. Defaults to `30` seconds.
- `wait_for_response` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `False`.

#### **Returns**

- A successful `LogMessageResponse` message, or the last `LogMessageResponse` message heard during the timeout period.

#### **Raises**

- `TimeoutError`: Returns a .NET `System.TimeoutException` if a `LogMessageResponse` message is not heard during the timeout period.

---

### `send_telemetry()`

Sends a telemetry message to the Logging Host Service.

#### **Arguments**

- `metric_name`: A `str` containing the name of the metric being emitted.
- `metric_value`: A `int` containing the value of the metric being emitted.
- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a successful `TelemetryMetricResponse`. Defaults to `30` seconds.
- `wait_for_response` (optional): A `bool` indicating if the calling thread should wait for a response. Defaults to `False`.

#### **Returns**

- A successful `TelemetryMetricResponse` message, or the last `TelemetryMetricResponse` message heard during the timeout period.

#### **Raises**

- `TimeoutError`: Returns a .NET `System.TimeoutException` if a `TelemetryMetricResponse` message is not heard during the timeout period.

---

