using ASPNetMicroserviceTemplate.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

 // Test, should be removed from the real project!
namespace ASPNetMicroserviceTemplate.Controllers
{
    [ApiController, Route("api/avatarTest")]
    public class AvatarTestController(ISomeModelsRepo repo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<AvatarTestController> logger) : BaseController<AvatarTestController>(repo, mapper, httpClientFactory, logger)
    {
        #region Functionality
        [HttpGet]
        public async Task<ActionResult> GetImage()
        {
            var avatarUrlString = Environment.GetEnvironmentVariable("AVATAR_ENDPOINT");
            if (string.IsNullOrEmpty(avatarUrlString))
            {
                logger.LogError("[ERROR]: AVATAR_ENDPOINT is not available at {DateTimeNowString}", DateTimeNowString);
                return Problem("AVATAR_ENDPOINT is not available");
            }
            else
            {
                logger.LogInformation("[INFO]: Avatar endpoint {Point} available at {DateTime}", avatarUrlString, DateTimeNowString);

                ActionResult? pingResult = await SendPing(avatarUrlString);
                if(pingResult is not null && pingResult is ObjectResult objectResult && objectResult.StatusCode != 200)
                {
                    return objectResult;
                }

                Random random = new();
                int avatarId = (int)random.NextInt64(1, 99);
                avatarUrlString = string.Concat(avatarUrlString, "/", avatarId);

                HttpResponseMessage? response;
                using (HttpClient client = httpClientFactory.CreateClient())
                {
                    response = await client.GetAsync(avatarUrlString);
                    logger.LogInformation("Get random avatar with id {avatarId} at {Time}", avatarId, DateTimeNowString);
                    response.EnsureSuccessStatusCode();
                }

                return response == null || response.StatusCode != System.Net.HttpStatusCode.OK ?
                    Problem(detail: $"Failed to retrieve avatar from {avatarUrlString}. Status code: {response?.StatusCode}", statusCode: (int)response?.StatusCode) :
                    File(await response.Content.ReadAsStreamAsync(), "image/jpeg");
            }
        }
        #endregion
    }
}