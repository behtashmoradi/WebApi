using AutoMapper;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/Camps/{moniker}/Talks")]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public TalksController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var result = await this.campRepository.GetTalksByMonikerAsync(moniker);
                return Ok(mapper.Map<TalkModel[]>(result));
            }
            catch (Exception ex)
            {

                return BadRequest();
            }


        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker,int id)
        {
            try
            {
                var talk = await this.campRepository.GetTalkByMonikerAsync(moniker, id);
                if(talk == null)
                {
                    return NotFound($" Could not find talk with moniker={moniker} and id={id}");
                }

                return Ok(this.mapper.Map<TalkModel>(talk));

            }
            catch (Exception ex)
            {

                return BadRequest();
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
