# A simple agent template
### Note: This project is a Work in progress


## Spec

The project is a .NET Core project using .NET 8

The template addes support for common enterprise scale concerns such as 
- Telemtry (Logging, Metrics, Tracing)
- Health Checks
- Configuration
- Feature Flags

### Telemetry
The template mainly uses __OpenTelemetry__ with the _OTLP_ protocol exporter configured in the 'appsettings.{environment}.json' file, and looks like this

```json
"OpenTelemetry": {
    "Logging": {
      "Enabled": true,
      "Endpoint": "http://127.0.0.1:38889",
      "Headers": {
      }
    },
    "Tracing": {
      "Enabled": true,
      "Endpoint": "http://127.0.0.1:38889",
      "Headers": {
      }
    },
    "Metrics": {
      "Enabled": true,
      "Endpoint": "http://127.0.0.1:38889",
      "Headers": {
      }
    }
  }
```

Add your OTLP endpoint address and you can add your custom header values as a json dictionary in the _Headers_ field

> NOTE <br/>
  You can use any other custom OpenTelemetry Exporter of your choosing by changing this part of the program file and adding the extra exporter

```c#
private static void ConfigureOpenTelemetry(IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
...

services.AddOpenTelemetry()
            .WithTracing(tracing =>

...

if (config.Enabled)
{
    metrics.AddOtlpExporter(otlp =>
    {
        otlp.Headers = config.JoinedHeaders;
        otlp.Endpoint = new Uri(config.Endpoint!);
        otlp.TimeoutMilliseconds = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
    });
}

...

if (config.Enabled)
{

    tracing.AddOtlpExporter(otlp =>
    {
        otlp.Headers = config.JoinedHeaders;
        otlp.Endpoint = new Uri(config.Endpoint!);
        otlp.TimeoutMilliseconds = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
    });

    // Add your other exporter configurations here
}

...
```

### Custom Telemetry

- ##### Logging

The project uses __Serilog__ As the log _formatter_ and the otlp exporter as the log _exporter_
meaning you can use seilog's data enrichment capabilities alongside the logs being exported

configure serilog here

```c#
    private static void ConfigureSerilog(IConfiguration configuration, IHostEnvironment env, LoggerConfiguration serilog)
    {
        serilog
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            .ReadFrom.Configuration(configuration.GetSection("Logging"))
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithEnvironmentUserName();
    }
```

- ##### Metrics

To create your own custom metrics simply inject a default `Meter` class from your DI into your classes and create `Counter`s, `Histogram`s and other from the meter. the custom metrics are expoterd by default

- ##### Traces

To create custom Traces inject a default `ActivitySource` class into your classes from your DI and create `Activity` objects from it and run them.
the custom traces are exported by default

<hr />

### Health Checks

the app exposes health checks

##### TODO: configure a way for healthchecks withour exposing apis

#### Adding Health Checks

Most popular tools used with dotnet have an open source healthcheck package. Install the package and add the required configuration to this part of the code

```c#
public static void ConfigureHealthChecks(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
{
    // try chaining all future required health checks here for: external services, databases, etc
    services
        .AddHealthChecks();
}
```

<hr />

### Configuration

The app uses several sources for configuration including _Env Variables_, _appsettings.json_ files and _input args_

The configuration follows the microsoft default patter. Read More about them __[Here](https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration)__

<hr />

### Feature Flags
The app makes use of feature flags, by default features are defined in the `features.json` file as a dictionary of &lt;name&gt;: &lt;bool&gt; values
extra configuration cn also be applied. Read about them __[Here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core#feature-flag-declaration)__

#### Usage:
Define your Feature names in the `features.json` file, Add the name in the `FeatureNames` file as _constants_.

```c#
public class FeatureNames
{
    public const string DevOnly = nameof(DevOnly);
    // add others here
}
```

You can also inject the `IFeatureManager` Interface to check wheter a feature is active or not to enable or disable code logic

> NOTE <br />
  You can disable feature management for your development without modifying the features.json file by setting the 
  `DEV_APP_FeatureFlags__Disabled` (Development env only) or `APP_FeatureFlags__Disabled` variables to `true`
  
<hr />
