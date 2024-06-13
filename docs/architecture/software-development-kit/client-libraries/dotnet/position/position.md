# `spacefx.position`

The `spacefx.position` module provides functionality for payload applications to determine the current location of a satellite. This helps facilitate applications running position-sensitive operations, such as oceanic observation.

## Public Methods

### `request_position()`

Requests the last observed position of the satellite.

#### **Arguments**

- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a `SUCCESSFUL` or `NOT_FOUND` `PositionResponse`. Defaults to `30` seconds.

#### **Returns**

- Returns a `SUCCESSFUL` or `NOT_FOUND` `PositionResponse`, or the last heard `PositionResponse` during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `PositionResponse` message is not heard during the timeout period.
