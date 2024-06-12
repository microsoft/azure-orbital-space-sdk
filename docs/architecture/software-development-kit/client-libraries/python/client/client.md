# `spacefx.client` Module

The `spacefx.client` module provides a client that initializes your application's connection to the Azure Orbital Space SDK runtime framework.

## Methods

### `build()`

Initializes the GRPC channel to the Dapr sidecar, serving as the entry point to the Microsoft Azure SpaceFx.

#### **Arguments**

- None

#### **Returns**

- An initialized instance of `Microsoft.Azure.SpaceFx.SDK.Client`. This is the .NET client library's `Client` object which `spacefx.client` wraps via PythonNET.

---

### `get_app_id()`

Retrieves the application ID associated with your application.

#### **Arguments**

- None

#### **Returns**

- A `str` containing the registered application ID.

---

### `get_config_dir()`

Retrieves the path to the application's configuration directory.

#### **Arguments**

- None

#### **Returns**

- A `str` containing the path to the application's configuration directory.

---

### `get_config_setting()`

Retrieves a configuration setting by reading the supplied filename from the application's configuration directory.

#### **Arguments**

- `config_file_name`: A `str` containing the name of the configuration file to read.

#### **Returns**

- A `str` containing a configuration file's contents.

---

### `keep_app_open()`

A utility function that prevents an application from closing.

#### **Arguments**

- None

#### **Returns**

- This function does not return. An infinite loop holds the calling thread until terminated by an outside process.

---

### `services_online()`

Retrieves a list of services that are currently online, based on the receipt of recent heartbeat messages.

#### **Arguments**

- None

#### **Returns**

- A list of services, encapsulated in `Microsoft.Azure.SpaceFx.SDK.Client.ServicesOnline()`, indicating which services have recently transmitted heartbeat messages.

---

### `wait_for_sidecar()`

A utility function that instructs the application to wait until its Dapr sidecar is active.

#### **Arguments**

- `timeout_period` (optional): An `int` specifying how long to wait for the sidecar to activate. Defaults to `30` seconds if not provided.
#### **Returns**

- A list of services, encapsulated in `Microsoft.Azure.SpaceFx.SDK.Client.ServicesOnline()`, indicating which services have recently transmitted heartbeat messages.

#### **Raises**
-   `TimeoutException`: Raises a `TimeoutException` if the Dapr sidecar does not respond within the `timeout_period`.
