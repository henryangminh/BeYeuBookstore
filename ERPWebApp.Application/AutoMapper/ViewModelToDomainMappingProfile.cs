using AutoMapper;
using ERPWebApp.Application.ViewModels.Acc;
using ERPWebApp.Application.ViewModels.System;
using ERPWebApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ERPWebApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, User>()
                .ConstructUsing(c => new User(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.Avatar, c.Dob,c.IdNumber,
                c.IdDate, c.PhoneNumber, c.Email,
                c.TaxIdnumber, c.Street, c.Ward, c.District, c.City, c.Country, c.Origin_FK, c.Gender, c.Fax, 
                c.Notes, c.IsCustomer, c.IsVendor, c.IsEmployee, c.IsPersonal, c.Status, 
             c.LastupdatedFk, c.LastupdatedName));


            CreateMap<FunctionViewModel, Function>().ConstructUsing(c => new Function(c.KeyId,c.Name,c.URL,c.ParentId,c.IconCss,
                c.SortOrder));

            CreateMap<PermissionViewModel, Permission>().ConstructUsing(c => new Permission(c.KeyId, c.RoleId, c.FunctionId, c.CanCreate, 
                c.CanRead, c.CanUpdate, c.CanDelete, c.CanConfirm));

            CreateMap<PermissionViewModel, Permission>().ConstructUsing(c => new Permission(c.KeyId, c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete, c.CanConfirm));

            
        }
    }
}
