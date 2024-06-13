# `Microsoft.Azure.SpaceFx.SDK.Link`

The `Microsoft.Azure.SpaceFx.SDK.Link` namespace provides functionalities for managing file transfers between your application host services, and other applications running within the Azure Orbital Space SDK runtime framework.

## `public class Link`

### Public Methods

#### `public static Task<MessageFormats.HostServices.Link.LinkResponse> SendFileToApp`

##### **Arguments**

- `string destinationAppId`: A `string` containing the ID of the destination application.
- `string file`: A `string` containing the local path to the file to be sent.
- `bool overwriteDestinationFile = false` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `false`.
- `int? responseTimeoutSecs = null` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Link.LinkResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Link.LinkResponse` result.

---

#### `public static Task<MessageFormats.HostServices.Link.LinkResponse> DownlinkFile`

Sends a file to Message Translation Service (MTS) to downlink to the ground at the next available opportunity.

##### **Arguments**

- `string destinationAppId`: A `string` containing the ID of the destination application.
- `string file`: A `string` containing the local path to the file to be sent.
- `bool overwriteDestinationFile = false` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `false`.
- `int? responseTimeoutSecs = null` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Link.LinkResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Link.LinkResponse` result.

---

#### `public static Task<MessageFormats.HostServices.Link.LinkResponse> CrosslinkFile`

Crosslinks a file to the destination service's inbox directory.

##### **Arguments**

- `string destinationAppId`: A `string` containing the ID of the destination application.
- `string file`: A `string` containing the local path to the file to be sent.
- `bool overwriteDestinationFile = false` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `false`.
- `int? responseTimeoutSecs = null` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Link.LinkResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Link.LinkResponse` result.

---

#### `public static Task<MessageFormats.HostServices.Link.LinkResponse> SendLinkRequest`

Sends a link request to the link host service.

##### **Arguments**

- `MessageFormats.HostServices.Link.LinkRequest linkRequest`: A `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Link.LinkRequest` message to be sent.
- `string file`: A `string` containing the local path to the file to be sent.
- `bool overwriteDestinationFile = false` (optional): A `bool` indicating if the file should be overwritten at its destination if it already exists. Defaults to `false`.
- `int? responseTimeoutSecs = null` (optional): An `int` indicating the number of seconds to wait for a successful LinkResponse message. Defaults to `Microsoft.Azure.SpaceFx.SDK.Client.MessageResponseTimeout`.

##### **Returns**

- `Task<MessageFormats.HostServices.Link.LinkResponse>`: Returns a `System.Threading.Tasks.Task` with a `Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Link.LinkResponse` result.
