# Getting Started

In this tutorial, we will be building a console application that prints out the value of secrets read from configuration, by reversing the secret text.

---

Create a .NET Core console application named "SecretsTutorial".

---

Add a nuget reference for "RockLib.Secrets" to the project.

---

Add a class named 'ReversedSecret.cs' to the project. Replace the default code with the following:

```c#
using RockLib.Secrets;
using System.Linq;

namespace SecretsTutorial
{
    public class ReversedSecret : ISecret
    {
        private readonly string _secret;

        public ReversedSecret(string configurationKey, string secret)
        {
            ConfigurationKey = configurationKey;
            _secret = secret;
        }

        public string ConfigurationKey { get; }

        public string GetValue()
        {
            return new string(_secret.ToCharArray().Reverse().ToArray());
        }
    }
}
```
---

Add a new JSON file to the project named 'appsettings.json'. Set its 'Copy to Output Directory' setting to 'Copy if newer'. Add the following configuration:

```json
{
  "FirstSecretKey": "the first secret has not been applied yet",
  "SecondSecretKey": "the second secret has not been applied yet",
  "ThirdSecretKey": "the third secret has not been applied yet",

  "RockLib.Secrets": [
    {
      "Type": "SecretsTutorial.ReversedSecret, SecretsTutorial",
      "Value": {
        "ConfigurationKey": "FirstSecretKey",
        "Secret": "terces tsrif eht si sihT"
      }
    },
    {
      "Type": "SecretsTutorial.ReversedSecret, SecretsTutorial",
      "Value": {
        "ConfigurationKey": "SecondSecretKey",
        "Secret": "terces dnoces eht si sihT"
      }
    },
    {
      "Type": "SecretsTutorial.ReversedSecret, SecretsTutorial",
      "Value": {
        "ConfigurationKey": "ThirdSecretKey",
        "Secret": "terces driht eht si sihT"
      }
    }
  ]
}
```

This configuration will set up three secrets that can be accessed through config via the following keys, "FirstSecretKey", "SecondSecretKey", or "ThirdSecretKey".

---

Edit the `Program.cs` file as follows:

```c#
using Microsoft.Extensions.Configuration;
using RockLib.Secrets;
using System;
using System.Diagnostics;

namespace SecretsTutorial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("appsettings.json");
            configBuilder.AddRockLibSecrets();

            var config = configBuilder.Build();

            Console.WriteLine("First Secret: " + config["FirstSecretKey"]);
            Console.WriteLine("Second Secret: " + config["SecondSecretKey"]);
            Console.WriteLine("Third Secret: " + config["ThirdSecretKey"]);

            Debugger.Break();
        }
    }
}
```

---

Start the application. It should print out the reversed secrets, like below:

```
First Secret: This is the first secret
Second Secret: This is the second secret
Third Secret: This is the third secret
```
