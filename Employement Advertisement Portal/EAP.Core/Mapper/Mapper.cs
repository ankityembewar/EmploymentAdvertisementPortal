using AutoMapper;
using EAP.Core.Data;
using EAP.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAP.Core.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<LoginViewModel, UserLoginTbl>().ReverseMap();
        }
    }
}
