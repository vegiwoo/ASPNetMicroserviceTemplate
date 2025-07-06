using ASPNetMicroserviceTemplate.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers;

// https://hub.docker.com/r/mcp/nasdaq-data-link

[ApiController, Route("api/nasdaqtest")]
public class NasdaqTestController (ISomeModelsRepo repo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<NasdaqTestController> logger) : BaseController<NasdaqTestController>(repo, mapper, httpClientFactory, logger)
{
    [HttpGet("country_code/{country}")]
    public IActionResult GetCountryCode(string country)
    {
        var nasdaqUrlString = Environment.GetEnvironmentVariable("NASDAQ_ENDPOINT");
        if (string.IsNullOrEmpty(nasdaqUrlString)) {
            logger.LogError("[ERROR]: NASDAQ_ENDPOINT is not available at {DateTimeNowString}", DateTimeNowString);
            return Problem("NASDAQ_ENDPOINT is not available");
        }
        else {
            logger.LogInformation("[INFO]: Nasdaq endpoint {Point} available at {DateTime}", nasdaqUrlString, DateTimeNowString);

            ActionResult? pingResult = SendPing(nasdaqUrlString).GetAwaiter().GetResult();
            if (pingResult is not null && pingResult is ObjectResult objectResult && objectResult.StatusCode != 200) {
                return objectResult;
            }

            HttpResponseMessage? response;
            using (HttpClient client = httpClientFactory.CreateClient()) {
                response = client.GetAsync($"{nasdaqUrlString}/country_code/{country}").GetAwaiter().GetResult();
                logger.LogInformation("Get country code for {country} at {Time}", country, DateTimeNowString);
                response.EnsureSuccessStatusCode();
            }

            return response == null || response.StatusCode != System.Net.HttpStatusCode.OK ?
                Problem(detail: $"Failed to retrieve country code from {nasdaqUrlString}. Status code: {response?.StatusCode}", statusCode: (int)response?.StatusCode) :
                Ok(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
        }
    }
}