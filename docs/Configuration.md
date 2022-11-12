---
sidebar_position: 3
sidebar_label: 'Define secrets in configuration'
---

# How to define secrets in configuration

Any secrets that are defined in the "RockLib.Secrets" section of the *other* sources of a configuration builder are added to the `SecretsConfigurationSource`.

For example, if a configuration builder has JSON file and RockLib secrets sources added:

```csharp
IConfigurationBuilder builder = new ConfigurationBuilder;

builder.AddJsonFile("appsettings.json");

// Note: No secrets are added directly to the builder in this example, but if
// any had been, they would also be available from the builder's configuration.
builder.AddRockLibSecrets();
```

And the `appsettings.json` file defines one or more secrets in its "RockLib.Secrets" section, like this:

```json
{
  "RockLib.Secrets": [
    {
      "Type": "ExampleApp.MyCustomSecret, ExampleApp",
      "Value": {
        "ConfigurationKey": "MyConnectionString"
      }
    }
  ]
}
```

Then when the builder is built, those secrets will be available from the returned `IConfiguration` by the value of their `ConfigurationKey` property.

```csharp
IConfiguration config = builder.Build();

string myConnectionString = config["MyConnectionString"];
```
