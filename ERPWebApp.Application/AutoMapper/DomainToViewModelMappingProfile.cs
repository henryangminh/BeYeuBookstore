using AutoMapper;
using ERPWebApp.Application.ViewModels.Acc;

using ERPWebApp.Application.ViewModels.System;
using ERPWebApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Role, RoleViewModel>();
           // tự động mapper 2 bảng này
           
            CreateMap<Function, FunctionViewModel>();
            CreateMap<Permission, PermissionViewModel>();
           

        }
    }
}
