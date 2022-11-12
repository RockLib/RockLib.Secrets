---
sidebar_position: 2
sidebar_label: 'Add secrets to configuration builder'
---

# How to add secrets directly to a configuration builder

To add secrets to an `IConfigurationBuilder`, call the `AddRockLibSecrets` extension method on the builder, optionally passing an `Action<SecretsConfigurationSource>` for [configuring the source](#SecretsConfigurationSource).

```csharp
IConfigurationBuilder builder = new ConfigurationBuilder();

builder.AddRockLibSecrets(source =>
{
    // TODO: configure source
});
```

This extension method returns an `ISecretsConfigurationBuilder`, which has a `AddSecret` method for adding secrets.

```csharp
IConfigurationBuilder builder = new ConfigurationBuilder();

ISecretsConfigurationBuilder secretsBuilder = builder.AddRockLibSecrets();

secretsBuilder.AddSecret(new MyCustomSecret(configurationKey: "MyConnectionString"));
```

 Implementations of `ISecret`, such as `AwsSecret`, have extension methods for simplifying the adding of a secret:

```csharp
IConfigurationBuilder builder = new ConfigurationBuilder();

ISecretsConfigurationBuilder secretsBuilder = builder.AddRockLibSecrets();

secretsBuilder.AddAwsSecret("MyConnectionString", "MySecretId", "MySecretKey");
```

---

## SecretsConfigurationSource

The `SecretsConfigurationSource` defines several options:

- `Secrets`
  - This property defines the secrets that the source provides.
  - The `ConfigurationBuilder` class adds to this property when its `AddSecret` method is called.
- `OnSecretException`
  - This callback is invoked whenever the `GetValue()` method of an `ISecret` throws an exception.
- `ReloadMilliseconds`
  - This property defines how often the configuration provider reloads its data. Specify `Timeout.Infinite` or call the `DisableReload()` method to disable reloading.
