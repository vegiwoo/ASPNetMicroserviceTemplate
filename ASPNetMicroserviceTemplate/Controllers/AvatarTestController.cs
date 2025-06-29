using ASPNetMicroserviceTemplate.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

 // Test, should be removed from the real project!
namespace ASPNetMicroserviceTemplate.Controllers
{
    [ApiController, Route("api/avatarTest")]
    public class AvatarTestController(ISomeModelsRepo repo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<AvatarTestController> logger) : ControllerBase
    {
        #region Fields
        private readonly ISomeModelsRepo _repo = repo != null ? repo : throw new ArgumentNullException(nameof(repo), "Repo cannot be null");
        private readonly IMapper _mapper = mapper != null ? mapper : throw new ArgumentNullException(nameof(mapper), "Mapper cannot be null");
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory != null ? httpClientFactory : throw new ArgumentNullException(nameof(httpClientFactory), "HTTPClientFactory cannot be null");
        private readonly ILogger<AvatarTestController> _logger = logger != null ? logger : throw new ArgumentNullException(nameof(logger), "Logger cannot be null");

        #endregion
        #region Constructor
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

                
                HttpResponseMessage response;
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    response = await client.GetAsync(avatarUrlString);
                    response.EnsureSuccessStatusCode();

                    _logger.LogInformation("--> Get random avatar with id {avatarId}...", DateTime.UtcNow.ToLongTimeString());
                }

                return response.StatusCode != System.Net.HttpStatusCode.OK ?
                    Problem(detail: $"Failed to retrieve avatar from {avatarUrlString}. Status code: {response.StatusCode}", statusCode: (int)response.StatusCode) :
                    File(await response.Content.ReadAsStreamAsync(), "image/jpeg");
            }
        }
        #endregion
    }
}