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
        public ActionResult<SomeModelReadDto> CreatSomeModel(SomeModelCreateDto createDto) 
        {
            var someModel = mapper.Map<SomeModel>(createDto);
            repo.CreateItem(someModel);  
            repo.SaveShanges();

            var readModel = mapper.Map<SomeModelReadDto>(someModel);  

            // return readModel is not null ?
            //     Ok(readModel) :
            //     BadRequest();

            return CreatedAtRoute(nameof(GetSomeModelById), new { Id = readModel.Id, readModel });
            // TODO: Рабротать с возвратом CreatedAtRoute
        } 
        #endregion
    }
}