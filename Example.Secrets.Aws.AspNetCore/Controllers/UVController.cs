using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Example.Secrets.Aws.AspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UVController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UVController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("{city}")]
        public async Task<float> Get(City city)
        {
            float latitude, longitude;

            latitude = 42;
            longitude = -83;

            switch (city)
            {
                case City.Detroit:
                    break;
                case City.Chicago:
                    break;
                case City.London:
                    break;
                case City.Tokyo:
                    break;
                case City.Auckland:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(city), city, null);
            }

            using var httpClient = _httpClientFactory.CreateClient("OpenUV");

            var response = await httpClient.GetAsync($"https://api.openuv.io/api/v1/uv?lat={latitude}&lng={longitude}");

            var content = await response.Content.ReadAsStringAsync();

            dynamic parsedContent = JObject.Parse(content);

            return parsedContent.result.uv;
        }
    }
}
