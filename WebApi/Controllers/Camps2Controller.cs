using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/Camps")]
    [ApiVersion("2.0")]
    [ApiController]
    public class Camps2Controller : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public Camps2Controller(ICampRepository campRepository,IMapper mapper,LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult> GetCamps()
        {
            try
            {
                var result = await this.campRepository.GetAllCampsAsync();
                var output = new
                {
                    count = result.Length,
                    data = this.mapper.Map<CampsModel[]>(result)
                };

                return Ok(output);
               
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database issue");
            }
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult> Get(string moniker)
        {
            try
            {
                var result = await this.campRepository.GetCampAsync(moniker);
                var output = new
                {
                    count = result.Length,
                    data = this.mapper.Map<CampsModel>(result)
                };
                return Ok(output);

            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database issue");
            }

        }
      
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