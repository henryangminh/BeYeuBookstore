﻿using AutoMapper;
using BeYeuBookstore.Application.ViewModels;
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

            CreateMap<BookViewModel, Book>().ConstructUsing(c => new Book(c.KeyId , c.BookTitle, c.Author, c.BookCategoryFK, c.MerchantFK,c.isPaperback,
                c.UnitPrice, c.Length, c.Height, c.Width, c.PageNumber, c.Description,c.Quantity, c.DateCreated, c.DateModified));

            CreateMap<BookCategoryViewModel, BookCategory>().ConstructUsing(c => new BookCategory(c.KeyId,c.BookCategoryName));

            CreateMap<AdvertiseContractViewModel, AdvertiseContract>().ConstructUsing(c => new AdvertiseContract(c.KeyId,c.AdvertisementContentFK,c.DateStart,
                c.DateFinish, c.ContractValue, c.Paid, c.Status, c.DateCreated, c.DateModified));

            CreateMap<AdvertisementContentViewModel, AdvertisementContent>().ConstructUsing(c => new AdvertisementContent(c.KeyId, c.AdvertiserFK,
                c.ImageLink,c.Title,c.Description,c.UrlToAdvertisement,c.Deposite,c.PaidDeposite,c.CensorStatus,c.DateCreated,c.DateModified));

            CreateMap<AdvertisementPositionViewModel, AdvertisementPosition>().ConstructUsing(c => new AdvertisementPosition(c.KeyId, c.PageUrl,
                c.IdOfPosition,c.AdvertisePrice,c.Height,c.Width,c.Status));

            CreateMap<AdvertiserViewModel, Advertiser>().ConstructUsing(c => new Advertiser(c.KeyId, c.UserFK,c.BrandName,c.UrlToBrand,c.DateCreated,c.DateModified));

            CreateMap<CustomerViewModel, Customer>().ConstructUsing(c => new Customer(c.KeyId, c.UserFK, c.DateCreated,c.DateModified ));

            CreateMap<MerchantViewModel, Merchant>().ConstructUsing(c => new Merchant(c.KeyId, c.DirectContactName, c.Hotline, c.MerchantCompanyName,c.Address,
                c.ContactAddress,c.BussinessRegisterId,c.TaxId,c.Website,c.LegalRepresentative,c.MerchantBankingName, c.Bank,c.BankBranch,c.BussinessRegisterLinkImg,
                c.BankAccountNotificationLinkImg,c.TaxRegisterLinkImg,c.Status,c.Scales, c.EstablishDate,c.DateCreated,c.DateModified));

            CreateMap<MerchantContractViewModel, MerchantContract>().ConstructUsing(c => new MerchantContract(c.KeyId,c.ContractLink,c.MerchantFK,c.DateCreated,c.DateModified));

            CreateMap<WebMasterViewModel, WebMaster>().ConstructUsing(c => new WebMaster(c.KeyId,c.UserFK,c.WebMasterTypeFK,c.DateCreated,c.DateModified));

            CreateMap<WebMasterTypeViewModel, WebMasterType>().ConstructUsing(c => new WebMasterType(c.KeyId,c.WebMasterTypeName,c.DateCreated,c.DateModified));

            CreateMap<InvoiceViewModel, Invoice>().ConstructUsing(c => new Invoice(c.KeyId,c.CustomerFK,c.TotalPrice,c.DateCreated,c.DateModified));

            CreateMap<InvoiceDetailViewModel, InvoiceDetail>().ConstructUsing(c => new InvoiceDetail(c.KeyId,c.BookFK,c.Qty,c.UnitPrice,c.SubTotal,c.DateCreated,c.DateModified));

            CreateMap<DeliveryViewModel, Delivery>().ConstructUsing(c => new Delivery(c.KeyId,c.InvoiceFK,c.DeliveryStatus,c.DateCreated,c.DateModified));

        }
    }
}
