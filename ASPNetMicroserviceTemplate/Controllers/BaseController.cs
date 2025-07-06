using System.Net.NetworkInformation;
using ASPNetMicroserviceTemplate.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers
{
    public abstract class BaseController<T>(ISomeModelsRepo repo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<T> logger) : ControllerBase
    {
        #region Fields
        protected readonly ISomeModelsRepo repo = repo ?? throw new ArgumentNullException(nameof(repo), "Repo cannot be null");
        protected readonly IMapper mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Mapper cannot be null");
        protected readonly ILogger<T> logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        protected string DateTimeNowString => DateTime.UtcNow.ToLongTimeString();
        #endregion

        #region Functionality
        protected async Task<ActionResult> SendPing(string address)
        {
            var pingSender = new Ping();
            try
            {
                var reply = await pingSender.SendPingAsync(address);
                if (reply.Status == IPStatus.Success)
                {
                    logger.LogInformation("[INFO]: Ping to {Address} successful at {Time}", address, DateTimeNowString);
                    return Ok($"Ping to {address} successful");
                }
                else
                {
                    logger.LogWarning("[WARNING]: Ping to {Address} failed with status: {Status} at {Time}", address, reply.Status, DateTimeNowString);
                    return StatusCode((int)reply.Status, $"Ping to {address} failed with status: {reply.Status}");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[ERROR]: Exception occurred while pinging {Address} at {Time}", address, DateTimeNowString);
                return StatusCode(500, $"An error occurred while pinging {address}: {ex.Message}");
            }
        }


        #endregion
    }
}