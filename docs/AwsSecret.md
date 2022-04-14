# How to configure AWS secrets

Add AWS secrets to the `ISecretsConfigurationBuilder` by calling the `AddAwsSecret` method, supplying the following parameters:
- `configurationKey` (required)
  - The configuration key for the secret.
- `secretId` (required)
  - The Amazon Resource Name (ARN) or the friendly name of the secret.
- `secretKey` (optional)
  - The key of the secret in AWS.
- `secretsManager` (optional)
  - The client object used for routing calls to AWS. Typically, this is an instance of `AmazonSecretsManagerClient`.

```c#
var builder = new ConfigurationBuilder();
builder.AddRockLibSecrets()
  .AddAwsSecret("MyConnectionString", "MyApp", "ConnectionString", new AmazonSecretsManagerClient())
```