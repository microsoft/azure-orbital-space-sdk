# Azure Orbital Space SDK - Logging Service (hostsvc-logging)

The Logging Host Service provides comprehensive logging capabilities for payload applications and components of the runtime framework. This service facilitates the collection and downlink of service logs, enabling developers and operators to monitor system performance, troubleshoot issues, and ensure the integrity of mission-critical operations.

## Key Features

- **Comprehensive Log Collection**: Captures detailed logs from all components of the satellite and ground station operations, including system events, operational data, and error messages.

- **Advanced Analysis Tools**: Integrates with analysis tools and services, offering powerful capabilities for log data filtering, searching, and visualization, to derive actionable insights.

## Use Cases

- **System Performance Monitoring**: Enables continuous monitoring of system health and performance metrics, helping operators maintain optimal operation of satellite payloads and ground station equipment.

- **Issue Troubleshooting and Resolution**: Facilitates the rapid identification and resolution of issues, with detailed logs providing critical insights into system anomalies and operational errors.

## Logging Host Service Operations and Workflows

### Logging

The logging host service provides payload applications and other components of the runtime framework a common interface for the collection and downlink of service logs. Aligned to the [OpenTelemetry Logging Standard](https://opentelemetry.io/docs/specs/otel/logs/), the logging host service provides a uniform log data model that can be used by many different log analysis tools on Earth.

Additionally, the logging host service emphasizes the importance of log correlation and the use of logging levels to enhance the observability and manageability of satellite operations. Log correlation is a critical feature that allows developers and operators to trace events across different components of the operators and ground station systems. Each service and payload application automatically assigns a unique identifier to its log entries, helping developers efficiently track the flow of operations, diagnose issues, and understand the relationships between disparate events. This capability is particularly valuable in complex systems where operations span multiple services and components.

Furthermore, the logging host service supports a hierarchy of logging levels, enabling developers to categorize log messages by their severity and importance. By leveraging these levels, developers can fine-tune their logging strategy, ensuring that critical issues are highlighted and addressed promptly while less urgent information is logged for routine analysis. This structured approach to logging not only aids in the effective monitoring and troubleshooting of systems but also helps in managing the volume of log data, ensuring that storage and analysis resources are focused on the most impactful information.

#### Logging Examples

The following C# example demonstrates how to publish a log message at the `INFO` level using the Azure Orbital Space SDK .NET Client Library:

```csharp
using System.Threading.Tasks;

using Microsoft.Azure.SpaceFx;
using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Logging;

Client.Build();

Task<LogMessageResponse> logMessageRequestTask = Logging.SendLogMessage(logMessage: "Hello, Space World!");
logMessageRequestTask.Wait();

LogMessageResponse logMessageResponse = logMessageRequestTask.Result;
```

The following Python example demonstrates how to publish a log message at the `INFO` level using the Azure Orbital Space SDK Python Client Library:

```python
import logging

import spacefx

logger = spacefx.logger(level=logging.INFO)

spacefx.client.build()

logger.info("Hello, Space World!")
```

> **NOTE**: See logging host service documentation for the [.NET Client Library](../../software-development-kit/client-libraries/dotnet/logging/logging.md) or the [Python Client Library](../../software-development-kit/client-libraries/python/logging/logging.md) for guidance on additional logging levels.

### Telemetry

The logging host service also provides payload applications and other components of the runtime framework a common interface for the collection and downlink of metrics and other telemetry. Aligned to the [OpenTelemetry Metrics Standard](https://opentelemetry.io/docs/concepts/signals/metrics/), the logging host service provides a uniform log data model that can be used by many different metric analysis tools on Earth.

Metric aggregation plays a crucial role in synthesizing vast amounts of telemetry data into meaningful insights. This process involves summarizing and combining various metrics to provide a clear overview of satellite health and performance over time.

By employing aggregation, operators can efficiently monitor trends, detect anomalies, and make informed decisions without being overwhelmed by the sheer volume of raw data. The SDK supports various aggregation methods through the OpenTelemetry standard, allowing for flexible and powerful analysis tailored to the specific needs of each satellite mission. Implementing metric aggregation effectively reduces data storage and processing requirements while enhancing the ability to maintain optimal operational standards.

#### Telemetry Examples

The following C# example demonstrates how to publish a metric using the Azure Orbital Space SDK .NET Client Library:

```csharp
using System.Threading.Tasks;

using Microsoft.Azure.SpaceFx;
using Microsoft.Azure.SpaceFx.MessageFormats.HostServices.Logging;

Client.Build();

Task<TelemetryMetricResponse> telemetryMetricRequestTask = Logging.SendTelemetry(
    metricName: "Some Metric",
    metricValue: 12345,
    waitForResponse: false
)
telemetryMetricRequestTask.Wait();

TelemetryMetricResponse telemetryMetricResponse = telemetryMetricRequestTask.Result;
```

The following Python example demonstrates how to publish a metric using the Azure Orbital Space SDK Python Client Library:

```python
import spacefx

spacefx.client.build()

telemetry_response = spacefx.logging.send_telemetry("Some Metric", 12345)
```