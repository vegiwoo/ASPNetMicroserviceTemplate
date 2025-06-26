using ASPNetMicroserviceTemplate.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

 // Test, should be removed from the real project!
namespace ASPNetMicroserviceTemplate.Controllers
{
    [ApiController, Route("api/avatarTest")]
    public class AvatarTestController(ISomeModelsRepo repo, IMapper mapper, IHttpClientFactory httpClientFactory,  ILogger<AvatarTestController> logger) : ControllerBase
    {
        #region Fields
        private readonly ISomeModelsRepo repo = repo;
        private readonly IMapper mapper = mapper;
        private readonly IHttpClientFactory httpClientFactory = httpClientFactory;
        private readonly ILogger<AvatarTestController> logger = logger;
        #endregion

        #region Functionality
        [HttpGet]
        public async Task<ActionResult> GetImage()
        {
            var avatarUrlString = Environment.GetEnvironmentVariable("AVATAR_ENDPOINT");
            if (string.IsNullOrEmpty(avatarUrlString))
            {
                return Problem("AVATAR_ENDPOINT is not available");
            }
            else
            {
                Random random = new();
                int avatarId = (int)random.NextInt64(1, 99);
                avatarUrlString = string.Concat(avatarUrlString, "/", avatarId);

                logger.LogInformation($"--> Get random avatar with id {avatarId}...", DateTime.UtcNow.ToLongTimeString());

                // TODO: Передавать фабрику! 
                HttpClient client = httpClientFactory.CreateClient();
                
                HttpResponseMessage response = await client.GetAsync(avatarUrlString);
                response.EnsureSuccessStatusCode();
                return File(await response.Content.ReadAsStreamAsync(), "image/jpeg");
            }
        }
        #endregion
    }
}