using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.Dtos;
using ASPNetMicroserviceTemplate.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers 
{
    // Should be removed from the real project!
    // "anotherServiceDataClient" implement in target controller for link another service
    [ApiController, Route("api/somemodels")]
    public class SomeModelsController(ISomeModelsRepo repo, IMapper mapper, ILogger<SomeModelsController> logger) : ControllerBase
    {
        #region Fields
        private readonly ISomeModelsRepo repo = repo;
        private readonly IMapper mapper = mapper;
        private readonly ILogger logger = logger;
        #endregion

        #region Functionality
        [HttpGet]
        public ActionResult<IEnumerable<SomeModelReadDto>> GetSomeModels() 
        {
            logger.LogInformation("--> Getting some models...", DateTime.UtcNow.ToLongTimeString());

            var someModelItems = repo.GetAllItems();

            return someModelItems is not null && someModelItems.Any() ?
                Ok(mapper.Map<IEnumerable<SomeModelReadDto>>(someModelItems)) :
                NoContent();
        }

        [HttpGet("{id}", Name = "GetSomeModelById")]
        public ActionResult<SomeModelReadDto> GetSomeModelById(int id) 
        {
            logger.LogInformation($"--> Get some model by id {id}...", DateTime.UtcNow.ToLongTimeString());

            var someModelItem = repo.GetItemById(id);

            return someModelItem is not null ? 
                Ok(mapper.Map<SomeModelReadDto>(someModelItem)) : 
                NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SomeModelReadDto>> CreatSomeModel(SomeModelCreateDto createDto) 
        {
            logger.LogInformation("--> Create new model...", DateTime.UtcNow.ToLongTimeString());

            var someModel = mapper.Map<SomeModel>(createDto);
            repo.CreateItem(someModel);  
            repo.SaveChanges();

            var readModel = mapper.Map<SomeModelReadDto>(someModel);  

            return readModel is not null ?
                Ok(readModel) :
                BadRequest();

            //return CreatedAtRoute(nameof(GetSomeModelById), new { Id = readModel.Id, readModel });
            // TODO: Работать с возвратом CreatedAtRoute
        } 
        #endregion
    }
}