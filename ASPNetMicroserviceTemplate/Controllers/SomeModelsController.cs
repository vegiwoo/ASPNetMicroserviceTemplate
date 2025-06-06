using System.Diagnostics;
using ASPNetMicroserviceTemplate.Data;
using ASPNetMicroserviceTemplate.Dtos;
using ASPNetMicroserviceTemplate.Model;
using ASPNetMicroserviceTemplate.SyncDataServices.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ASPNetMicroserviceTemplate.Controllers 
{
    // Should be removed from the real project!
    // "anotherServiceDataClient" implement in target controller for link another service
    [ApiController, Route("api/somemodels")]
    public class SomeModelsController(ISomeModelsRepo repo, IMapper mapper, IAnotherServiceDataClient anotherServiceDataClient) : ControllerBase
    {
        #region Fields
        private readonly ISomeModelsRepo repo = repo;
        private readonly IMapper mapper = mapper;
        private readonly IAnotherServiceDataClient anotherServiceDataClient = anotherServiceDataClient;
        #endregion

        #region Functionality
        [HttpGet]
        public ActionResult<IEnumerable<SomeModelReadDto>> GetSomeModels() 
        {
            Trace.WriteLine("--> Getting some models");

            var someModelItems = repo.GetAllItems();

            return someModelItems is not null && someModelItems.Any() ? 
                Ok(mapper.Map<IEnumerable<SomeModelReadDto>>(someModelItems)) : 
                NoContent();
        }

        [HttpGet("{id}", Name = "GetSomeModelById")]
        public ActionResult<SomeModelReadDto> GetSomeModelById(int id) 
        {
            Trace.WriteLine($"--> Get some model by id {id}");

            var someModelItem = repo.GetItemById(id);

            return someModelItem is not null ? 
                Ok(mapper.Map<SomeModelReadDto>(someModelItem)) : 
                NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<SomeModelReadDto>> CreatSomeModel(SomeModelCreateDto createDto) 
        {
            var someModel = mapper.Map<SomeModel>(createDto);
            repo.CreateItem(someModel);  
            repo.SaveShanges();

            var readModel = mapper.Map<SomeModelReadDto>(someModel);  

            // ??? 
            try
            {
                await anotherServiceDataClient.SendPostFromThisServiceToAnotherService(readModel);
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine($"Could not send synchronously to another service {ex.Message}");
            }

            return readModel is not null ?
                Ok(readModel) :
                BadRequest();

            //return CreatedAtRoute(nameof(GetSomeModelById), new { Id = readModel.Id, readModel });
            // TODO: Работать с возвратом CreatedAtRoute
        } 
        #endregion
    }
}