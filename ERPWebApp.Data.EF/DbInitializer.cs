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

                      new Function() {KeyId = "PermissionItem", Name = "Phân quyền",ParentId = "UsersItem",SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "RoleItem", Name = "Nhập liệu nhóm quyền",ParentId = "PermissionItem",SortOrder = 1,Status = Status.Active,URL = "/Role",IconCss = "fa-home"  },
                      new Function() {KeyId = "UserRoleItem",Name = "User vào nhóm quyền",ParentId = "PermissionItem",SortOrder = 2,Status = Status.Active,URL = "/User",IconCss = "fa-hom"  },


                      new Function() {KeyId = "ProductItem",Name = "Sản phẩm",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-building"  },

                      new Function() {KeyId = "BookItem",Name = "Sách",ParentId = "ProductItem",SortOrder = 1,Status = Status.Active,URL = "/Book",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "BookCategoryItem",Name = "Loại sách",ParentId = "ProductItem",SortOrder = 2,Status = Status.Active,URL = "/UnitCategory",IconCss = "fa-clone"  },
                      new Function() {KeyId = "AdvertisementContentItem",Name = "Nội dung quảng cáo",ParentId = "ProductItem",SortOrder = 3,Status = Status.Active,URL = "/UnitCategory",IconCss = "fa-clone"  },

                      new Function() {KeyId = "ContractItem",Name = "Hợp đồng",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-building"  },

                      new Function() {KeyId = "MerchantContractItem",Name = "Hợp đồng với nhà cung cấp",ParentId = "ContractItem",SortOrder = 1,Status = Status.Active,URL = "/Book",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "AdvertiseContractItem",Name = "Hợp đồng với nhà quảng cáo",ParentId = "ContractItem",SortOrder = 2,Status = Status.Active,URL = "/UnitCategory",IconCss = "fa-clone"  },

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
                    Description = "Thằng bán sách"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "Advertiser",
                    NormalizedName = "Người quảng cáo",
                    Description = "Thằng đăng quảng cáo"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster",
                    NormalizedName = "Full quyền WebMaster",
                    Description = "Thằng full quyền khi test"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Accountant",
                    NormalizedName = "Kế toán_WebMaster",
                    Description = "Thằng kế toán trong WM"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "test",
                    NormalizedName = "nhóm test ",
                    Description = "full chức năng tiện cho qua trình test. sẽ bị xóa trước khi chạy thực tế."
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_AdvertiserCensor",
                    NormalizedName = "Kiểm duyệt Advertiser_WebMaster",
                    Description = "Thằng kiểm duyệt quảng cáo trong WM"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_Admin",
                    NormalizedName = "Admin_WebMaster",
                    Description = "Thằng Admin ký hợp đồng với bên Merchant"
                });
             
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "WebMaster_MerchantCensor",
                    NormalizedName = "Kiểm duyệt Merchant_WebMaster",
                    Description = "Thằng kiểm duyệt merchant trong WM"
                });
                

            }
            #endregion

            if (_context.BookCategorys.Count() == 0)
            {
                _context.BookCategorys.AddRange(new List<BookCategory>()
                {
                    new BookCategory(){BookCategoryName="Kinh dị"},
                    new BookCategory(){BookCategoryName="Học thuật"},
                    new BookCategory(){BookCategoryName="Truyện"},
                    new BookCategory(){BookCategoryName="Loại khác"},

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
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore114@gmail.com"); // tim user admin
                    _userManager.AddToRoleAsync(user, "test").Wait(); // add vao role test :">
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.Webmaster, UserFK = user.Id });
                }

                //tạo user kế toán bên webmaster
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore115@gmail.com",
                    FullName = "Bé Yêu Accountant",
                    Email = "hippore115@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore115@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Accountant"); // add vao role accountant
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.Accountant, UserFK = user.Id });
                }

                //tạo user kiểm duyệt quảng cáo
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore116@gmail.com",
                    FullName = "Bé Yêu AdCensor",
                    Email = "hippore116@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore116@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_AdvertiserCensor"); // add vao role 
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.AdCensor, UserFK = user.Id });
                }

                //tạo user Admin bên webmaster
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore117@gmail.com",
                    FullName = "Bé Yêu Admin",
                    Email = "hippore117@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore117@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_Admin"); // add vao role 
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.Admin, UserFK = user.Id });
                }

                //tạo user kiểm duyệt merchant bên webmaster
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore118@gmail.com",
                    FullName = "Bé Yêu MerchantCensor",
                    Email = "hippore118@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Webmaster, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore118@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "WebMaster_MerchantCensor"); // add vao role 
                    _context.WebMasters.Add(new WebMaster() { WebMasterTypeFK = Const_WebmasterType.MerchantCensor, UserFK = user.Id });
                }


                //tạo user customer
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore119@gmail.com",
                    FullName = "Bé Yêu Customer",
                    Email = "hippore119@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore119@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Customer"); // add vao role 
                    _context.Customers.Add(new Customer() {  UserFK = user.Id });
                }

                //tạo user merchant
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore120@gmail.com",
                    FullName = "Bé Yêu Merchant",
                    Email = "hippore120@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Customer, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore120@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Merchant"); // add vao role 
                    _context.Merchants.Add(new Merchant(){ UserFK = user.Id });
                }

                //tạo user advertiser
                result = _userManager.CreateAsync(new User()
                {
                    UserName = "hippore121@gmail.com",
                    FullName = "Bé Yêu Advertiser",
                    Email = "hippore121@gmail.com",
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    UserTypeFK = Const_UserType.Advertiser, //Webmaster
                    Status = Status.Active
                }, CommonConstants.DefaultPW).Result;
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync("hippore121@gmail.com"); // tim user 
                    await _userManager.AddToRoleAsync(user, "Advertiser"); // add vao role 
                    _context.Advertisers.Add(new Advertiser() { UserFK = user.Id });
                }
                #endregion

            }
        }
    }
}
