using AutoMapper;
using BeYeuBookstore.Application.ViewModels.Acc;

using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.AutoMapper
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
