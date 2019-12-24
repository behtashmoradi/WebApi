﻿using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public CampsController(ICampRepository campRepository,IMapper mapper,LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
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