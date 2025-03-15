using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers 
{
    // Should be removed from the real project!
    [ApiController, Route("api/somemodels")]
    public class SomeModelsController(ISomeModelsRepo repo, IMapper mapper) : ControllerBase
    {
        #region Fields
        private readonly ISomeModelsRepo repo = repo;
        private readonly IMapper mapper = mapper;
        #endregion

        #region Functionality
        [HttpGet]
        public ActionResult<IEnumerable<SomeModelReadDto>> GetSomeModels() 
        {
            Trace.WriteLine("--> Getting Some models");

            var someModelItems = repo.GetAllItems();
            return someModelItems is not null && someModelItems.Any() ? 
                Ok(mapper.Map<IEnumerable<SomeModelReadDto>>(someModelItems)) : 
                NoContent();
        }
        #endregion
    }
}