---
sidebar_position: 4
sidebar_label: 'Use secrets from RockLib.Secrets'
---

# How to use the secrets from RockLib.Secrets

*There's not much to do.*

## From a .NET Core app with DI

For .NET Core applications that use an `IHostBuilder` or `IWebHostBuilder` and call the `ConfigureAppConfiguration` and `AddRockLibSecrets` extension methods, the `IConfiguration` that is DI registered contains the values of the provider's secrets. Such applications have a Program.cs similar to this:

```csharp
Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder => builder.AddRockLibSecrets());
```

Controllers and servivces can access secret values through configuration like this:

```csharp
public class MyService
{
    private readonly IConfiguration _config;

    public MyService(IConfiguration config)
    {
        _config = config;
    }

    public void DoSomething()
    {
        string connectionString = _config["MyConnectionString"];
    }
}
```

## From a built IConfigurationBuilder

When building an `IConfiguration` or `IConfigurationRoot` directly, secrets can be accessed by their key:

```csharp
IConfigurationBuilder configBuilder = new ConfigurationBuilder();

configBuilder.AddRockLibSecrets()
    .AddSecret(new MyCustomSecret(configurationKey: "MyConnectionString"));

IConfiguration config = configBuilder.Build();

// Variable will contain whatever MyCustomSecret.GetValue() returned.
string myConnectionString = config["MyConnectionString"];
```

## From Config.Root

The `Config.Root` property from the RockLib.Configuration library automatically adds the secrets defined in its default configuration. See [defining secrets in configuration](Configuration.md) for more details.

```json
{
  "MyConnectionString": "the connection string secret has not been applied yet",

  "RockLib.Secrets": {
    "Type": "ExampleApp.MyCustomSecret, ExampleApp",
    "Value": {
      "ConfigurationKey": "MyConnectionString"
    }
  }
}
```

Having the above `appsettings.json` file would allow access to the secrets like this:

```csharp
using RockLib.Configuration;
. . .
string myConnectionString = Config.Root["MyConnectionString"];
```
