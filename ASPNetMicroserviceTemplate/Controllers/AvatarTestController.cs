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
            var avatarEnvString = Environment.GetEnvironmentVariable("AVATAR_ENDPOINT");

            
            if (string.IsNullOrEmpty(avatarEnvString))
            {
                logger.LogError("[ERROR]: Avatar service env variable is not available at {DateTimeNowString}", DateTimeNowString);
                return Problem("[ERROR]: Avatar service env variable is not available at {DateTimeNowString}", DateTimeNowString);
            }
            else
            {
                var avatarUrlString = $"http://{avatarEnvString}";
                logger.LogInformation("[INFO]: Robohash service env variables {Point} available at {DateTime}", avatarUrlString, DateTimeNowString);

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