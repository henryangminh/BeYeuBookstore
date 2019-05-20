using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Data.EF
{
    public class DbInitializer
    {
        private readonly ERPDbContext _context;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;
        public DbInitializer(ERPDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void SeedStatus()
        {


            if (_context.Functions.Count() == 0)
            {
                _context.Functions.AddRange(new List<Function>()
                {
                      new Function() {KeyId = "UsersItem",Name = "Quản lý thông tin chung",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-address-book"  },

                      new Function() {KeyId = "AdvertiserItem", Name = "Quảng cáo",ParentId = "UsersItem",SortOrder = 1,Status = Status.Active,URL = "/Advertiser",IconCss = "fa-home"  },
                      new Function() {KeyId = "CustomerItem", Name = "Khách hàng",ParentId = "UsersItem",SortOrder = 2,Status = Status.Active,URL = "/Customer",IconCss = "fa-home"  },
                      new Function() {KeyId = "MerchantItem", Name = "Nhà cung cấp",ParentId = "UsersItem",SortOrder =3,Status = Status.Active,URL = "/Merchant",IconCss = "fa-home"  },
                      new Function() {KeyId = "WebMasterItem", Name = "WebMaster",ParentId = "UsersItem",SortOrder = 4,Status = Status.Active,URL = "/WebMaster",IconCss = "fa-home"  },

                      new Function() {KeyId = "PermissionItem", Name = "Phân quyền",ParentId = "UsersItem",SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-divide"  },
                      new Function() {KeyId = "RoleItem", Name = "Nhập liệu nhóm quyền",ParentId = "PermissionItem",SortOrder = 1,Status = Status.Active,URL = "/Role",IconCss = "fa-home"  },
                      new Function() {KeyId = "UserRoleItem",Name = "User vào nhóm quyền",ParentId = "PermissionItem",SortOrder = 2,Status = Status.Active,URL = "/User",IconCss = "fa-home"  },


                      new Function() {KeyId = "ProductItem",Name = "Sản phẩm",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-box"  },

                      new Function() {KeyId = "BookItem",Name = "Sách",ParentId = "ProductItem",SortOrder = 1,Status = Status.Active,URL = "/Book",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "BookCategoryItem",Name = "Loại sách",ParentId = "ProductItem",SortOrder = 2,Status = Status.Active,URL = "/BookCategory",IconCss = "fa-clone"  },
                      new Function() {KeyId = "AdvertisementContentItem",Name = "Nội dung quảng cáo",ParentId = "ProductItem",SortOrder = 3,Status = Status.Active,URL = "/AdvertisementContent",IconCss = "fa-clone"  },
                      new Function() {KeyId = "AdvertisementPositionItem",Name = "Vị trí quảng cáo",ParentId = "ProductItem",SortOrder = 4,Status = Status.Active,URL = "/AdvertisementPosition",IconCss = "fa-clone"  },
                      
                      new Function() {KeyId = "WarehouseItem",Name = "Xuất nhập kho",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-warehouse"  },

                      new Function() {KeyId = "BooksReceiptItem",Name = "Nhập sách",ParentId = "WarehouseItem",SortOrder = 1,Status = Status.Active,URL = "/BooksIn",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "BooksIssueItem",Name = "Xuất sách",ParentId = "WarehouseItem",SortOrder = 2,Status = Status.Active,URL = "/BooksOut",IconCss = "fa-clone"  },
                     
                      new Function() {KeyId = "ReceiptDeliveryItem",Name = "Hóa đơn & Giao hàng",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-receipt"  },

                      new Function() {KeyId = "ReceiptItem",Name = "Hóa đơn",ParentId = "ReceiptDeliveryItem",SortOrder = 1,Status = Status.Active,URL = "/Invoice",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "DeliveryItem",Name = "Phiếu giao hàng",ParentId = "ReceiptDeliveryItem",SortOrder = 2,Status = Status.Active,URL = "/Delivery",IconCss = "fa-clone"  },
                   
                      
                      new Function() {KeyId = "ContractItem",Name = "Hợp đồng",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-file-signature"  },

                      new Function() {KeyId = "MerchantContractItem",Name = "Hợp đồng với nhà cung cấp",ParentId = "ContractItem",SortOrder = 1,Status = Status.Active,URL = "/MerchantContract",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "AdvertiseContractItem",Name = "Hợp đồng quảng cáo",ParentId = "ContractItem",SortOrder = 2,Status = Status.Active,URL = "/AdvertiseContract",IconCss = "fa-clone"  },
   
                      new Function() {KeyId = "TermItem",Name = "Điều khoản dịch vụ, quy trình",ParentId = null,SortOrder = 7,Status = Status.Active,URL = "/",IconCss = "fa-handshake"  },

                      new Function() {KeyId = "MerchantTermItem",Name = "Đối với Nhà cung cấp",ParentId = "TermItem",SortOrder = 3,Status = Status.Active,URL = "/Process/Merchant",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "AdvertiserTermItem",Name = "Đối với bên quảng cáo",ParentId = "TermItem",SortOrder = 2,Status = Status.Active,URL = "/Process/Advertiser",IconCss = "fa-clone"  },
                      new Function() {KeyId = "GeneralTermItem",Name = "Điều khoản và dịch vụ",ParentId = "TermItem",SortOrder = 1,Status = Status.Active,URL = "/Process/GeneralTerm",IconCss = "fa-clone"  },

                      new Function() {KeyId = "StatisticsItem",Name = "Thống kê",ParentId = null,SortOrder = 6,Status = Status.Active,URL = "/",IconCss = "fa-chart-line"  },

                      new Function() {KeyId = "AdStatisticsItem",Name = "Quảng cáo", ParentId = "StatisticsItem",SortOrder = 1,Status = Status.Active,URL = "/AdvertiseContract/Statistic",IconCss = "fa-chevron-down"  },

                       new Function() {KeyId = "AboutUsItem",Name = "Về chúng tôi",ParentId = null,SortOrder = 8,Status = Status.Active,URL = "/",IconCss = "fa-address-card"  },

                      new Function() {KeyId = "AddressItem",Name = "Địa chỉ", ParentId = "AboutUsItem",SortOrder = 1,Status = Status.Active,URL = "/Process/Address",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "BankingInfoItem",Name = "Cách thức thanh toán", ParentId = "AboutUsItem",SortOrder = 2,Status = Status.Active,URL = "/Process/BankingInfo",IconCss = "fa-chevron-down"  },
                 });
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

        }
        public async Task Seed()
        {
            //tao cac role
            #region AddRole
            if (!_roleManager.Roles.Any())
            {
                var _item1 = new Role()
                {
                    Name = "Khách hàng",
                    NormalizedName = "Khách hàng",
                    Description = "Khách mua sách"
                };
                await _roleManager.CreateAsync(_item1);
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Nhà cung cấp",
                    NormalizedName = "Nhà bán sách",
                    Description = "Nhà cung cấp sách sỉ và lẻ"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Quảng cáo",
                    NormalizedName = "Người quảng cáo",
                    Description = "Bên đăng ký quảng cáo"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster",
                    NormalizedName = "Full quyền WebMaster",
                    Description = "Thằng full quyền"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Kế toán",
                    NormalizedName = "Kế toán_WebMaster",
                    Description = "Thằng kế toán"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "test",
                    NormalizedName = "nhóm test ",
                    Description = "full chức năng tiện cho quá trình test. sẽ bị xóa trước khi chạy thực tế."
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Kiểm duyệt Quảng cáo",
                    NormalizedName = "Kiểm duyệt Quảng cáo_WebMaster",
                    Description = "Thằng kiểm duyệt quảng cáo"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Hành chính kinh doanh",
                    NormalizedName = "Hành chính kinh doanh_WebMaster",
                    Description = "Thằng Admin quản lý hợp đồng"
                });
             
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Kiểm duyệt Nhà cung cấp",
                    NormalizedName = "Kiểm duyệt Nhà cung cấp_WebMaster",
                    Description = "Thằng kiểm duyệt Merchant"
                });
                

            }
            #endregion
            if (_context.AdvertisementPositions.Count() == 0)
            {
                _context.AdvertisementPositions.AddRange(new List<AdvertisementPosition>()
                {
                    new AdvertisementPosition(){PageUrl="/beyeubookstore", IdOfPosition="AdPosition1", Title="Đầu trang chủ",AdvertisePrice=10000000,Height=223,Width=263,Status=Status.Active},
                    new AdvertisementPosition(){PageUrl="/beyeubookstore", IdOfPosition="AdPosition2", Title="Giữa trang chủ phải",AdvertisePrice=9000000,Height=185,Width=460,Status=Status.Active},
                    new AdvertisementPosition(){PageUrl="/beyeubookstore", IdOfPosition="AdPosition3", Title="Giữa trang chủ",AdvertisePrice=9000000,Height=183,Width=385,Status=Status.Active},
                    new AdvertisementPosition(){PageUrl="/beyeubookstore", IdOfPosition="AdPosition4", Title="Giữa trang chủ trái",AdvertisePrice=80000000,Height=396,Width=263,Status=Status.Active},
                    new AdvertisementPosition(){PageUrl="/beyeubookstore", IdOfPosition="AdPosition5", Title="Cuối trang chủ",AdvertisePrice=50000000,Height=138,Width=1140,Status=Status.Active},
                    //
                    new AdvertisementPosition(){PageUrl="/BeyeuBookstore/Shopping", IdOfPosition="AdPosition6", Title="Đầu trang mua hàng",AdvertisePrice=10000000,Height=848,Width=333,Status=Status.Active},
                    new AdvertisementPosition(){PageUrl="/BeyeuBookstore/Shopping", IdOfPosition="AdPosition7", Title="Giữa trang mua hàng trái",AdvertisePrice=5000000,Height=263,Width=396,Status=Status.Active},
                    new AdvertisementPosition(){PageUrl="/BeyeuBookstore/BookDetail", IdOfPosition="AdPosition8", Title="Trên góc phải trang chi tiết sách",AdvertisePrice=500000,Height=277,Width=154,Status=Status.Active},
                

                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };
            if (_context.BookCategorys.Count() == 0)
            {
                _context.BookCategorys.AddRange(new List<BookCategory>()
                {
                    new BookCategory(){Status=Status.Active,BookCategoryName="Kinh dị"}, //1
                    new BookCategory(){Status=Status.Active,BookCategoryName="Học thuật"}, //2
                    new BookCategory(){Status=Status.Active,BookCategoryName="Truyện"}, //3
                    new BookCategory(){Status=Status.Active,BookCategoryName="Ngoại ngữ"}, //5
                    new BookCategory(){Status=Status.Active,BookCategoryName="Truyện tranh"}, //6
                    new BookCategory(){Status=Status.Active,BookCategoryName="Kỹ năng sống"}, //7
                    new BookCategory(){Status=Status.Active,BookCategoryName="Phát triển bản thân"}, //8
                    new BookCategory(){Status=Status.Active,BookCategoryName="Truyền cảm hứng"}, //9
                    new BookCategory(){Status=Status.Active,BookCategoryName="Kinh tế"}, //10
                    new BookCategory(){Status=Status.Active,BookCategoryName="Thiếu nhi"}, //11
                    new BookCategory(){Status=Status.Active,BookCategoryName="Nuôi dạy con"}, //12
                    new BookCategory(){Status=Status.Active,BookCategoryName="Tiểu thuyết"}, //13
                    new BookCategory(){Status=Status.Active,BookCategoryName="Tản văn"}, //14
                    new BookCategory(){Status=Status.Active,BookCategoryName="Loại khác"}, //4

                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.WebMasterTypes.Count() == 0)
            {
                _context.WebMasterTypes.AddRange(new List<WebMasterType>()
                {
                    new WebMasterType(){WebMasterTypeName="WebMaster"},
                    new WebMasterType(){WebMasterTypeName="Kế toán"},
                    new WebMasterType(){WebMasterTypeName="Kiểm duyệt quảng cáo"},
                    new WebMasterType(){WebMasterTypeName="Kiểm duyệt nhà cung cấp"},
                    new WebMasterType(){WebMasterTypeName="Hành chính kinh doanh"},

                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };
            if (_context.UserTypes.Count() == 0)
            {
                _context.UserTypes.AddRange(new List<UserType>()
                {
                    new UserType(){UserTypeName="Nhà cung cấp"},
                    new UserType(){UserTypeName="Quảng cáo"},
                    new UserType(){UserTypeName="Khách hàng"},
                    new UserType(){UserTypeName="Webmaster"},
                                
                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };
            #region AddUser
            //tao user Webmaster full quyền (pass word phai lon hon 6 ktu
            if (_userManager.Users.Count() == 0)
            {

                var result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore114@gmail.com",
                    FullName = "Bé Yêu",
                    Email = "hippore114@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    EmailConfirmed = true, //Webmaster
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore114@gmail.com"); // tim user admin
                    _userManager.AddToRoleAsync(user, "test").Wait(); // add vao role test :">
                    _context.WebMasters.Add(new WebMaster() { UserFK = user.Id, WebMasterTypeFK = Const_WebmasterType.Webmaster });
                }
                //tạo user kế toán bên webmaster
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "accountant@gmail.com",
                    FullName = "Bé Yêu Accountant",
                    Email = "hippore115@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("accountant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Kế toán"); // add vao role accountant
                    _context.WebMasters.Add(new WebMaster() { UserFK = user.Id, WebMasterTypeFK = Const_WebmasterType.Accountant });
                }

                //tạo user kiểm duyệt quảng cáo
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "adcensor@gmail.com",
                    FullName = "Bé Yêu AdCensor",
                    Email = "hippore116@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("adcensor@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Kiểm duyệt Quảng cáo"); // add vao role 
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.AdCensor, UserFK = user.Id });
                }

                //tạo user Admin bên webmaster
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "saleadmin@gmail.com",
                    FullName = "Bé Yêu Admin",
                    Email = "hippore117@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("saleadmin@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Hành chính kinh doanh"); // add vao role 
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.Admin, UserFK = user.Id });
                }

                //tạo user kiểm duyệt merchant bên webmaster
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "merchantcensor@gmail.com",
                    FullName = "Bé Yêu MerchantCensor",
                    Email = "hippore118@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("merchantcensor@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Kiểm duyệt Nhà cung cấp"); // add vao role 
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.MerchantCensor, UserFK = user.Id });
                }


                //tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "cucmo_cuaca@gmail.com",
                    FullName = "Mỡ Đại gia",
                    Email = "cucmo_cuaca@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "09000000002",
                    Address = "Chung cư HaDo Q.10",
                    Gender = Gender.Male,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("cucmo_cuaca@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }

                //tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "conca_cuamo@gmail.com",
                    FullName = "Cá Mập Heo",
                    Email = "conca_cuamo@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0900000001",
                    Address = "289 Bình Đông F14 Q8",
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("conca_cuamo@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }
                //tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "utbibebong@gmail.com",
                    FullName = "Út Bi",
                    Email = "utbibebong@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0989282912",
                    Address = "23 Nguyễn Đình Chiểu F12 Q3 ",
                    Gender = Gender.Male,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("utbibebong@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "banhmi@gmail.com",
                    FullName = "Bánh mì ciute",
                    Email = "banhmi@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0922229931",
                    Address = "",
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("banhmi@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "captainmarvel@gmail.com",
                    FullName = "Captain Marvel",
                    Email = "captainmarvel@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    Status = Status.Active,
                    EmailConfirmed = true,
                    PhoneNumber = "0972822223",
                    Address = "12 Ba Tháng Hai F2 Q10",
                    Gender = Gender.Male,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("captainmarvel@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "vinhhuynh@gmail.com",
                    FullName = "Huỳnh Mỹ Vinh",
                    Email = "vinhhuynh@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0906736222",
                    Address = "289 Mai Chí Thọ F3 Q2",
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("vinhhuynh@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "huyenngoc@gmail.com",
                    FullName = "Ngọc Huyền",
                    Email = "huyenngoc@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0978782493",
                    Address = "34 Nguyễn Hữu Cảnh F5 Q4",
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("huyenngoc@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hachibich@gmail.com",
                    FullName = "Chi Hà",
                    Email = "hachibich@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0978373382",
                    Address = "24 Trần Văn Đang F3 Q3",
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hachibich@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "duyhuynh@gmail.com",
                    FullName = "Huỳnh Hữu Duy",
                    Email = "duyhuynh@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0900000002",
                    Address = "29 Trần Tuấn Khải  F2 Q1",
                    Gender = Gender.Male,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("duyhuynh@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                ////tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "vanlamle@gmail.com",
                    FullName = "Lê Văn Lâm",
                    Email = "vanlamle@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    PhoneNumber = "0900000021",
                    Address = "289 Thành Thái F12 Q2",
                    Gender = Gender.Male,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("vanlamle@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Khách hàng"); // add vao role 
                    _context.Customers.Add(new Customer() { UserFK = user.Id });
                }


                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "khang_merchant@gmail.com",
                    FullName = "Khang Merchant",
                    Email = "khang_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("khang_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Đào Bảo Khang",
                        Hotline = "19001112",
                        MerchantCompanyName = "Khang Book",
                        Address = "01 Nguyễn Khang Dương, P.5, Q.5",
                        ContactAddress = "01 Nguyễn Khang Dương, P.5, Q.5",
                        BussinessRegisterId = "45342",
                        TaxId = "1",
                        Website = "khang.sgu.edu.vn",
                        LegalRepresentative = "Đào Bảo Khang",
                        MerchantBankingName = "Đào Bảo Khang",
                        Bank = "ACB",
                        BankBranch = "An Đông",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2018-05-03")
                    });
                }

                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "tram_merchant@gmail.com",
                    FullName = "Trâm Merchant",
                    Email = "tram_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    Status = Status.Active,
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("tram_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Hà Thị Bích Trâm",
                        Hotline = "12234332",
                        MerchantCompanyName = "Trâm Store",
                        Address = "3 Trần Quang Diệu, P.4, Q.3",
                        ContactAddress = "3 Trần Quang Diệu, P.4, Q.3",
                        BussinessRegisterId = "896895",
                        TaxId = "2",
                        Website = "tram.vn",
                        LegalRepresentative = "Hà Thị Bích Trâm",
                        MerchantBankingName = "Hà Thị Bích Trâm",
                        Bank = "Agribank",
                        BankBranch = "An Lộc",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2016-04-01")
                    });
                }
                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "chim_merchant@gmail.com",
                    FullName = "Chimte Merchant",
                    Email = "chim_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Female
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("chim_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Nguyễn Thị Chim",
                        Hotline = "19001119",
                        MerchantCompanyName = "Chim shop",
                        Address = "178 Nguyễn Văn Lượng, P.6, Q.6",
                        ContactAddress = "178 Nguyễn Văn Lượng, P.6, Q.6",
                        BussinessRegisterId = "18895",
                        TaxId = "3",
                        Website = "chimte.vn",
                        LegalRepresentative = "Nguyễn Thị Chim",
                        MerchantBankingName = "Nguyễn Thị Chim",
                        Bank = "MB",
                        BankBranch = "Lê Văn Sỹ",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2017-03-02")
                    });
                }

                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "huy_merchant@gmail.com",
                    FullName = "Huy Merchant",
                    Email = "huy_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("huy_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Huỳnh Tấn Huy",
                        Hotline = "19001122",
                        MerchantCompanyName = "Huy Huỳnh",
                        Address = "177/1 Trần Tuấn Khải, P.5, Q.5",
                        ContactAddress = "177/1 Trần Tuấn Khải, P.5, Q.5",
                        BussinessRegisterId = "456788",
                        TaxId = "4",
                        Website = "huyhuynh.com.vn",
                        LegalRepresentative = "Huỳnh Tấn Huy",
                        MerchantBankingName = "Huỳnh Tấn Huy",
                        Bank = "Đông Á",
                        BankBranch = "Tân Phú",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2016-08-10"),
                    });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hien_merchant@gmail.com",
                    FullName = "Hiền Merchant",
                    Email = "hien_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Female
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hien_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Nguyễn Thị Bích Hiền",
                        Hotline = "19900123",
                        MerchantCompanyName = "Phương Nam Book",
                        Address = "22 Nguyễn Văn Linh, P.1, Q.7",
                        ContactAddress = "22 Nguyễn Văn Linh, P.1, Q.7",
                        BussinessRegisterId = "11234",
                        TaxId = "5",
                        Website = "phuongnam.net",
                        LegalRepresentative = "Nguyễn Thị Bích Hiền",
                        MerchantBankingName = "Nguyễn Thị Bích Hiền",
                        Bank = "Shinhan",
                        BankBranch = "Nguyễn Thị Thập",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2011-02-04")
                    });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "nhan_merchant@gmail.com",
                    FullName = "Nhân Merchant",
                    Email = "nhan_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("nhan_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Trương Vĩ Nhân",
                        Hotline = "19001219",
                        MerchantCompanyName = "Nhân Company",
                        Address = "19 Nguyễn Thượng Hiền, P.4, Q.3",
                        ContactAddress = "19 Nguyễn Thượng Hiền, P.4, Q.3",
                        BussinessRegisterId = "18892",
                        TaxId = "6",
                        Website = "nhancompany.com.vn",
                        LegalRepresentative = "Trương Vĩ Nhân",
                        MerchantBankingName = "Trương Vĩ Nhân",
                        Bank = "Agribank",
                        BankBranch = "An Dương Vương",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2013-12-12")
                    });
                }
                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "minh_merchant@gmail.com",
                    FullName = "Minh Merchant",
                    Email = "minh_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("minh_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Huỳnh Hà Khánh Minh",
                        Hotline = "19001112",
                        MerchantCompanyName = "Alpha Books ",
                        Address = "99 Nguyễn Văn Trỗi, P.2, Q.Tân Phú",
                        ContactAddress = "99 Nguyễn Văn Trỗi, P.2, Q.Tân Phú",
                        BussinessRegisterId = "11282",
                        TaxId = "7",
                        Website = "alphabooks.com",
                        LegalRepresentative = "Huỳnh Hà Khánh Minh",
                        MerchantBankingName = "Huỳnh Hà Khánh Minh",
                        Bank = "Viettin Bank",
                        BankBranch = "Trương Định",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2009-11-22")
                    });
                }

                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "vu_merchant@gmail.com",
                    FullName = "Vũ Merchant",
                    Email = "vu_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("vu_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Lê Tấn Vũ",
                        Hotline = "10002233",
                        MerchantCompanyName = "First News",
                        Address = "31 Lê Văn Lương, P.3, Q.7",
                        ContactAddress = "31 Lê Văn Lương, P.3, Q.7",
                        BussinessRegisterId = "12234",
                        TaxId = "8",
                        Website = "firstnews.edu.vn",
                        LegalRepresentative = "Lê Tấn Vũ",
                        MerchantBankingName = "Lê Tấn Vũ",
                        Bank = "Vietcombank",
                        BankBranch = "Đinh Công Tráng",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2011-12-04")
                    });
                }

                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "linh_merchant@gmail.com",
                    FullName = "Linh Merchant",
                    Email = "linh_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Female
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("linh_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Huỳnh Hà Khánh Linh",
                        Hotline = "19009892",
                        MerchantCompanyName = "Linh New Style",
                        Address = "16 Nguyễn Biểu, P.5, Q.5",
                        ContactAddress = "16 Nguyễn Biểu, P.5, Q.5",
                        BussinessRegisterId = "13242",
                        TaxId = "9",
                        Website = "linhnewstyle.net",
                        LegalRepresentative = "Huỳnh Hà Khánh Linh",
                        MerchantBankingName = "Huỳnh Hà Khánh Linh",
                        Bank = "BIDV",
                        BankBranch = "Nguyễn Văn Cừ",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2013-12-02")
                    });
                }

                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "trung_merchant@gmail.com",
                    FullName = "Trung Merchant",
                    Email = "trung_merchant@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Merchant,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("trung_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Nhà cung cấp"); // add vao role 
                    _context.Merchants.Add(new Merchant()
                    {
                        UserFK = user.Id,
                        DirectContactName = "Lê Trọng Trung",
                        Hotline = "0989345512",
                        MerchantCompanyName = "Trung Lê",
                        Address = "02 Lê Đức Thọ, P.1, Q.Gò Vấp",
                        ContactAddress = "02 Lê Đức Thọ, P.1, Q.Gò Vấp",
                        BussinessRegisterId = "45333",
                        TaxId = "10",
                        Website = "trungle.edu.com.vn",
                        LegalRepresentative = "Lê Trọng Trung",
                        MerchantBankingName = "Lê Trọng Trung",
                        Bank = "SCB",
                        BankBranch = "Lê Lai",
                        Status = Status.Active,
                        Scales = Scale.Large,
                        EstablishDate = DateTime.Parse("2017-12-18")
                    });
                }
                //tạo user advertiser
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "butkimlong@gmail.com",
                    FullName = "Anh Long",
                    Email = "butkimlong@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser, //Webmaster
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("butkimlong@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Bút Kim Long", UrlToBrand = "butkimlong.com.vn", Status = Status.Active });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "viethung@gmail.com",
                    FullName = "Chị Hưng",
                    Email = "viethung@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("viethung@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Bao Sách Viết Hưng", UrlToBrand = "bsviethung.com.vn", Status = Status.InActive });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "phongzu@gmail.com",
                    FullName = "Anh Zũ",
                    Email = "phongzu@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("phongzu@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Phong Vũ", UrlToBrand = "phongvu.com.vn", Status = Status.Active });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "tiki@gmail.com",
                    FullName = "Anh Ki",
                    Email = "tiki@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser,
                    EmailConfirmed = true,
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("tiki@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Quảng cáo"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Tiki", UrlToBrand = "tiki.vn", Status = Status.Active });
                }
                #endregion
            }
            if (_context.Books.Count() == 0)
            {
                //_context.Books.AddRange(new List<Book>()
                var books = new List<Book>
                {
                    new Book(){KeyId = 1, Status=Status.Active, BookTitle="Đắc Nhân Tâm",Author="Dale Carnegie",BookCategoryFK=4,MerchantFK=1,isPaperback=false,UnitPrice=45000, Length=20, Height=2, Width=14,PageNumber=320,Description="Đắc Nhân Tâm nằm trong top bán chạy nhất thế giới",Quantity=50, Img="/images/merchant/Khang Book/books/1.jpg"},
                    new Book(){KeyId = 2, Status=Status.Active, BookTitle="Đời ngắn đừng ngủ dài",Author="Robins Sharma",BookCategoryFK=4,MerchantFK=2,isPaperback=true,UnitPrice=46000, Length=21, Height=2, Width=13,PageNumber=239,Description="Bằng những lời chia sẻ thật ngắn gọn, dễ hiểu về những trải nghiệm và suy ngẫm trong đời, Robin Sharma tiếp tục phong cách viết của ông từ cuốn sách Điều vĩ đại đời thường để mang đến cho độc giả những bài viết như lời tâm sự, vừa chân thành vừa sâu sắc.",Quantity=50,Img="/images/merchant/Trâm Store/books/2.jpg"},
                    new Book(){KeyId = 3, Status=Status.Active, BookTitle="Nếu tôi biết khi còn 20",Author="Tina Seelig",BookCategoryFK=4,MerchantFK=1,isPaperback=true,UnitPrice=44000, Length=21, Height=2, Width=14,PageNumber=252,Description="Thông qua quyển sách, tác giả còn muốn các độc giả, đặc biệt là độc giả trẻ, sẽ được  bị đủ sự tự tin để biến căng thẳng thành sự hào hứng, biến thử thách thành các cơ hội, và cứ sau mỗi lần vấp ngã lại đứng lên trưởng thành hơn.",Quantity=50,Img="/images/merchant/Khang Book/books/3.jpg"},
                    new Book(){KeyId = 4, Status=Status.Active, BookTitle="Café sáng cùng Tony",Author="Tony",BookCategoryFK=4,MerchantFK=1,isPaperback=false,UnitPrice=55000, Length=21, Height=3, Width=15,PageNumber=323,Description="Chúng tôi tin rằng những người trẻ tuổi luôn mang trong mình khát khao vươn lên và tấm lòng hướng thiện, và có nhiều cách để động viên họ biến điều đó thành hành động. Nếu như tập sách nhỏ này có thể khơi gợi trong lòng bạn đọc trẻ một cảm hứng tốt đẹp, như tách cà phê thơm vào đầu ngày nắng mới, thì đó là niềm vui lớn của tác giả Tony Buổi Sáng.",Quantity=46,Img="/images/merchant/Khang Book/books/4.jpg"},
                    new Book(){KeyId = 5, Status=Status.Active, BookTitle="Tuổi trẻ đáng giá bao nhiêu ?",Author="Chi",BookCategoryFK=4,MerchantFK=2,isPaperback=true,UnitPrice=65000, Length=21, Height=2, Width=16,PageNumber=234,Description="Hãy đọc sách, nếu bạn đọc sách một cách bền bỉ, sẽ đến lúc bạn bị thôi thúc không ngừng bởi ý muốn viết nên cuốn sách của riêng mình. Nếu tôi còn ở tuổi đôi mươi, hẳn là tôi sẽ đọc Tuổi trẻ đáng giá bao nhiêu? nhiều hơn một lần.",Quantity=45,Img="/images/merchant/Trâm Store/books/5.jpg"},
                    new Book(){KeyId = 6, Status=Status.Active, BookTitle="Tokyo Hoàng Đạo Án",Author="Soji Shimada",BookCategoryFK=1,MerchantFK=1,isPaperback=false,UnitPrice=45000, Length=21, Height=2, Width=17,PageNumber=332,Description="Tiểu thuyết trinh thám kinh dị",Quantity=47,Img="/images/merchant/Khang Book/books/6.jpg"},
                    new Book(){KeyId = 7, Status=Status.Active, BookTitle="Athoner",Author="Yukito Ayatsuji",BookCategoryFK=1,MerchantFK=2,isPaperback=true,UnitPrice=40000, Length=21, Height=2, Width=18,PageNumber=231,Description="Hai mươi sáu năm về trước có một học sinh hoàn thiện hoàn mĩ. Rất đẹp, rất giỏi, rất hòa đồng, ai cũng yêu quý, những lời tán tụng người ấy được truyền mãi qua các thế hệ học sinh của trường. Nhưng tên đầy đủ là gì, chết đi thế nào, thậm chí giới tính ra sao lại không một ai hay biết. Người ta chỉ rỉ tai nhau, bỗng nhiên giữa năm bạn ấy chết, trường lớp không sao thoát được nỗi buồn nhớ thương, họ bèn cư xử như thể học sinh này còn tồn tại. Bàn ghế để nguyên, bạn cùng lớp vẫn giả vờ nói chuyện với người đã khuất.",Quantity=34,Img="/images/merchant/Trâm Store/books/7.jpg"},
                    new Book(){KeyId = 8, Status=Status.Active, BookTitle="Kỹ Thuật Làm Bánh Ngọt",Author="Thảo Đoàn",BookCategoryFK=2,MerchantFK=1,isPaperback=true,UnitPrice=20000, Length=32, Height=450, Width=19,PageNumber=332,Description="Cuốn Sách Cho Người Bắt Đầu Học Làm Bánh là những kiến thức cơ bản nhất mà một người bắt đầu làm quen với bột, lò nướng cần có: Gồm các kiến thức chung về nguyên liệu, dụng cụ làm bánh, các kỹ năng làm bánh cơ bản. Cuốn sách còn cung cấp các công thức làm bánh Cookies. Muffin, Cream Cake, Tart đơn giản, dễ làm nhưng cũng rất hấp dẫn, ngon miệng.",Quantity=43,Img="/images/merchant/Khang Book/books/8.jpg"},
                    new Book(){KeyId = 9, Status=Status.Active, BookTitle="150 Thuật Ngữ Văn Học",Author="Lại Nguyên Ân",BookCategoryFK=2,MerchantFK=2,isPaperback=false,UnitPrice=10000, Length=7, Height=4, Width=20,PageNumber=588,Description="150 Thuật Ngữ Văn Học - Với khoảng trên 150 mục từ THUẬT NGỮ VĂN HỌC, cuốnsách nhỏ này chưa thể bao quát toàn bộ các bình diện, cấp độ, sắc thái của một loại hiện tượng văn hoá nhân bản đặc sắc và vô cùng phong phú là văn học và các chuyên ngành nghiên cứu nó…",Quantity=42,Img="/images/merchant/Trâm Store/books/9.png"},
                    new Book(){KeyId = 10, Status=Status.Active, BookTitle="Truyện ngụ ngôn Êdốp",Author="êdop",BookCategoryFK=3,MerchantFK=1,isPaperback=true,UnitPrice=36000, Length=13, Height=1, Width=21,PageNumber=46,Description="Truyện tranh thiếu nhi",Quantity=46,Img="/images/merchant/Khang Book/books/10.jpg"},
                    new Book(){KeyId = 11, Status=Status.Active, BookTitle="Alixơ ở xứ sở diệu kỳ",Author="Hàn Thuyên",BookCategoryFK=3,MerchantFK=2,isPaperback=false,UnitPrice=120000, Length=21, Height=2, Width=22,PageNumber=132,Description="Truyện tranh thiếu nhi",Quantity=46,Img="/images/merchant/Trâm Store/books/11.jpg"},
                    new Book(){KeyId = 12, Status=Status.Active, BookTitle="Kỳ án ánh trăng",Author="Quỷ Cổ Nữ",BookCategoryFK=1,MerchantFK=1,isPaperback=false,UnitPrice=165000, Length=24, Height=3, Width=15,PageNumber=528,Description="Người tới số năm nay là Diệp Hinh. Không cam chịu lời nguyền trái ngang này, cô tìm gặp người duy nhất thoát nạn trong mười sáu năm qua để học hỏi kinh nghiệm, người ta liền trèo lên bậu cửa sổ ngay trước mắt cô, lao đầu xuống sân và chết.",Quantity=10, Img="/images/merchant/Khang Book/books/12.jpg"},
                    new Book(){KeyId = 13, Status=Status.Active, BookTitle="Tự học tiếng Đức",Author="Paul Coggle & Heiner Schenke",BookCategoryFK=5,MerchantFK=2,isPaperback=false,UnitPrice=165000, Length=14, Height=2, Width=20,PageNumber=318,Description="Tự Học Tiếng Đức Cho Người Mới Bắt Đầu là cuốn giáo trình tự học tiếng Đức hoàn chỉnh nhất và được biên soạn công phu nhằm giúp người học nâng cao bốn kỹ năng của mình: Nghe, Nói, Đọc, Viết.",Quantity=10, Img="/images/merchant/Trâm Store/books/13.jpg"},
                    new Book(){KeyId = 14, Status=Status.Active, BookTitle="Thám tử lừng danh Conan (Tập 93)",Author="Aoyama Gosho",BookCategoryFK=6,MerchantFK=1,isPaperback=false,UnitPrice=18000, Length=18, Height=1, Width=11,PageNumber=192,Description="Thi thể đã biến đi đâu!? “Vụ án xác chết biến mất trong bể bơi” sẽ được làm sáng tỏ! Bên cạnh đó, bóng dáng “Rum”, nhân vật quyền lực thứ 2 của tổ chức Áo Đen sẽ theo sát Conan và Haibara!?",Quantity=12, Img="/images/merchant/Khang Book/books/14.jpg"},
                    new Book(){KeyId = 15, Status=Status.Active, BookTitle="Khéo ăn nói sẽ có được thiên hạ",Author="Trác Nhã",BookCategoryFK=7,MerchantFK=2,isPaperback=false,UnitPrice=145000, Length=16, Height=3, Width=23,PageNumber=403,Description="Như thế nào mới gọi là biết cách ăn nói? Nói năng lưu loát, không ấp úng có được gọi là biết cách nói chuyện không? Nói ngắn gọn, nói ít nhưng ý nghĩa thâm sâu có được gọi là biết cách nói chuyện không?",Quantity=12, Img="/images/merchant/Trâm Store/books/15.jpg"},
                    new Book(){KeyId = 16, Status=Status.Active, BookTitle="7 thói quen hiệu quả",Author="Stephen R.Covey",BookCategoryFK=8,MerchantFK=1,isPaperback=false,UnitPrice=18000, Length=15, Height=3, Width=23,PageNumber=476,Description="Tác phẩm 7 Thói quen Hiệu quả / 7 Habits for Highly Effective People đã ra đời hơn 25 năm, được biết đến là cuốn sách quản trị (quản trị bản thân và quản trị tổ chức) bán chạy nhất mọi thời đại với hơn 30 triệu bản bán ra trên toàn thế giới và được dịch sang 40 ngôn ngữ. Sách có mặt tại thị trường Việt Nam hơn 10 năm nay dưới cái tên 7 Thói quen để Thành đạt.", Quantity=0, Img="/images/merchant/Khang Book/books/16.jpg"},
                    new Book(){KeyId = 17, Status=Status.Active, BookTitle="Phép màu để trở thành chính mình",Author="Nhan Húc Quân",BookCategoryFK=9,MerchantFK=2,isPaperback=false,UnitPrice=99000, Length=13, Height=2, Width=20,PageNumber=260,Description="Cuốn sách Phép màu để trở thành chính mình chính là những đúc kết thành công qua những nỗ lực vượt khó của tác giả, từ khi bắt đầu lập nghiệp cho đến bây giờ - là Tổng Giám đốc New Toyo Việt Nam. Là phụ nữ, con đường lập nghiệp không hề đơn giản, và càng không hề đơn giản hơn đối với bản thân tác giả - là một phụ nữ làm việc trong một môi trường mà chỉ có sức lực người đàn ông nhiều khi mới đảm đương nổi.", Quantity=2, Img="/images/merchant/Trâm Store/books/17.jpg"},
                    new Book(){KeyId = 18, Status=Status.Active, BookTitle="Tiếp thị 4.0",Author="Philip Kopler",BookCategoryFK=10,MerchantFK=1,isPaperback=false,UnitPrice=10000, Length=13, Height=2, Width=20,PageNumber=260,Description="Quyển cẩm nang vô cùng cần thiết cho những người làm tiếp thị trong thời đại số. Được viết bởi cha đẻ ngành tiếp thị hiện đại, cùng hai đồng tác giả là lãnh đạo của công ty MarkPlus, quyển sách sẽ giúp bạn lèo lái thế giới không ngừng kết nối và khách hàng không ngừng thay đổi để có được nhiều khách hàng hơn, xây dựng thương hiệu hiệu quả hơn, và cuối cùng kinh doanh thành công hơn. Ngày nay khách hàng không có nhiều thời gian và sự chú ý dành cho thương hiệu của bạn – và họ còn bị bao quanh bởi vô số các chọn lựa.", Quantity=9, Img="/images/merchant/Khang Book/books/18.jpg"},
                    new Book(){KeyId = 19, Status=Status.Active, BookTitle="Phép lạ",Author="Tạ Duy Anh",BookCategoryFK=11,MerchantFK=2,isPaperback=false,UnitPrice=37000, Length=13, Height=1, Width=20,PageNumber=120,Description="Truyện vừa Phép lạ kể về chuyến nghỉ hè của một cô bé thành phố yếu ớt bị bệnh tim bẩm sinh, từ nhỏ vốn luôn phải nép mình trong những quy tắc nghiêm ngặt về môi trường sống, hạn chế tiếp xúc với bên ngoài ồn ào phức tạp.", Quantity=29, Img="/images/merchant/Trâm Store/books/19.jpg"},
                    new Book(){KeyId = 20, Status=Status.Active, BookTitle="Nuôi con không phải là cuộc chiến",Author="Mẹ Ong Bông",BookCategoryFK=12,MerchantFK=1,isPaperback=false,UnitPrice=79000, Length=15, Height=2, Width=24,PageNumber=316,Description="Nuôi con không phải là cuộc chiến là cuốn sách của tác giả Việt giúp mẹ hiểu hơn về chu kỳ sinh học của con để có thể nuôi dưỡng bé được tốt hơn thông qua những phương pháp đã được cả thế giới công nhận.", Quantity=29, Img="/images/merchant/Khang Book/books/20.jpg"},
                    new Book(){KeyId = 21, Status=Status.Active, BookTitle="Tắt đèn",Author="Ngô Tất Tố",BookCategoryFK=13,MerchantFK=2,isPaperback=false,UnitPrice=45000, Length=13, Height=2, Width=20,PageNumber=264,Description="Tắt đèn của nhà văn Ngô Tất Tố phản ánh rất chân thực cuộc sống khốn khổ của tầng lớp nông dân Việt Nam đầu thế kỷ XX dưới ách đô hộ của thực dân Pháp. Tác phẩm xoay quanh nhân vật chị Dậu và gia đình – một điển hình của cuộc sống bần cùng hóa sưu cao thuế nặng mà chế độ thực dân áp đặt lên xã hội Việt Nam", Quantity=29, Img="/images/merchant/Trâm Store/books/21.jpg"},
                    new Book(){KeyId = 22, Status=Status.Active, BookTitle="Thương mấy cũng là người dưng",Author="Anh Khang",BookCategoryFK=14,MerchantFK=1,isPaperback=false,UnitPrice=94000, Length=12, Height=2, Width=20,PageNumber=264,Description="Thương mấy cũng là người dưng được tác giả viết “trong những ngày tủi thân nhất của thanh xuân mình. Cái quãng đời đã bước qua đủ nhiều cuộc yêu để tự thấy mình không còn dư dả tuổi trẻ, niềm tin và tình thương để phung phí; thành ra chỉ muốn nắm thật chặt bàn tay của người bên cạnh, bình bình đạm đạm đi đến cuối.", Quantity=20, Img="/images/merchant/Khang Book/books/22.jpg"},
                    new Book(){KeyId = 23, Status=Status.Active, BookTitle="Doreamon Kỉ niệm (Tập 3)",Author="Fujiko.F.Fujio",BookCategoryFK=6,MerchantFK=2,isPaperback=false,UnitPrice=16000, Length=11, Height=1, Width=17,PageNumber=196,Description="Những câu chuyện về chú mèo máy thông minh Doraemon và nhóm bạn Nobita, Shizuka, Jaian, Dekisugi sẽ đưa chúng ta bước vào thế giới hồn nhiên, trong sáng, đầy ắp tiếng cười với một kho bảo bối kì diệu - những bảo bối biến giấc mơ của chúng ta thành sự thật.", Quantity=20, Img="/images/merchant/Trâm Store/books/23.jpg"},
                    new Book(){KeyId = 24, Status=Status.Active, BookTitle="Khẩu ngữ tiếng Thái",Author="Bình Minh",BookCategoryFK=5,MerchantFK=1,isPaperback=false,UnitPrice=89000, Length=13, Height=1, Width=20,PageNumber=196,Description="Cuốn sách là phương tiện hữu ích giúp những người có nhu cầu học tiếng Thái có thể chuẩn bị cho mình vốn từ vựng cơ bản để có thể tự tin hơn khi giao tiếp với người bản địa ở những tình huống phổ biến.", Quantity=20, Img="/images/merchant/Khang Book/books/24.jpg"},
                    new Book(){KeyId = 25, Status=Status.Active, BookTitle="Thế giới phẳng",Author="Thomas L. Friedman",BookCategoryFK=10,MerchantFK=2,isPaperback=false,UnitPrice=272000, Length=16, Height=4, Width=23,PageNumber=720,Description="Trong xu thế toàn cầu hóa, việc tiếp cận và tham khảo những tri thức đương đại từ những nước đã phát triển về sự chuyển động của thế giới (đang ở bước ngoặt từ “tròn” sang “phẳng”, như cách nói của tác giả) có lẽ sẽ giúp chúng ta có thêm những thông tin bổ ích để có sự chủ động trong quá trình hội nhập.", Quantity=20, Img="/images/merchant/Trâm Store/books/25.jpg"},
                    new Book(){KeyId = 26, Status=Status.Active, BookTitle="Sức mạnh của thói quen",Author="Charles Duhigg",BookCategoryFK=8,MerchantFK=1,isPaperback=false,UnitPrice=149000, Length=13, Height=3, Width=20,PageNumber=443,Description="Sức mạnh của thói quen sẽ làm bạn say mê bởi những ý tưởng thú vị, những nghiên cứu ấn tượng, những phân tích thông minh và những lời khuyên thiết thực. Những độc giả đưa cuốn sách này vào danh sách bestseller của Thời báo New York suốt 40 tuần đã kiểm chứng điều đó.", Quantity=20, Img="/images/merchant/Khang Book/books/26.jpg"},
                    new Book(){KeyId = 27, Status=Status.Active, BookTitle="Bài học diệu kỳ từ chiếc xe rác",Author="David J.Pollay",BookCategoryFK=8,MerchantFK=2,isPaperback=false,UnitPrice=30000, Length=10, Height=3, Width=15,PageNumber=112,Description="Bạn có dễ bị tác động bởi cách hành xử của người khác không? Liệu một tài xế taxi chạy ẩu, người phục vụ bàn thô lỗ, người quản lý nóng nảy hay một đồng nghiệp vô ý có phá hỏng một ngày tốt đẹp của bạn không?", Quantity=20, Img="/images/merchant/Trâm Store/books/27.jpg"},
                    new Book(){KeyId = 28, Status=Status.Active, BookTitle="Người xưa đã quên ngày xưa",Author="Anh Khang",BookCategoryFK=14,MerchantFK=1,isPaperback=false,UnitPrice=89000, Length=12, Height=2, Width=20,PageNumber=216,Description="Sự trở lại của hiện tượng xuất bản – nhà văn của nỗi buồn tuổi trẻ. Cảm ơn người, vì đã từng một lần nắm lấy tay nhau.", Quantity=20, Img="/images/merchant/Khang Book/books/28.jpg"},
                    new Book(){KeyId = 29, Status=Status.Active, BookTitle="Truyện cổ Grimm",Author="Jakob Grim - Vilhelm Grim",BookCategoryFK=11,MerchantFK=2,isPaperback=false,UnitPrice=118000, Length=18, Height=1, Width=24,PageNumber=196,Description="Truyện cổ Grimm là một trong số ít những tác phẩm được dịch ra nhiều thứ tiếng nhất trên thế giới. Gần hai thế kỉ sắp qua từ ngày tập truyện cổ dân gian Đức do hai anh em Grimm sưu tầm ra đời, tác phẩm này vẫn là một nguồn cảm hứng dồi dào, mang lại cho bạn đọc niềm vui vô tận, nhắc nhở mọi thế hệ trên toàn cầu một đạo lí nhân bản.", Quantity=20, Img="/images/merchant/Trâm Store/books/29.jpg"},
                    new Book(){KeyId = 30, Status=Status.Active, BookTitle="Tôi tư duy tôi thành đạt",Author="John C. Maxwell",BookCategoryFK=7,MerchantFK=1,isPaperback=false,UnitPrice=109000, Length=15, Height=1, Width=20,PageNumber=126,Description="Những người tư duy tốt luôn luôn có rất nhiều mong muốn và đòi hỏi. Những người biết đặt câu hỏi “làm thế nào” luôn  tìm được một công việc hợp lý, còn những người biết đặt câu hỏi “tại sao” thì luôn luôn trở thành những nhà lãnh đạo.", Quantity=0, Img="/images/merchant/Khang Book/books/30.jpg"},
                    new Book(){KeyId = 31, Status=Status.Active, BookTitle="Ngày xưa có một con bò",Author="Camilo Cruz",BookCategoryFK=10,MerchantFK=2,isPaperback=false,UnitPrice=62000, Length=13, Height=1, Width=20,PageNumber=148,Description="Nếu bạn vẫn muốn nuôi bò, vẫn muốn “ăn mày quá khứ” thì có lẽ cuốn sách này không dành cho bạn. Cuốn sách này chỉ dành cho những người đang nuôi bò và mong muốn được tiêu diệt con bò của chính mình.", Quantity=10, Img="/images/merchant/Trâm Store/books/31.jpg"},
                    new Book(){KeyId = 32, Status=Status.Active, BookTitle="Lối sống tối giản của người Nhật",Author="Camilo Cruz",BookCategoryFK=7,MerchantFK=1,isPaperback=false,UnitPrice=95000, Length=13, Height=1, Width=20,PageNumber=148,Description="Lối sống tối giản là cách sống cắt giảm vật dụng xuống còn mức tối thiểu. Và cùng với cuộc sống ít đồ đạc, ta có thể để tâm nhiều hơn tới hạnh phúc, đó chính là chủ đề của cuốn sách này.", Quantity=10, Img="/images/merchant/Khang Book/books/32.jpg"},
                    new Book(){KeyId = 33, Status=Status.Active, BookTitle="Cẩm nang tự học IETLS",Author="Kiên Trần",BookCategoryFK=5,MerchantFK=2,isPaperback=false,UnitPrice=90000, Length=16, Height=2, Width=24,PageNumber=188,Description="Cuốn sách này khác với các cuốn sách khác ở chỗ nó tập trung thay đổi belief system của bạn thay vì cố nhồi nhét thêm từ vựng hay ngữ pháp khiến bạn… gấp sách lại để năm sau đọc tiếp.", Quantity=0, Img="/images/merchant/Trâm Store/books/33.jpg"},
                    new Book(){KeyId = 34, Status=Status.Active, BookTitle="Khi tài năng không theo kịp giấc mơ",Author="Mèo Maverick",BookCategoryFK=7,MerchantFK=1,isPaperback=false,UnitPrice=82000, Length=13, Height=1, Width=20,PageNumber=256,Description="Khi tài năng không theo kịp giấc mơ, bạn sẽ lựa chọn từ bỏ, hay cố gắng hết sức để vươn tới ước mơ ấy?", Quantity=10, Img="/images/merchant/Khang Book/books/34.jpg"},
                    new Book(){KeyId = 35, Status=Status.Active, BookTitle="Thất tịch không mưa",Author="Lâu Vũ Tình",BookCategoryFK=13,MerchantFK=2,isPaperback=false,UnitPrice=79000, Length=13, Height=2, Width=20,PageNumber=319,Description="Cái chết - có nhiều người sẽ xem nó là một điều tồi tệ nhất, nhưng với cái chết trong câu chuyện này thì nó là cách giải thoát cho họ khỏi số phận đầy bi ai và đau đớn đến xé lòng kia.", Quantity=20, Img="/images/merchant/Trâm Store/books/35.jpg"},
                    new Book(){KeyId = 36, Status=Status.Active, BookTitle="Tớ thích cậu hơn cả Harvard",Author="Lan Rùa",BookCategoryFK=13,MerchantFK=1,isPaperback=false,UnitPrice=98000, Length=13, Height=2, Width=20,PageNumber=369,Description="Tuổi trẻ đầy mộng mơ và tươi đẹp của ai cũng đã từng có một chàng trai bàn bên, một cô nàng lớp kế, một người mà chúng ta dùng tất cả mọi sự thuần khiết và vô tư của mình để tưởng nhớ về một thời Thanh Xuân ấy.", Quantity=10, Img="/images/merchant/Khang Book/books/36.jpg"},
                    new Book(){KeyId = 37, Status=Status.Active, BookTitle="Để con được ốm",Author="Uyên Bùi - BS. Trí Đoàn",BookCategoryFK=12,MerchantFK=2,isPaperback=false,UnitPrice=95000, Length=13, Height=1, Width=20,PageNumber=300,Description="Cuốn sách sẽ giúp các bậc phụ huynh trang bị một số kiến thức cơ bản trong việc chăm sóc trẻ một cách khoa học và góp phần giúp các mẹ và những-người-sẽ-là-mẹ trở nên tự tin hơn trong việc chăm con, xua tan đi những lo lắng, để mỗi em bé ra đời đều được hưởng sự chăm sóc tốt nhất.", Quantity=10, Img="/images/merchant/Trâm Store/books/37.jpg"},
                    new Book(){KeyId = 38, Status=Status.Active, BookTitle="Dám nghĩ lớn",Author="David J. Schwartz. Ph.D",BookCategoryFK=12,MerchantFK=1,isPaperback=false,UnitPrice=98000, Length=13, Height=1, Width=20,PageNumber=200,Description="Bạn không cần phải thông minh tuyệt đỉnh hay tài năng xuất chúng mới đạt được thành công lớn lao, bạn chỉ cần rèn luyện và áp dụng thường xuyên tư duy Dám Nghĩ Lớn. Và đây là những điều diệu kỳ mà cuốn sách sẽ mang đến cho bạn.", Quantity=10, Img="/images/merchant/Khang Book/books/38.jpg"},
                    new Book(){KeyId = 39, Status=Status.Active, BookTitle="Nói nhiều không bằng nói đúng",Author="2.1/2 Bạn Tốt",BookCategoryFK=8,MerchantFK=2,isPaperback=false,UnitPrice=50000, Length=13, Height=1, Width=20,PageNumber=120,Description="Nói chuyện không dễ nghe sẽ khiến mọi người phản cảm và xa cách bạn, đồng thời dẫn đến việc bạn trở thành người có ấn tượng xấu. Lời nói hay như những giai điệu đẹp, ai cũng muốn nghe.", Quantity=10, Img="/images/merchant/Trâm Store/books/39.jpg"},
                    new Book(){KeyId = 40, Status=Status.Active, BookTitle="Ai ở sau lưng bạn thế?",Author="Accototo",BookCategoryFK=11,MerchantFK=1,isPaperback=false,UnitPrice=30000, Length=17, Height=1, Width=24,PageNumber=32,Description="Ai ở sau lưng bạn thế? là bộ truyện Ehon Nhật Bản gồm 6 cuốn dành cho các bé từ 0 - 3 tuổi theo chủ đề về các loài động vật. Điểm thú vị của truyện chính là những bức tranh gồm một loài động vật cụ thể và một phần chi tiết về một con vật khác.", Quantity=89, Img="/images/merchant/Khang Book/books/40.jpg"},
                    new Book(){KeyId = 41, Status=Status.Active, BookTitle="Ngày người thương một người thương khác",Author="Trí",BookCategoryFK=4,MerchantFK=3,isPaperback=false,UnitPrice=60000, Length=22, Height=202, Width=23,PageNumber=300,Description="Sách chia sẻ những câu chuyện tình yêu lãng mạng",Quantity=50,Img="/images/merchant/Chim Shop/books/12.jpg"},
                    new Book(){KeyId = 42, Status=Status.Active, BookTitle="Thám tử lừng danh Conan",Author="Gosho Aoyama,",BookCategoryFK=3,MerchantFK=4,isPaperback=false,UnitPrice=18000, Length=11, Height=210, Width=17,PageNumber=89,Description="Truyện tranh thể loại trinh thám",Quantity=29,Img="/images/merchant/Huy Huỳnh/books/13.jpg"},
                    new Book(){KeyId = 43, Status=Status.Active, BookTitle="Ba người thầy vĩ đại",Author="Robin Sharma",BookCategoryFK=4,MerchantFK=5,isPaperback=false,UnitPrice=78000, Length=22, Height=442, Width=18,PageNumber=233,Description="Cuốn sổ mà cha Mike - người thầy đầu tiên ở Rome đưa cho Jack đã đúc kết 10 điều mà anh đã học được trong suốt cuộc hành trình",Quantity=189,Img="/images/merchant/Phương Nam Book/books/14.jpg"},
                    new Book(){KeyId = 44, Status=Status.Active, BookTitle="Những Câu Chuyện Kỳ Lạ Của Darren Shan 01 - Gánh Xiếc Quái Dị ",Author="Darren Shan",BookCategoryFK=1,MerchantFK=6,isPaperback=false,UnitPrice=160000, Length=20, Height=256, Width=14,PageNumber=119,Description="Những Chuyện Kỳ Lạ Của Darren Shan với những chi tiết phong phú và sinh động, kỳ lạ, quái dị, pha lẫn chút khiếp đảm, không chỉ là những câu chuyện ma quái bình thường mà còn là chuyện kể về đời sống tự nhiên xung quanh ta, về những mối quan hệ tình bạn, tình thầy trò ngập tràn yêu thương và nhân bản.",Quantity=12,Img="/images/merchant/Nhân Company/books/15.jpg"},
                    new Book(){KeyId = 45, Status=Status.Active, BookTitle="Bộ Sách Kinh Điển Về Phân Tâm Học",Author="Sigmund Freud",BookCategoryFK=2,MerchantFK=7,isPaperback=false,UnitPrice=190000, Length=19, Height=223, Width=29,PageNumber=189,Description="Truyện kinh dị ",Quantity=90,Img="/images/merchant/Alpha Books/books/16.jpg"},
                    new Book(){KeyId = 46, Status=Status.Active, BookTitle="The Haunting of Hill House",Author="Shirley Disert",BookCategoryFK=1,MerchantFK=8,isPaperback=false,UnitPrice=120000, Length=21, Height=222, Width=22,PageNumber=132,Description="Truyện tranh thiếu nhi",Quantity=18,Img="/images/merchant/First News/books/17.jpg"},
                    new Book(){KeyId = 47, Status=Status.Active, BookTitle="Tớ Thích Cậu Hơn Cả Harvard",Author="Lan Rùa",BookCategoryFK=3,MerchantFK=9,isPaperback=false,UnitPrice=120000, Length=23, Height=233, Width=20,PageNumber=98,Description="Món quà tuyệt vời cho mùa hè rực rỡ",Quantity=46,Img="/images/merchant/Linh New Style/books/18.jpg"},
                    new Book(){KeyId = 48, Status=Status.Active, BookTitle="Bạn Có Phải Là Đứa Trẻ Sợ Hãi Ẩn Sau Lớp Vỏ Trưởng Thành",Author="Beth Evans",BookCategoryFK=4,MerchantFK=10,isPaperback=false,UnitPrice=230000, Length=27, Height=234, Width=28,PageNumber=209,Description="Cuốn sách tâm lý cùng bạn học cách trưởng thành dành cho người HƯỚNG NỘI.",Quantity=55,Img="/images/merchant/Trung Lê/books/19.jpg"},
                };


                try
                {
                    books.ForEach(x => _context.Books.Add(x));
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    string a = ex.ToString();
                };
            }

            if (_context.AdvertisementContents.Count() == 0)
            {
                _context.AdvertisementContents.AddRange(new List<AdvertisementContent>()
            {
                new AdvertisementContent(){AdvertisementPositionFK=1, AdvertiserFK=1, Title="Bút Thiên Long", Description="Bút TL mua 3 tặng chục", UrlToAdvertisement="http://thienlonggroup.com", CensorStatus=CensorStatus.ContentCensored, ImageLink="/images/advertiser/Bút Kim Long/content/butthienlong.jpg"},
                new AdvertisementContent(){AdvertisementPositionFK=3, AdvertiserFK=3, Title="Laptop Phong Vũ", Description="Laptop khuyến mãi mùa học lại mua 10 tặng 1", UrlToAdvertisement="https://phongvu.vn", CensorStatus=CensorStatus.ContentCensored, ImageLink="/images/advertiser/Phong Vũ/content/phongvu.jpg",},
                new AdvertisementContent(){AdvertisementPositionFK=2, AdvertiserFK=4, Title="Tiki", Description="Săn sách giá rẻ",UrlToAdvertisement="https://tiki.vn", CensorStatus=CensorStatus.ContentCensored,ImageLink="/images/advertiser/Tiki/content/tiki.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=4, AdvertiserFK=4, Title="Tiki Number4", Description="Săn hàng giá rẻ",UrlToAdvertisement="https://tiki.vn", CensorStatus=CensorStatus.ContentCensored,ImageLink="/images/advertiser/Tiki/content/1.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=5, AdvertiserFK=3, Title="PVNo5", Description="Săn hàng mùa deadline",UrlToAdvertisement="https://phongvu.vn", CensorStatus=CensorStatus.ContentCensored,ImageLink="/images/advertiser/Phong Vũ/content/2.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=6, AdvertiserFK=3, Title="PhongVu", Description="Săn laptop giá rẻ",UrlToAdvertisement="https://phongvu.vn", CensorStatus=CensorStatus.Uncensored,ImageLink="/images/advertiser/Phong Vũ/content/3.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=7, AdvertiserFK=1, Title="Bút", Description="Săn BÚT giá CỰC rẻ",UrlToAdvertisement="http://thienlonggroup.com", CensorStatus=CensorStatus.ContentCensored,ImageLink="/images/advertiser/Bút Kim Long/content/4.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=7, AdvertiserFK=1, Title="Bút TL", Description="Bút mới 2 bi",UrlToAdvertisement="http://thienlonggroup.com", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Bút Kim Long/content/5.jpg", },

                //New Data from Theo :">
                new AdvertisementContent(){AdvertisementPositionFK=8, AdvertiserFK=1, Title="Bút TL KM", Description="Mua 10 viết tặng 1 viết xóa",UrlToAdvertisement="http://thienlonggroup.com", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Bút Kim Long/content/4.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=4, AdvertiserFK=3, Title="Phong Vũ KM", Description="Mua laptop tặng cục sạc",UrlToAdvertisement="https://phongvu.vn", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Phong Vũ/content/phongvu.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=6, AdvertiserFK=4, Title="Tiki 91%", Description="Trời ơi! Tin được hông Tiki sale 91 91%",UrlToAdvertisement="https://tiki.vn", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Tiki/content/tiki.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=1, AdvertiserFK=3, Title="Phong Vũ Hè", Description="MIỄN PHÍ giao hàng mùa deadline",UrlToAdvertisement="https://phongvu.vn", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Phong Vũ/content/3.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=2, AdvertiserFK=1, Title="Bút TL KM", Description="GIẢM 50% cho những bạn là Bé Yêu",UrlToAdvertisement="http://thienlonggroup.com", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Bút Kim Long/content/butthienlong.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=3, AdvertiserFK=1, Title="Bút TL no1", Description="Hỗ trợ mùa thi mua viết bi tặng viết chì",UrlToAdvertisement="http://thienlonggroup.com", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Bút Kim Long/content/4.jpg", },
                new AdvertisementContent(){AdvertisementPositionFK=1, AdvertiserFK=4, Title="Tiki Dealine", Description="Tặng ngay phiếu mua hàng mặt hàng cafe cho các Bé Yêu",UrlToAdvertisement="https://tiki.vn", CensorStatus=CensorStatus.Unqualified,ImageLink="/images/advertiser/Tiki/content/1.jpg", },

                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };
            //Seed AdContract
            if (_context.AdvertiseContracts.Count() == 0)
            {
                _context.AdvertiseContracts.AddRange(new List<AdvertiseContract>()
            {
                new AdvertiseContract(){AdvertisementContentFK=3, DateStart=DateTime.Parse("2019-05-11"), DateFinish=DateTime.Parse("2019-05-13 23:59:59"), ContractValue=27000000,  Deposite=13500000, Status=ContractStatus.AccountingCensored },
                new AdvertiseContract(){AdvertisementContentFK=1, DateStart=DateTime.Parse("2019-05-01"), DateFinish=DateTime.Parse("2019-05-05 23:59:59"), ContractValue=50000000,  Deposite=25000000, Status=ContractStatus.Success },
                new AdvertiseContract(){AdvertisementContentFK=2, DateStart=DateTime.Parse("2019-05-01"), DateFinish=DateTime.Parse("2019-05-05 23:59:59"), ContractValue=45000000,  Deposite=22500000, Status=ContractStatus.Unqualified },
                new AdvertiseContract(){AdvertisementContentFK=8, DateStart=DateTime.Parse("2019-05-12"), DateFinish=DateTime.Parse("2019-05-16 23:59:59"), ContractValue=25000000,  Deposite=12500000, Status=ContractStatus.AccountingCensored },
                new AdvertiseContract(){AdvertisementContentFK=5, DateStart=DateTime.Parse("2019-05-04"), DateFinish=DateTime.Parse("2019-05-08 23:59:59"), ContractValue=250000000, Deposite=125000000,  Status=ContractStatus.Success },
                new AdvertiseContract(){AdvertisementContentFK=7, DateStart=DateTime.Parse("2019-05-08"), DateFinish=DateTime.Parse("2019-05-10 23:59:59"), ContractValue=15000000,  Deposite=7500000, Status=ContractStatus.Unqualified },
                new AdvertiseContract(){AdvertisementContentFK=4, DateStart=DateTime.Parse("2019-05-08"), DateFinish=DateTime.Parse("2019-05-10 23:59:59"), ContractValue=240000000, Deposite=120000000, Status=ContractStatus.Success },
                new AdvertiseContract(){AdvertisementContentFK=6, DateStart=DateTime.Parse("2019-05-13"), DateFinish=DateTime.Parse("2019-05-17 23:59:59"), ContractValue=240000000, Deposite=120000000,  Status=ContractStatus.AccountingCensored },
                new AdvertiseContract(){AdvertisementContentFK=13, DateStart=DateTime.Parse("2019-05-14"), DateFinish=DateTime.Parse("2019-05-16 23:59:59"), ContractValue=27000000, Deposite=13500000, Status=ContractStatus.AccountingCensored },
                new AdvertiseContract(){AdvertisementContentFK=9, DateStart=DateTime.Parse("2019-05-05"), DateFinish=DateTime.Parse("2019-05-08 23:59:59"), ContractValue=27000000,  Deposite=13500000, Status=ContractStatus.Unqualified },
                new AdvertiseContract(){AdvertisementContentFK=11, DateStart=DateTime.Parse("2019-05-02"), DateFinish=DateTime.Parse("2019-05-07 23:59:59"), ContractValue=60000000, Deposite=30000000,  Status=ContractStatus.AccountingCensored },
                new AdvertiseContract(){AdvertisementContentFK=10, DateStart=DateTime.Parse("2019-05-12"), DateFinish=DateTime.Parse("2019-05-15 23:59:59"), ContractValue=320000000,Deposite=160000000, Status=ContractStatus.AccountingCensored },
                new AdvertiseContract(){AdvertisementContentFK=12, DateStart=DateTime.Parse("2019-05-13"), DateFinish=DateTime.Parse("2019-05-16 23:59:59"), ContractValue=40000000, Deposite=20000000,Status=ContractStatus.Unqualified },
                new AdvertiseContract(){AdvertisementContentFK=15, DateStart=DateTime.Parse("2019-05-07"), DateFinish=DateTime.Parse("2019-05-10 23:59:59"), ContractValue=40000000, Deposite=20000000,Status=ContractStatus.Unqualified },
                new AdvertiseContract(){AdvertisementContentFK=14, DateStart=DateTime.Parse("2019-05-13"), DateFinish=DateTime.Parse("2019-05-17 23:59:59"), ContractValue=45000000, Deposite=22500000,Status=ContractStatus.AccountingCensored },




                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.MerchantContracts.Count() == 0)
            {
                _context.MerchantContracts.AddRange(new List<MerchantContract>()
            {
                    new MerchantContract(){ ContractLink="", MerchantFK = 1, DateStart = DateTime.Parse("2019-01-01"), DateEnd = DateTime.Parse("2020-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 2, DateStart = DateTime.Parse("2019-01-01"), DateEnd = DateTime.Parse("2021-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 3, DateStart = DateTime.Parse("2019-01-01"), DateEnd = DateTime.Parse("2022-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 4, DateStart = DateTime.Parse("2018-01-01"), DateEnd = DateTime.Parse("2025-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 5, DateStart = DateTime.Parse("2017-01-01"), DateEnd = DateTime.Parse("2024-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 6, DateStart = DateTime.Parse("2016-01-01"), DateEnd = DateTime.Parse("2024-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 7, DateStart = DateTime.Parse("2016-01-01"), DateEnd = DateTime.Parse("2026-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 8, DateStart = DateTime.Parse("2016-01-01"), DateEnd = DateTime.Parse("2022-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 8, DateStart = DateTime.Parse("2015-01-01"), DateEnd = DateTime.Parse("2021-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 9, DateStart = DateTime.Parse("2016-01-01"), DateEnd = DateTime.Parse("2022-01-01")},
                    new MerchantContract(){ ContractLink="", MerchantFK = 10, DateStart = DateTime.Parse("2029-01-01"), DateEnd = DateTime.Parse("2023-01-01")}
                });
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.Invoices.Count() == 0)
            {
                    _context.Invoices.AddRange(new List<Invoice>()
                {
                        new Invoice(){KeyId = 1,  CustomerFK=1,TotalPrice=182000,DeliAddress="Lầu 19 Landmark 81 Q.Bình Thạnh", DeliContactName="Mỡ's Má", DeliContactHotline="0908468188"},
                        new Invoice(){KeyId = 2,  CustomerFK=1,TotalPrice=44000,DeliAddress="Lầu 19 Landmark 81 Q.Bình Thạnh", DeliContactName="Mỡ's Ba", DeliContactHotline="0908466048"},
                        new Invoice(){KeyId = 3,  CustomerFK=2,TotalPrice=120000,DeliAddress="26 Nguyễn Trãi Q.3", DeliContactName="Con Cá", DeliContactHotline="0908468183"},
                        new Invoice(){KeyId = 4,  CustomerFK=3,TotalPrice=45000,DeliAddress="07 Pasteur Q3", DeliContactName="UTS Bi", DeliContactHotline="0908468182"},
                        new Invoice(){KeyId = 5,  CustomerFK=4,TotalPrice=133000,DeliAddress="12 Trần Hưng Đạo Q.5", DeliContactName="Bánh Mì", DeliContactHotline="0908468908"},
                        new Invoice(){KeyId = 6,  CustomerFK=4,TotalPrice=40000,DeliAddress="12 Trần Hưng Đạo Q.5", DeliContactName="Bánh Mì", DeliContactHotline="0908468908"},
                        new Invoice(){KeyId = 7,  CustomerFK=5,TotalPrice=219000,DeliAddress="02 Phạm Hùng Q.8", DeliContactName="Phát Tài", DeliContactHotline="0908221882"},
                        new Invoice(){KeyId = 8,  CustomerFK=6,TotalPrice=165000,DeliAddress="45 Nguyễn Hữu Cảnh Q.Bình Thạnh", DeliContactName="Thành công", DeliContactHotline="0938368188"},
                        new Invoice(){KeyId = 9,  CustomerFK=7,TotalPrice=183000,DeliAddress="17 Chung cư Trần Quang Khải Q.1", DeliContactName="Minh Khánh", DeliContactHotline="0908338188"},
                        new Invoice(){KeyId = 10, CustomerFK=7,TotalPrice=145000,DeliAddress="17 Chung cư Trần Quang Khải Q.1", DeliContactName="Minh Anh", DeliContactHotline="0908463388"},
                        new Invoice(){KeyId = 11, CustomerFK=8,TotalPrice=47000,DeliAddress="03 Trương Định Q.3", DeliContactName="Hùng Em", DeliContactHotline="0908468133"},
                        new Invoice(){KeyId = 12, CustomerFK=9,TotalPrice=165000,DeliAddress="19 Nam Kỳ Khởi Nghĩa", DeliContactName="Linh Linh", DeliContactHotline="0908338188"},
                        new Invoice(){KeyId = 13, CustomerFK=10,TotalPrice=12000,DeliAddress="20 Đinh Công Tráng Q.1", DeliContactName="Hồng Thu", DeliContactHotline="0908468228"},
                        new Invoice(){KeyId = 14, CustomerFK=10,TotalPrice=47000,DeliAddress="20 Đinh Công Tráng Q.1", DeliContactName="Hồng Thịnh", DeliContactHotline="0908462288"},
                    });
             
            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.InvoiceDetails.Count() == 0)
            {
                _context.InvoiceDetails.AddRange(new List<InvoiceDetail>()
                {
                    new InvoiceDetail(){KeyId = 1,  InvoiceFK=1,BookFK=1,Qty=2,UnitPrice=45000,SubTotal=90000},
                    new InvoiceDetail(){KeyId = 2,  InvoiceFK=1,BookFK=2,Qty=2,UnitPrice=46000,SubTotal=92000},
                    new InvoiceDetail(){KeyId = 3,  InvoiceFK=2,BookFK=3,Qty=1,UnitPrice=44000,SubTotal=44000},
                    new InvoiceDetail(){KeyId = 4,  InvoiceFK=3,BookFK=4,Qty=1,UnitPrice=55000,SubTotal=55000},
                    new InvoiceDetail(){KeyId = 5,  InvoiceFK=3,BookFK=5,Qty=1,UnitPrice=65000,SubTotal=65000},
                    new InvoiceDetail(){KeyId = 6,  InvoiceFK=4,BookFK=6,Qty=2,UnitPrice=45000,SubTotal=90000},
                    new InvoiceDetail(){KeyId = 7,  InvoiceFK=5,BookFK=3,Qty=1,UnitPrice=89000,SubTotal=89000},
                    new InvoiceDetail(){KeyId = 8,  InvoiceFK=5,BookFK=4,Qty=1,UnitPrice=44000,SubTotal=44000},
                    new InvoiceDetail(){KeyId = 9,  InvoiceFK=6,BookFK=7,Qty=2,UnitPrice=40000,SubTotal=80000},
                    new InvoiceDetail(){KeyId = 10, InvoiceFK=7,BookFK=11,Qty=1,UnitPrice=120000,SubTotal=120000},
                    new InvoiceDetail(){KeyId = 11, InvoiceFK=7,BookFK=17,Qty=1,UnitPrice=99000,SubTotal=99000},
                    new InvoiceDetail(){KeyId = 12, InvoiceFK=8,BookFK=13,Qty=2,UnitPrice=165000,SubTotal=330000},
                    new InvoiceDetail(){KeyId = 13, InvoiceFK=9,BookFK=12,Qty=1,UnitPrice=165000,SubTotal=165000},
                    new InvoiceDetail(){KeyId = 14, InvoiceFK=9,BookFK=16,Qty=1,UnitPrice=18000,SubTotal=18000},
                    new InvoiceDetail(){KeyId = 15, InvoiceFK=10,BookFK=15,Qty=2,UnitPrice=145000,SubTotal=290000},
                    new InvoiceDetail(){KeyId = 16, InvoiceFK=11,BookFK=19,Qty=3,UnitPrice=37000,SubTotal=111000},
                    new InvoiceDetail(){KeyId = 17, InvoiceFK=11,BookFK=18,Qty=3,UnitPrice=10000,SubTotal=30000},
                    new InvoiceDetail(){KeyId = 18, InvoiceFK=12,BookFK=12,Qty=2,UnitPrice=165000,SubTotal=330000},
                    new InvoiceDetail(){KeyId = 19, InvoiceFK=13,BookFK=11,Qty=1,UnitPrice=12000,SubTotal=120000},
                    new InvoiceDetail(){KeyId = 20, InvoiceFK=14,BookFK=18,Qty=2,UnitPrice=10000,SubTotal=20000},
                    new InvoiceDetail(){KeyId = 21, InvoiceFK=14,BookFK=19,Qty=1,UnitPrice=37000,SubTotal=37000},
                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.Deliveries.Count() == 0)
            {
                _context.Deliveries.AddRange(new List<Delivery>()
                {
                    new Delivery(){KeyId = 1,  InvoiceFK=1,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Confirmed, OrderPrice=90000,ShipPrice=25000,},
                    new Delivery(){KeyId = 2,  InvoiceFK=1,MerchantFK=2,DeliveryStatus=Const_DeliStatus.OnDelivery, OrderPrice=92000, ShipPrice=25000},
                    new Delivery(){KeyId = 3,  InvoiceFK=2,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Packaged, OrderPrice=44000, ShipPrice=25000},
                    new Delivery(){KeyId = 4,  InvoiceFK=3,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Success, OrderPrice=55000,ShipPrice=25000},
                    new Delivery(){KeyId = 5,  InvoiceFK=3,MerchantFK=2,DeliveryStatus=Const_DeliStatus.UnConfirmed, OrderPrice=65000},
                    new Delivery(){KeyId = 6,  InvoiceFK=4,MerchantFK=1,DeliveryStatus=Const_DeliStatus.OnDelivery, OrderPrice=90000,ShipPrice=25000},
                    new Delivery(){KeyId = 7,  InvoiceFK=5,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Fail, OrderPrice=133000, ShipPrice=25000},
                    new Delivery(){KeyId = 8,  InvoiceFK=6,MerchantFK=2,DeliveryStatus=Const_DeliStatus.Confirmed, OrderPrice=80000, ShipPrice=25000},
                    new Delivery(){KeyId = 9,  InvoiceFK=7,MerchantFK=2,DeliveryStatus=Const_DeliStatus.UnConfirmed, OrderPrice=219000},
                    new Delivery(){KeyId = 10, InvoiceFK=8,MerchantFK=2,DeliveryStatus=Const_DeliStatus.Success, OrderPrice=330000, ShipPrice=25000},
                    new Delivery(){KeyId = 11, InvoiceFK=9,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Confirmed, OrderPrice=183000, ShipPrice=25000},
                    new Delivery(){KeyId = 12, InvoiceFK=10,MerchantFK=2,DeliveryStatus=Const_DeliStatus.OnDelivery, OrderPrice=290000,ShipPrice=25000},
                    new Delivery(){KeyId = 13, InvoiceFK=11,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Packaged, OrderPrice=30000,ShipPrice=25000},
                    new Delivery(){KeyId = 14, InvoiceFK=11,MerchantFK=2,DeliveryStatus=Const_DeliStatus.Success, OrderPrice=111000,ShipPrice=25000},
                    new Delivery(){KeyId = 15, InvoiceFK=12,MerchantFK=1,DeliveryStatus=Const_DeliStatus.Confirmed, OrderPrice=330000,ShipPrice=25000},
                    new Delivery(){KeyId = 16, InvoiceFK=13,MerchantFK=2,DeliveryStatus=Const_DeliStatus.OnDelivery, OrderPrice=120000,ShipPrice=25000},
                    new Delivery(){KeyId = 17, InvoiceFK=14,MerchantFK=1,DeliveryStatus=Const_DeliStatus.UnConfirmed, OrderPrice=20000},
                    new Delivery(){KeyId = 18, InvoiceFK=14,MerchantFK=2,DeliveryStatus=Const_DeliStatus.UnConfirmed, OrderPrice=37000 }
                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };
                
            if (_context.RatingDetails.Count() == 0)
            {
                _context.RatingDetails.AddRange(new List<RatingDetail>()
                {
                        new RatingDetail(){ BookFK = 1, CustomerFK = 1, Rating = 5,Comment = "good"},
                        new RatingDetail(){ BookFK = 2, CustomerFK = 1, Rating = 4,Comment = "blabla"},
                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.BooksIns.Count() == 0)
            {
                _context.BooksIns.AddRange(new List<BooksIn>()
                {
                        new BooksIn(){MerchantFK=1, DateCreated=DateTime.Parse("2019-02-03 10:13:14"), DateModified=DateTime.Now,  },
                            
                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };

            if (_context.BooksInDetails.Count() == 0)
            {
                _context.BooksInDetails.AddRange(new List<BooksInDetail>()
                {
                        new BooksInDetail(){BooksInFK=1, BookFK=1, Qty=10, Price=10000  },
                        new BooksInDetail(){BooksInFK=1, BookFK=3, Qty=21, Price=100000  },

                });

            }
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string a = ex.ToString();
            };
        }
    }
}
