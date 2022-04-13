using Example.Secrets.Custom;
using Microsoft.Extensions.Configuration;
using RockLib.Secrets;
using System;
using System.Diagnostics;

// This example defines two secrets to be added to configuration. The 'SomeApiKey'
// secret is added programmatically and the 'MyConnectionString' secret is defined
// in the appsettings.json file under the 'RockLib.Secrets' section.

var builder = new ConfigurationBuilder();

builder.AddJsonFile("appsettings.json");
builder.AddRockLibSecrets()
    .AddSecret(new ReversedSecret("SomeApiKey", "terces dnoces eht si sihT"));

var configuration = builder.Build();

Console.WriteLine("MyConnectionString: " + configuration["MyConnectionString"]);
Console.WriteLine("SomeApiKey: " + configuration["SomeApiKey"]);