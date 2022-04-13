using Example.Secrets.Aws.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RockLib.Secrets;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetSecretExceptionHandler(context => Console.WriteLine(context.Exception))
    .AddRockLibSecrets();

builder.Services.AddHttpClient("OpenUV", 
    client => client.DefaultRequestHeaders.Add("x-access-token", builder.Configuration["OpenUVApiKey"]));

var application = builder.Build();

application.MapGet("/{city}", async (IHttpClientFactory factory, [FromRoute] City city) =>
{
    GetCoordinates(city, out var latitude, out var longitude);

    using var httpClient = factory.CreateClient("OpenUV");
    var response = await httpClient.GetAsync(new Uri($"https://api.openuv.io/api/v1/uv?lat={latitude}&lng={longitude}")).ConfigureAwait(false);
    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

    dynamic parsedContent = JObject.Parse(content);

    return parsedContent.result.uv;
});

if (application.Environment.IsDevelopment())
{
    application.UseDeveloperExceptionPage();
}

application.UseHttpsRedirection();
application.UseRouting();
application.UseAuthorization();
application.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

application.Run();

static void GetCoordinates(City city, out double latitude, out double longitude)
{
    switch (city)
    {
        case City.Detroit:
            latitude = 42.331389;
            longitude = -83.045833;
            break;

        case City.Chicago:
            latitude = 41.881944;
            longitude = -87.627778;
            break;

        case City.London:
            latitude = 51.507222;
            longitude = -0.1275;
            break;

        case City.Tokyo:
            latitude = 35.689722;
            longitude = 139.692222;
            break;

        case City.Auckland:
            latitude = -36.840556;
            longitude = 174.74;
            break;

        default:
            throw new ArgumentOutOfRangeException(nameof(city), city, null);
    }
}

/*

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using RockLib.Secrets;

namespace Example.Secrets.Aws.AspNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                    builder.SetSecretExceptionHandler(context => Console.WriteLine(context.Exception))
                        .AddRockLibSecrets())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
*/