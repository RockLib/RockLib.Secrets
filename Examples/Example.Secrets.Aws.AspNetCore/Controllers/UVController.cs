using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Example.Secrets.Aws.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UvController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UvController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{city}")]
        public async Task<double> Get(City city)
        {
            GetCoordinates(city, out var latitude, out var longitude);

            using var httpClient = _httpClientFactory.CreateClient("OpenUV");

            var response = await httpClient.GetAsync($"https://api.openuv.io/api/v1/uv?lat={latitude}&lng={longitude}");

            var content = await response.Content.ReadAsStringAsync();

            dynamic parsedContent = JObject.Parse(content);

            return parsedContent.result.uv;
        }

        private static void GetCoordinates(City city, out double latitude, out double longitude)
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
    }
}
