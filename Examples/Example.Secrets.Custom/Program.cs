using Microsoft.Extensions.Configuration;
using RockLib.Secrets;
using System;
using System.Diagnostics;

namespace Example.Secrets.Custom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // This example defines two secrets to be added to configuration. The 'SomeApiKey'
            // secret is added programmatically and the 'MyConnectionString' secret is defined
            // in the appsettings.json file under the 'RockLib.Secrets' section.

            IConfigurationBuilder configBuilder = new ConfigurationBuilder();

            configBuilder.AddJsonFile("appsettings.json");
            
            configBuilder.AddRockLibSecrets()
                .AddSecret(new ReversedSecret("SomeApiKey", "terces dnoces eht si sihT"));

            IConfiguration config = configBuilder.Build();

            Console.WriteLine("MyConnectionString: " + config["MyConnectionString"]);
            Console.WriteLine("SomeApiKey: " + config["SomeApiKey"]);

            Debugger.Break();
        }
    }
}
