using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.Dtos;
using ASPNetMicroserviceTemplate.Model;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers 
{
    // Should be removed from the real project!
    [ApiController, Route("api/somemodels")]
    public class SomeModelsController(ISomeModelsRepo repo, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<SomeModelsController> logger) : BaseController<SomeModelsController>(repo, mapper, httpClientFactory, logger)
    {
        #region Functionality
        [HttpGet]
        public ActionResult<IEnumerable<SomeModelReadDto>> GetSomeModels() 
        {
            logger.LogInformation("[INFO]: Getting some models at {Time}", DateTimeNowString);

            var someModelItems = repo.GetAllItems();

            return someModelItems is not null && someModelItems.Any() ?
                Ok(mapper.Map<IEnumerable<SomeModelReadDto>>(someModelItems)) :
                NoContent();
        }

        [HttpGet("{id}", Name = "GetSomeModelById")]
        public ActionResult<SomeModelReadDto> GetSomeModelById(int id) 
        {
            logger.LogInformation("[INFO]: Get some model by id: {id}, at {DateTimeNowString}", id, DateTimeNowString);

            var someModelItem = repo.GetItemById(id);

            return someModelItem is not null ? 
                Ok(mapper.Map<SomeModelReadDto>(someModelItem)) : 
                NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SomeModelReadDto>> CreatSomeModel(SomeModelCreateDto createDto) 
        {
            logger.LogInformation("[INFO]: Create new model at {Time}, with data: {@CreateDto}", DateTimeNowString, createDto);

            var someModel = mapper.Map<SomeModel>(createDto);
            repo.CreateItem(someModel);  
            repo.SaveChanges();

            var readModel = mapper.Map<SomeModelReadDto>(someModel);  

            await Task.Run(() => 
            {
                // Simulate some async operation, e.g., sending a message to a queue
                Debug.WriteLine($"[DEBUG]: Simulated async operation for created model with ID {readModel.Id} at {DateTimeNowString}");
            });

            return readModel is not null ?
                Ok(readModel) :
                BadRequest();

            //return CreatedAtRoute(nameof(GetSomeModelById), new { Id = readModel.Id, readModel });
            // TODO: Работать с возвратом CreatedAtRoute
        } 
        #endregion
    }
}