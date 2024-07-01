# `Microsoft.Azure.SpaceFx.SDK.Client`

The `Microsoft.Azure.SpaceFx.SDK.Client` namespace provides a client that initializes your application's connection to the Azure Orbital Space SDK runtime framework.

## `Client` Class

### Public Methods

#### `Build()`

Initializes the GRPC channel to the Dapr sidecar, serving as the entry point to the Microsoft Azure SpaceFx.

##### **Arguments**

- `TimeSpan? MessageResponseTimeout = null` (optional): The amount of time to wait for a response to a message that has ben sent. Defaults to `30 seconds`.
- `TimeSpan? PollingTime = null` (optional): The amount of time to wait between checks for new messages. A lower polling time makes applications more responsive, but contribute a higher computational load on the satellite. Defaults to `250 milliseconds`.

##### **Returns**

- An initialized instance of `Microsoft.Azure.SpaceFx.SDK.Client`.

---

#### `KeepAppOpen()`

A utility function that prevents an application from closing.

##### **Arguments**

- None

##### **Returns**

- This function does not return. An infinite loop holds the calling thread until terminated by an outside process.

---

#### `Shutdown()`

Stops the `Microsoft.Azure.SpaceFx.SDK.Client` object and disposes all resources.

##### **Arguments**

- None

##### **Returns**

- None

##### **Raises***

- `System.Exception`: Raises an exception if the client has not been initialized.

---

#### `ServicesOnline()`

Retrieves a list of services that are currently online, based on the receipt of recent heartbeat messages.

##### **Arguments**

- None

##### **Returns**

- `List<MessageFormats.Common.HeartBeatPulse>`: A list containing the most recent heartbeat messages of all services.

##### **Raises***

- `System.Exception`: Raises an exception if the client has not been initialized.

---

#### `WaitForOnline()`

A utility function that instructs the application to wait until its Dapr sidecar is active.

##### **Arguments**

- `TimeSpan? timeOut = null` (optional): The amount of time to wait for a response from the Dapr sidecar. Defaults to `30 seconds`.

##### **Returns**

- `Task<Core.Enums.SIDECAR_STATUS>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.Core.Enums.SIDECAR_STATUS` result.

##### **Raises***

- `System.Exception`: Raises an exception if the client has not been initialized.
- `System.TimeoutException`: Raises a timeout exception if the Dapr sidecar has not responded within the timeout window.

---

#### `DirectToApp()`

Sends a message directly to another application running within the Azure Orbital Space SDK runtime framework.

##### **Arguments**

- `string appId`: The ID of the application to message.
- `Google.Protobuf.IMessage message`: The message to be transmitted.

##### **Returns**

- `Task`: Returns a `System.Threading.Tasks.Task`.

##### **Raises***

- `System.Exception`: Raises an exception if the client has not been initialized.

---

#### `Client()`

Constructs a new `Microsoft.Azure.SpaceFx.SDK.Client` object.

##### **Arguments**

- None

##### **Returns**

- `Microsoft.Azure.SpaceFx.SDK.Client`: Returns a new `Microsoft.Azure.SpaceFx.SDK.Client` object.

---

## `MessageHandler` Class

Extension of the `Google.Protobuf.IMessage` class that provides protobuf message handling and routing capabilities.

### Public Methods

#### `MessageReceived()`

Routes messages to the applicable registered EventHandler.

##### **Arguments**

- `Google.Protobuf.IMessage message`: A protobuf message specifying the message type.
- `Microsoft.Azure.SpaceFx.MessageFormats.Common.DirectToApp fullMessage`: The received message.

##### **Returns**

- None
