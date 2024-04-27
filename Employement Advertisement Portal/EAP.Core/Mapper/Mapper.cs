using AutoMapper;
using EAP.Core.Data;
using EAP.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            CreateMap<EmployeeViewModel, EmployeeDetailsTbl>().ReverseMap();
            CreateMap<UserRoleTbl, SelectListItem>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Role));


            CreateMap<EmployeeDetailsTbl, EmployeeViewModel>()
                .ForMember(dest => dest.EmployeeRole, opt => opt.MapFrom(src =>
                    new List<SelectListItem> {
                                                new SelectListItem {
                                                                      Value = src.Role.RoleId.ToString(),
                                                                      Text = src.Role.Role
                                                                    }
                                              }));

            CreateMap<AdvertisementDetailsTbl, AdvertisementViewModel>()
                .ForMember(dest => dest.AdvertisementCategoryList, opt => opt.MapFrom(src =>
                    new List<SelectListItem> {
                                                new SelectListItem {
                                                                      Value = src.AdvCategory.AdvCategoryId.ToString(),
                                                                      Text = src.AdvCategory.Category
                                                                    }
                                              }))
                .ForMember(dest => dest.EmployeeDetail, opt => opt.MapFrom(src => src.Emp));

            //CreateMap<List<EmployeeDetailsTbl>, List<EmployeeViewModel>>().ReverseMap();
        }
    }
}
