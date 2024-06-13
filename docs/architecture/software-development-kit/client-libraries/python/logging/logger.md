# `spacefx.logger`

The `spacefx.logger` module publicly exposes the `spacefx.logging.__SpaceFxLogger` class. This class extends `logging.getLoggerClass()` and serves as a drop-in replacement for Python's `logger` module. All logging performed by `spacefx.logger` will perform both native Python logging and logging through the logging host service.

## Methods

### `debug()`

Performs Python `DEBUG` logging and sends a message to the logging host service at the `Debug` log level.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `*args` (optional): Additional arguments for Python's native logger.
- `**kwargs` (optional): Additional keyword arguments for Python's native logger.

#### **Returns**

- None

---

### `info()`

Performs Python `INFO` logging and sends a message to the logging host service at the `Info` log level.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `*args` (optional): Additional arguments for Python's native logger.
- `**kwargs` (optional): Additional keyword arguments for Python's native logger.

#### **Returns**

- None

---

### `warning()`

Performs Python `WARN` logging and sends a message to the logging host service at the `Warning` log level.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `*args` (optional): Additional arguments for Python's native logger.
- `**kwargs` (optional): Additional keyword arguments for Python's native logger.

#### **Returns**

- None

---

### `error()`

Performs Python `ERROR` logging and sends a message to the logging host service at the `Error` log level.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `*args` (optional): Additional arguments for Python's native logger.
- `**kwargs` (optional): Additional keyword arguments for Python's native logger.

#### **Returns**

- None

---

### `critical()`

Performs Python `CRITICAL` logging and sends a message to the logging host service at the `Critical` log level.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `*args` (optional): Additional arguments for Python's native logger.
- `**kwargs` (optional): Additional keyword arguments for Python's native logger.

#### **Returns**

- None

---

### `send_log_message()`

Sends a log message to the logging host service at the specified log level. This method does not perform Python native logging.

#### **Arguments**

- `message`: A `str` containing the message to be logged.
- `level`: A `Microsoft.Azure.SpaceFx.MessageFormats.Common.LogMessage.Types.LOG_LEVEL` indicating the logging level

#### **Returns**

- None
