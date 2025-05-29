using ASPNetMicroserviceTemplate.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

 // Test, should be removed from the real project!
namespace ASPNetMicroserviceTemplate.Controllers
{
    [ApiController, Route("avatarTest")]
    public class AvatarTestController(ISomeModelsRepo repo, IMapper mapper) : ControllerBase
    {
        #region Fields
        private readonly ISomeModelsRepo repo = repo;
        private readonly IMapper mapper = mapper;
        #endregion

        #region Functionality
        [HttpGet]
        public ActionResult GetImage()
        {
            var avatarUrlString = Environment.GetEnvironmentVariable("AVATAR_ENDPOINT");
            if (string.IsNullOrEmpty(avatarUrlString))
            {
                return Problem("AVATAR_ENDPOINT is not available");
            }
            else
            {
                Random random = new();
                avatarUrlString = string.Concat(avatarUrlString, "/", (int)random.NextInt64(1, 99));
                return Ok(File(avatarUrlString, "image/jpeg"));
            }
        }
        #endregion
    }
}