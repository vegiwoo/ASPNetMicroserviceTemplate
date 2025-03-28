using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ASPNetMicroserviceTemplate.Dtos;

namespace ASPNetMicroserviceTemplate.SyncDataServices.Http
{
    /// Should be removed from the real project!
    public class AnotherServiceDataClient : IAnotherServiceDataClient
    {
        #region Fields
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The name of the section in the appsettings.json file
        /// that contains the configuration for the another service.
        /// </summary>
        private readonly IConfigurationSection? AnotherServiceSection;

        private readonly string? anotherServiceBaseUrl;
        private readonly string? anotherServicePostPath;
        #endregion
        
        #region Constructors
        public AnotherServiceDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            AnotherServiceSection = _configuration.GetSection(IAnotherServiceDataClient.AnotherServiceSectionName);
            anotherServiceBaseUrl = AnotherServiceSection.GetValue<string>(IAnotherServiceDataClient.AnotherServiceBaseUrlName);
            anotherServicePostPath = AnotherServiceSection.GetValue<string>(IAnotherServiceDataClient.AnotherServicePostPathName);
        }
        #endregion

        #region Functionality
        public async Task SendPostFromThisServiceToAnotherService(SomeModelReadDto model)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(model), 
                Encoding.UTF8, 
                "application/json");

            // address is a placeholder for the real address
            // of the another service to which we want to send data
            // e.g. "/api/someModels"
            var response = await _httpClient.PostAsync($"{anotherServiceBaseUrl}{anotherServicePostPath}", httpContent);

            Trace.WriteLine(response.IsSuccessStatusCode ? 
               "--> POST to AnotherService was OK!" : 
                "--> POST to AnotherService was NOT OK!");
            
            //return Task.CompletedTask; // Dummy return
         }
        #endregion
    }
}