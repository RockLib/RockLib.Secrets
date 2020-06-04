using Microsoft.Extensions.Configuration;
using RockLib.Secrets;
using System;

namespace Example.Secrets.Custom
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

            Console.ReadKey();
        }
    }
}
