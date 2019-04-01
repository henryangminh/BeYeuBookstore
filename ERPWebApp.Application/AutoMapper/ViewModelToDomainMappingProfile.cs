using AutoMapper;
using BeYeuBookstore.Application.ViewModels.Acc;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeYeuBookstore.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, User>()
                .ConstructUsing(c => new User(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.Avatar, c.Address, c.DateCreated, c.DateModified, c.Status, c.LastupdatedFk,c.Email));


            CreateMap<FunctionViewModel, Function>().ConstructUsing(c => new Function(c.KeyId,c.Name,c.URL,c.ParentId,c.IconCss,
                c.SortOrder));

            CreateMap<PermissionViewModel, Permission>().ConstructUsing(c => new Permission(c.KeyId, c.RoleId, c.FunctionId, c.CanCreate, 
                c.CanRead, c.CanUpdate, c.CanDelete, c.CanConfirm));

            CreateMap<PermissionViewModel, Permission>().ConstructUsing(c => new Permission(c.KeyId, c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete, c.CanConfirm));

            
        }
    }
}
