# `spacefx.link`

The `spacefx.link` module provides functionalities for managing file transfers between your application host services, and other applications running within the Azure Orbital Space SDK runtime framework.

## Methods

### `crosslink_file()`

Crosslinks a file to the destination service's inbox directory.

#### **Arguments**

- `destination_app_id`: A `str` containing the ID of the destination application.
- `filepath`: A `str` containing the local path of the file to be transferred.
- `overwrite_destination_file` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `False`.
- `response_timeout_seconds` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message.

#### **Returns**

- A successful `LinkResponse` message, or the last `LinkResponse` message heard during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `LinkResponse` message is not heard during the timeout period.

---

### `downlink_file()`

Sends a file to Message Translation Service (MTS) to downlink to the ground at the next available opportunity.

#### **Arguments**

- `destination_app_id`: A `str` containing the ID of the destination application.
- `filepath`: A `str` containing the local path of the file to be transferred.
- `overwrite_destination_file` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `False`.
- `response_timeout_seconds` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message.

#### **Returns**

- A successful `LinkResponse` message, or the last `LinkResponse` message heard during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `LinkResponse` message is not heard during the timeout period.

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

### `send_file_to_app()`

Sends a file to the destination's inbox directory.

#### **Arguments**

- `destination_app_id`: A `str` containing the ID of the destination application.
- `filepath`: A `str` containing the local path of the file to be transferred.
- `overwrite_destination_file` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `False`.
- `response_timeout_seconds` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message.

#### **Returns**

- A successful `LinkResponse` message, or the last `LinkResponse` message heard during the timeout period.

#### **Raises**

- `TimeoutException`: Returns a .NET `System.TimeoutException` if a `LinkResponse` message is not heard during the timeout period.

---
