# `spacefx.sensor`

The `spacefx.sensor` module 

## Public Methods

### `get_available_sensors()`

Queries the sensor host service for the list of sensors that are available to the payload application.

#### **Arguments**

- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a successful `SensorsAvailableResponse`. Defaults to `30` seconds.

#### **Returns**

- Returns a successful `SensorsAvailableResponse`, or the last heard `SensorsAvailableResponse` during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `SensorsAvailableResponse` message is not heard during the timeout period.

---

### `get_xfer_directories()`

Returns the inbox, outbox, and root transfer volume for the application.

#### **Arguments**

- None

#### **Returns**

- A `dict` of the form:
    ```python
    {
        'inbox':  str <path to inbox>,
        'outbox': str <path to inbox>,
        'root':   str <path to inbox>,
    }
    ```

---

### `sensor_tasking()`

Performs a sensor tasking request on the specified sensor.

#### **Arguments**

- `sensor_id`: A `str` containing the ID of the sensor to be tasked.
- `request_data` (optional): A `google.protobuf.any_pb2.Any` object containing relevant tasking request data. This typically takes the form as a sensor-specific protobuf object. Defaults to `None` if not provided.
- `metadata` (optional): A `Dict[str, str]` containing relevant request metadata, such as `SOURCE_PAYLOAD_APP_ID`. Defaults to `None` if not provided.
- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a successful `TaskingResponse`. Defaults to `30` seconds.

#### **Returns**

- Returns a successful `TaskingResponse`, or the last heard `TaskingResponse` during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `TaskingResponse` message is not heard during the timeout period.

#### **Notes**

In addition to the `TaskingResponse`, the sensor host service will asynchronously send a `SensorData` message containing the output of the sensor tasking. Use [`subscribe_to_sensor_data()`](#subscribe_to_sensor_data) to register a callback method to process these messages.

---

### `sensor_tasking_pre_check()`

Performs a sensor tasking pre-check request on the specified sensor. This helps establish if a sensor is ready for processing before submitting a full tasking request.

#### **Arguments**

- `sensor_id`: A `str` containing the ID of the sensor to be pre-checked.
- `request_data` (optional): A `google.protobuf.any_pb2.Any` object containing relevant pre-check request data. This typically takes the form as a sensor-specific protobuf object. Defaults to `None` if not provided.
- `metadata` (optional): A `Dict[str, str]` containing relevant request metadata, such as `SOURCE_PAYLOAD_APP_ID`. Defaults to `None` if not provided.
- `response_timeout_seconds` (optional): An `int` specifying the number of seconds to wait for a successful `TaskingPreCheckResponse`. Defaults to `30` seconds.

#### **Returns**

- Returns a successful `TaskingPreCheckResponse`, or the last heard `TaskingPreCheckResponse` during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `TaskingPreCheckResponse` message is not heard during the timeout period.

---

### `subscribe_to_sensor_data()`

Subscribes a callback method to the `SensorData` feed. All subscribed methods will be called anytime a `SensorData` message is received.

#### **Arguments**

- `callback_function`: A `Callable[[typing.TypeVar('T'), None]]` callback method. This method should take a `SensorData` object as its only argument.

#### **Returns**

- None

#### **Notes**

All callback methods registered via `subscribe_to_sensor_data` will be executed automatically anytime the payload application receives a `SensorData` message. Callback methods can not be unsubscribed. We recommend that only one callback method is registered with this method in general.