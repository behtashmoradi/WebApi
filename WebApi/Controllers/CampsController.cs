using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using WebApi.Configurations;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;
        private readonly CampsApiConfiguration options;

        public CampsController(ICampRepository campRepository,IMapper mapper,LinkGenerator linkGenerator,IOptions<CampsApiConfiguration> options)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
            this.options = options.Value;
        }

        [HttpGet]
        public async Task<ActionResult<CampsModel[]>> GetCamps()
        {
            try
            {
                var result = await this.campRepository.GetAllCampsAsync();
                return this.mapper.Map<CampsModel[]>(result);
               
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database issue");
            }

        }

        [HttpGet("{moniker}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CampsModel>> Get(string moniker)
        {
            try
            {
                var result = await this.campRepository.GetCampAsync(moniker);
                return this.mapper.Map<CampsModel>(result);

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database issue");
            }

        }
        [HttpGet("{moniker}")]
        [MapToApiVersion("1.1")]
        public async Task<ActionResult<CampsModel>> Get11(string moniker)
        {
            try
            {
                var result = await this.campRepository.GetCampAsync(moniker,true);
                return this.mapper.Map<CampsModel>(result);

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database issue");
            }

        }
        [HttpPost]
        public async Task<ActionResult<CampsModel>> Post(CampsModel campsModel)
        {
            try
            {
                var location = linkGenerator.GetPathByAction("Get", "Camps", new { moniker = campsModel.Moniker });
                if(string.IsNullOrEmpty(location))
                {
                    return BadRequest();
                }
                var camp = mapper.Map<Camp>(campsModel);
                campRepository.Add(camp);
                if(await campRepository.SaveChangesAsync())
                {
                    return Created(location, camp);
                }

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database issue");
            }
            return BadRequest();

        }
    }
}