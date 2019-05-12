using AutoMapper;
using BeYeuBookstore.Application.ViewModels;
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

            CreateMap<Book, BookViewModel>();
            CreateMap<BookCategory, BookCategoryViewModel>();
            CreateMap<AdvertiseContract, AdvertiseContractViewModel>();
            CreateMap<AdvertisementContent, AdvertisementContentViewModel>();
            CreateMap<AdvertisementPosition, AdvertisementPositionViewModel>();
            CreateMap<Advertiser, AdvertiserViewModel>();
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Merchant, MerchantViewModel>();
            CreateMap<MerchantContract, MerchantContractViewModel>();
            CreateMap<WebMaster, WebMasterViewModel>();
            CreateMap<WebMasterType, WebMasterTypeViewModel>();
            CreateMap<Invoice, InvoiceViewModel>();
            CreateMap<InvoiceDetail, InvoiceDetailViewModel>();
            CreateMap<Delivery, DeliveryViewModel>();
            CreateMap<RatingDetail, RatingDetailViewModel>();
            CreateMap<BooksIn, BooksInViewModel>();
            CreateMap<BooksOut, BooksOutViewModel>();
            CreateMap<BooksInDetail, BooksInDetailViewModel>();
            CreateMap<BooksOutDetail, BooksOutDetailViewModel>();

        }
    }
}
