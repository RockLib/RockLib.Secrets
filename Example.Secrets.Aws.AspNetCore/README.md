# Example.Secrets.Aws.AspNetCore

This example app provides an API with a single endpoint, which returns the UV index of selected cities. The endpoint is `/uv/{city}` and the available cities are Detroit, Chicago, London, Tokyo, and Auckland. For example, `/uv/Tokyo`.

 In order to run this example, there are some prerequisites:

1. [An API Key for the free service at OpenUV](#openuv)
2. [The API Key is stored in an AWS secret](#aws-secret)
3. The machine running the example app has an AWS profile that has access to the above AWS secret. See [Getting Started with the AWS SDK for .NET
](https://docs.aws.amazon.com/sdk-for-net/v2/developer-guide/net-dg-setup.html) for details.

 ## OpenUV

The Open UV API was selected for the Example project because it allowed for the API Key to be passed using an HTTP Request Header, and account/API Key creation is simple and free.

To obtain an Open UV API key:

1. Create an Open UV Index API account.
   - Navigate to https://www.openuv.io/.
   - Select Sign In - this will prompt the user to Sign in with Google.
2. Generate an OpenUV API Key.
   - Navigate to https://www.openuv.io/console.
   - Copy the generated API Key, or generate a new key.

*NOTE: The API Key header name is `x-access-token`.*

## AWS Secret

The Open UV Api Key needs to be stored as an AWS Secret:

1. Log in to the AWS console.
2. Navigate to Secrets Manager.
3. Click on the 'Store a new secret' button.
4. Select 'Other type of secrets'.
5. Add a secret with a key of 'OpenUVApiKey' and a value of the Open UV API Key above.
6. Click 'Next'.
7. Name the secret 'RockLib.Example.Secret'.
8. Click through the remainder of the wizard until the secret is created.
