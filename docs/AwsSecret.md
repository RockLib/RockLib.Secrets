# How to configure AWS secrets

Add AWS secrets to the `ISecretsConfigurationBuilder` by calling the `AddAwsSecret` method, supplying the following parameters:
- `configurationKey` (required)
  - The configuration key for the secret.
- `secretId` (required)
  - The Amazon Resource Name (ARN) or the friendly name of the secret.
- `secretKey` (optional)
  - The key of the secret in AWS.
- `secretsManager` (optional)
  - The client object used for routing calls to AWS. If not specified, the value of the `AwsSecret.DefaultSecretsManager` static property ([see below](#setting-the-default-amazonsecretsmanager)) is used.

```c#
IConfigurationBuilder builder = new ConfigurationBuilder();

IAmazonSecretsManager secretsManager = new AmazonSecretsManagerClient();

builder.AddRockLibSecrets()
    .AddAwsSecret("MyConnectionString", "MyApp", "ConnectionString", secretsManager)
```

### Setting the default AmazonSecretsManager

To set the default secrets manager, set the value of the static `DefaultSecretsManager` property:

```c#
AwsSecret.DefaultSecretsManager = new AmazonSecretsManagerClient(RegionEndpoint.USEast1);
```

Or call the `SetAmazonSecretsManager` extension method on an instance of `IConfigurationBuilder`:

```c#
IConfigurationBuilder builder = new ConfigurationBuilder();

builder
    .SetAmazonSecretsManager(new AmazonSecretsManagerClient(RegionEndpoint.USEast1))
    .AddRockLibSecrets()
        .AddAwsSecret("MyConnectionString", "MyApp", "ConnectionString");
```

*Note that this extension method does not actually add anything to the configuration builder - it only sets the value of the `AwsSecret.DefaultSecretsManager` property.*
