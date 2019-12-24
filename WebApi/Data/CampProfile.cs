using AutoMapper;
using CoreCodeCamp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            this.CreateMap<Camp, CampsModel>();
            this.CreateMap<Talk, TalkModel>();
            this.CreateMap<CampsModel, Camp>();
        }
    }
}
