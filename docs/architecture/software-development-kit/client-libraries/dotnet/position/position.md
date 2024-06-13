# `Microsoft.Azure.SpaceFx.SDK.Position`

The `Microsoft.Azure.SpaceFx.SDK.Position` module provides functionality to log information and send telemetry. The logging host service has integrations with the link host service which facilitates the transfer of this data to the ground via downlinks.

## `Position` Class

### Public Methods

#### `LastKnownPosition()`

Requests the last observed position of the satellite.

##### **Arguments**

- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `PositionResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Position.PositionResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Position.PositionResponse` result.

---

#### `LastKnownPosition(MessageFormats.HostServices.Position.PositionRequest positionRequest)`

Requests the last observed position of the satellite.

##### **Arguments**

- `MessageFormats.HostServices.Position.PositionRequest positionRequest`: A full `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Position.PositionRequest` to be sent.
- `int? responseTimeoutSecs` (optional): An `int` specifying the number of seconds to wait for a successful `PositionResponse`. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Position.PositionResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Position.PositionResponse` result.