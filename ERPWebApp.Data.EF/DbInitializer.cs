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

                      new Function() {KeyId = "ReceiptandDelivery",Name = "Hóa đơn & Giao hàng",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-receipt"  },
                      new Function() {KeyId = "ReceiptItem",Name = "Hóa đơn",ParentId = "ReceiptandDelivery",SortOrder = 1,Status = Status.Active,URL = "/Invoice",IconCss = "fa-home"  },
                      new Function() {KeyId = "DeliveryItem",Name = "Phiếu giao hàng",ParentId = "ReceiptandDelivery",SortOrder = 2,Status = Status.Active,URL = "/Delivery",IconCss = "fa-home"  },

                      new Function() {KeyId = "ContractItem",Name = "Hợp đồng",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-file-signature"  },

                      new Function() {KeyId = "MerchantContractItem",Name = "Hợp đồng với nhà cung cấp",ParentId = "ContractItem",SortOrder = 1,Status = Status.Active,URL = "/MerchantContract",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "AdvertiseContractItem",Name = "Hợp đồng quảng cáo",ParentId = "ContractItem",SortOrder = 2,Status = Status.Active,URL = "/AdvertiseContract",IconCss = "fa-clone"  },
   
                      new Function() {KeyId = "TermItem",Name = "Điều khoản dịch vụ, quy trình",ParentId = null,SortOrder = 6,Status = Status.Active,URL = "/",IconCss = "fa-handshake"  },

                      new Function() {KeyId = "MerchantTermItem",Name = "Đối với Nhà cung cấp",ParentId = "TermItem",SortOrder = 3,Status = Status.Active,URL = "/Process/Merchant",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "AdvertiserTermItem",Name = "Đối với bên quảng cáo",ParentId = "TermItem",SortOrder = 2,Status = Status.Active,URL = "/Process/Advertiser",IconCss = "fa-clone"  },
                      new Function() {KeyId = "GeneralTermItem",Name = "Điều khoản và dịch vụ",ParentId = "TermItem",SortOrder = 1,Status = Status.Active,URL = "/Process/GeneralTerm",IconCss = "fa-clone"  },

                      new Function() {KeyId = "StatisticsItem",Name = "Thống kê",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-chart-line"  },

                      new Function() {KeyId = "AdStatisticsItem",Name = "Quảng cáo", ParentId = "StatisticsItem",SortOrder = 1,Status = Status.Active,URL = "/AdvertiseContract/Statistic",IconCss = "fa-chevron-down"  },
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
                    Name = "Customer",
                    NormalizedName = "Khách hàng",
                    Description = "Khách mua sách"
                };
                await _roleManager.CreateAsync(_item1);
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Merchant",
                    NormalizedName = "Nhà bán sách",
                    Description = "Nhà cung cấp sách"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Advertiser",
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
                    Name = "WebMaster_Accountant",
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
                    Name = "WebMaster_AdvertiserCensor",
                    NormalizedName = "Kiểm duyệt Advertiser_WebMaster",
                    Description = "Thằng kiểm duyệt quảng cáo"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Admin",
                    NormalizedName = "Admin_WebMaster",
                    Description = "Thằng Admin quản lý hợp đồng"
                });
             
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_MerchantCensor",
                    NormalizedName = "Kiểm duyệt Merchant_WebMaster",
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
                    new BookCategory(){Status=Status.Active,BookCategoryName="Kinh dị"},
                    new BookCategory(){Status=Status.Active,BookCategoryName="Học thuật"},
                    new BookCategory(){Status=Status.Active,BookCategoryName="Truyện"},
                    new BookCategory(){Status=Status.Active,BookCategoryName="Loại khác"},

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
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore114@gmail.com"); // tim user admin
                    _userManager.AddToRoleAsync(user, "test").Wait(); // add vao role test :">
                    _context.WebMasters.Add(new WebMaster(){ UserFK = user.Id, WebMasterTypeFK = Const_WebmasterType.Webmaster });
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
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("accountant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Accountant"); // add vao role accountant
                    _context.WebMasters.Add(new WebMaster(){ UserFK = user.Id, WebMasterTypeFK = Const_WebmasterType.Accountant });
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
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("adcensor@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_AdvertiserCensor"); // add vao role 
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
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("saleadmin@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Admin"); // add vao role 
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
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("merchantcensor@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_MerchantCensor"); // add vao role 
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
                    Status = Status.Active,
                    PhoneNumber="09000000002",
                    Address = "Chung cư HaDo Q.10",
                    Gender = Gender.Male,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("cucmo_cuaca@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Customer"); // add vao role 
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
                    Status = Status.Active,
                    PhoneNumber = "0900000001",
                    Address = "289 Bình Đông F14 Q8",
                    Gender = Gender.Female,
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("conca_cuamo@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Customer"); // add vao role 
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
                    Status = Status.Active,
                    Gender = Gender.Male
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("khang_merchant@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Merchant"); // add vao role 
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
                    await _userManager.AddToRoleAsync(user, "Merchant"); // add vao role 
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

                //tạo user advertiser
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "butkimlong@gmail.com",
                    FullName = "Anh Long",
                    Email = "butkimlong@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser, //Webmaster
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("butkimlong@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Advertiser"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Bút Kim Long", UrlToBrand = "butkimlong.com.vn", Status = Status.Active });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "viethung@gmail.com",
                    FullName = "Chị Hưng",
                    Email = "viethung@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser, //Webmaster
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("viethung@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Advertiser"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Bao Sách Viết Hưng", UrlToBrand = "bsviethung.com.vn", Status = Status.InActive });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "phongzu@gmail.com",
                    FullName = "Anh Zũ",
                    Email = "phongzu@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser, //Webmaster
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("phongzu@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Advertiser"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Phong Vũ", UrlToBrand = "phongvu.com.vn",  Status = Status.Active });
                }

                result = _userManager.CreateAsync(new User()
                {
                    UserName = "tiki@gmail.com",
                    FullName = "Anh Ki",
                    Email = "tiki@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser, //Webmaster
                    Status = Status.Active,
                    Gender = Gender.Other
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("tiki@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Advertiser"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id, BrandName = "Tiki", UrlToBrand = "tiki.vn", Status = Status.Active });
                }
                #endregion

                if (_context.Books.Count() == 0)
                {
                    _context.Books.AddRange(new List<Book>()
                {
                    new Book(){Status=Status.Active, BookTitle="Đắc Nhân Tâm",Author="Dale Carnegie",BookCategoryFK=4,MerchantFK=1,isPaperback=false,UnitPrice=45000, Length=21, Height=200, Width=12,PageNumber=320,Description="Đắc Nhân Tâm nằm trong top bán chạy nhất thế giới",Quantity=50, Img="/images/merchant/Khang Book/books/1.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Đời ngắn đừng ngủ dài",Author="Robins Sharma",BookCategoryFK=4,MerchantFK=2,isPaperback=true,UnitPrice=46000, Length=21, Height=150, Width=13,PageNumber=239,Description="Bằng những lời chia sẻ thật ngắn gọn, dễ hiểu về những trải nghiệm và suy ngẫm trong đời, Robin Sharma tiếp tục phong cách viết của ông từ cuốn sách Điều vĩ đại đời thường để mang đến cho độc giả những bài viết như lời tâm sự, vừa chân thành vừa sâu sắc.",Quantity=50,Img="/images/merchant/Trâm Store/books/2.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Nếu tôi biết khi còn 20",Author="Tina Seelig",BookCategoryFK=4,MerchantFK=1,isPaperback=true,UnitPrice=44000, Length=21, Height=270, Width=14,PageNumber=252,Description="Thông qua quyển sách, tác giả còn muốn các độc giả, đặc biệt là độc giả trẻ, sẽ được  bị đủ sự tự tin để biến căng thẳng thành sự hào hứng, biến thử thách thành các cơ hội, và cứ sau mỗi lần vấp ngã lại đứng lên trưởng thành hơn.",Quantity=50,Img="/images/merchant/Khang Book/books/3.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Café sáng cùng Tony",Author="Tony",BookCategoryFK=4,MerchantFK=1,isPaperback=false,UnitPrice=55000, Length=21, Height=340, Width=15,PageNumber=323,Description="Chúng tôi tin rằng những người trẻ tuổi luôn mang trong mình khát khao vươn lên và tấm lòng hướng thiện, và có nhiều cách để động viên họ biến điều đó thành hành động. Nếu như tập sách nhỏ này có thể khơi gợi trong lòng bạn đọc trẻ một cảm hứng tốt đẹp, như tách cà phê thơm vào đầu ngày nắng mới, thì đó là niềm vui lớn của tác giả Tony Buổi Sáng.",Quantity=46,Img="/images/merchant/Khang Book/books/4.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Tuổi trẻ đáng giá bao nhiêu ?",Author="Chi",BookCategoryFK=4,MerchantFK=2,isPaperback=true,UnitPrice=65000, Length=21, Height=155, Width=16,PageNumber=234,Description="Hãy đọc sách, nếu bạn đọc sách một cách bền bỉ, sẽ đến lúc bạn bị thôi thúc không ngừng bởi ý muốn viết nên cuốn sách của riêng mình. Nếu tôi còn ở tuổi đôi mươi, hẳn là tôi sẽ đọc Tuổi trẻ đáng giá bao nhiêu? nhiều hơn một lần.",Quantity=45,Img="/images/merchant/Trâm Store/books/5.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Tokyo Hoàng Đạo Án",Author="Soji Shimada",BookCategoryFK=1,MerchantFK=1,isPaperback=false,UnitPrice=45000, Length=21, Height=157, Width=17,PageNumber=332,Description="Tiểu thuyết trinh thám kinh dị",Quantity=47,Img="/images/merchant/Khang Book/books/6.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Athoner",Author="Yukito Ayatsuji",BookCategoryFK=1,MerchantFK=2,isPaperback=true,UnitPrice=40000, Length=21, Height=247, Width=18,PageNumber=231,Description="Hai mươi sáu năm về trước có một học sinh hoàn thiện hoàn mĩ. Rất đẹp, rất giỏi, rất hòa đồng, ai cũng yêu quý, những lời tán tụng người ấy được truyền mãi qua các thế hệ học sinh của trường. Nhưng tên đầy đủ là gì, chết đi thế nào, thậm chí giới tính ra sao lại không một ai hay biết. Người ta chỉ rỉ tai nhau, bỗng nhiên giữa năm bạn ấy chết, trường lớp không sao thoát được nỗi buồn nhớ thương, họ bèn cư xử như thể học sinh này còn tồn tại. Bàn ghế để nguyên, bạn cùng lớp vẫn giả vờ nói chuyện với người đã khuất.",Quantity=34,Img="/images/merchant/Trâm Store/books/7.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Kỹ Thuật Làm Bánh Ngọt - Cuốn Sách Cho Người Bắt Đầu Học Làm Bánh",Author="Thảo",BookCategoryFK=2,MerchantFK=1,isPaperback=true,UnitPrice=20000, Length=32, Height=450, Width=19,PageNumber=332,Description="Cuốn Sách Cho Người Bắt Đầu Học Làm Bánh là những kiến thức cơ bản nhất mà một người bắt đầu làm quen với bột, lò nướng cần có: Gồm các kiến thức chung về nguyên liệu, dụng cụ làm bánh, các kỹ năng làm bánh cơ bản. Cuốn sách còn cung cấp các công thức làm bánh Cookies. Muffin, Cream Cake, Tart đơn giản, dễ làm nhưng cũng rất hấp dẫn, ngon miệng.",Quantity=43,Img="/images/merchant/Khang Book/books/8.jpg"},
                    new Book(){Status=Status.Active, BookTitle="150 Thuật Ngữ Văn Học",Author="Lại Nguyên Ân",BookCategoryFK=2,MerchantFK=2,isPaperback=false,UnitPrice=10000, Length=7, Height=643, Width=20,PageNumber=588,Description="150 Thuật Ngữ Văn Học - Với khoảng trên 150 mục từ THUẬT NGỮ VĂN HỌC, cuốnsách nhỏ này chưa thể bao quát toàn bộ các bình diện, cấp độ, sắc thái của một loại hiện tượng văn hoá nhân bản đặc sắc và vô cùng phong phú là văn học và các chuyên ngành nghiên cứu nó…",Quantity=42,Img="/images/merchant/Trâm Store/books/9.png"},
                    new Book(){Status=Status.Active, BookTitle="Truyện ngụ ngôn Êdốp",Author="êdop",BookCategoryFK=3,MerchantFK=1,isPaperback=true,UnitPrice=36000, Length=13, Height=342, Width=21,PageNumber=46,Description="Truyện tranh thiếu nhi",Quantity=46,Img="/images/merchant/Khang Book/books/10.jpg"},
                    new Book(){Status=Status.Active, BookTitle="Alixơ ở xứ sở diệu kỳ",Author="Hàn Thuyên",BookCategoryFK=3,MerchantFK=2,isPaperback=false,UnitPrice=120000, Length=21, Height=222, Width=22,PageNumber=132,Description="Truyện tranh thiếu nhi",Quantity=46,Img="/images/merchant/Trâm Store/books/11.jpg"},

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

                if (_context.AdvertisementContents.Count() == 0)
                {
                    _context.AdvertisementContents.AddRange(new List<AdvertisementContent>()
                {
                    new AdvertisementContent(){AdvertisementPositionFK=1, AdvertiserFK=1, Title="Bút Thiên Long", Description="Bút TL mua 3 tặng chục", UrlToAdvertisement="thienloi.vn", Deposite=2000000, CensorStatus=CensorStatus.AccountingCensored, ImageLink="/images/advertiser/Bút Kim Long/content/butthienlong.jpg", },
                    new AdvertisementContent(){AdvertisementPositionFK=3,AdvertiserFK=3, Title="Laptop Phong Vũ", Description="Laptop khuyến mãi mùa học lại mua 10 tặng 1", UrlToAdvertisement="phongvu.vn", Deposite=2000000, CensorStatus=CensorStatus.AccountingCensored, ImageLink="/images/advertiser/Phong Vũ/content/phongvu.jpg",},
                    new AdvertisementContent(){AdvertisementPositionFK=2, AdvertiserFK=4, Title="Tiki", Description="Săn sách giá rẻ",UrlToAdvertisement="tiki.vn",Deposite=2000000, CensorStatus=CensorStatus.AccountingCensored,ImageLink="/images/advertiser/Tiki/content/tiki.jpg", },
                   
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
                    new AdvertiseContract(){AdvertisementContentFK=3, DateStart=DateTime.Parse("2019-05-11"), DateFinish=DateTime.Parse("2019-05-13 23:59:59"), ContractValue=27000000, },
                    new AdvertiseContract(){},
                    new AdvertiseContract(){}
                   
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
                        new MerchantContract(){ ContractLink="", MerchantFK = 2, DateStart = DateTime.Parse("2019-01-01"), DateEnd = DateTime.Parse("2020-01-01")}
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
                            new Invoice(){ CustomerFK=1,TotalPrice=182000,DeliAddress="Lầu 19 Landmark 81 Q.Bình Thạnh", DeliContactName="Mỡ's Má", DeliContactHotline="0908468188"},
                            new Invoice(){ CustomerFK=1,TotalPrice=355000,DeliAddress="Lầu 19 Landmark 81 Q.Bình Thạnh", DeliContactName="Mỡ's Ba", DeliContactHotline="0908466048"}
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
                            new InvoiceDetail(){ InvoiceFK=1,BookFK=1,Qty=2,UnitPrice=45000,SubTotal=90000},
                            new InvoiceDetail(){ InvoiceFK=1,BookFK=2,Qty=2,UnitPrice=46000,SubTotal=92000},
                            new InvoiceDetail(){ InvoiceFK=2,BookFK=5,Qty=3,UnitPrice=65000,SubTotal=195000},
                            new InvoiceDetail(){ InvoiceFK=2,BookFK=7,Qty=4,UnitPrice=40000,SubTotal=160000},
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
                            new Delivery(){ InvoiceFK=1,MerchantFK=1,DeliveryStatus=1, OrderPrice=0},
                            new Delivery(){ InvoiceFK=1,MerchantFK=2,DeliveryStatus=1, OrderPrice=0},
                            new Delivery(){ InvoiceFK=2,MerchantFK=1,DeliveryStatus=1, OrderPrice=0},
                            new Delivery(){ InvoiceFK=2,MerchantFK=2,DeliveryStatus=1, OrderPrice=0},
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

            }
        }
    }
}
