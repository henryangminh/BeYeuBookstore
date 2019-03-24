using ERPWebApp.Data.eEnum;
using ERPWebApp.Data.Entities;
using ERPWebApp.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPWebApp.Data.EF
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
                      new Function() {KeyId = "ManageContactsItem",Name = "Quản lý danh bạ",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-address-book"  },

                      new Function() {KeyId = "AddressBookItem", Name = "Danh bạ chung",ParentId = "ManageContactsItem",SortOrder = 1,Status = Status.Active,URL = "/AddressBook",IconCss = "fa-home"  },
                      new Function() {KeyId = "CustomerItem", Name = "Khách hàng",ParentId = "ManageContactsItem",SortOrder = 2,Status = Status.Active,URL = "/Customer",IconCss = "fa-home"  },
                      new Function() {KeyId = "Vendoritem", Name = "Nhà cung cấp",ParentId = "ManageContactsItem",SortOrder =3,Status = Status.Active,URL = "/Vendor",IconCss = "fa-home"  },
                      new Function() {KeyId = "EmployeeItem", Name = "Nhân viên",ParentId = "ManageContactsItem",SortOrder = 4,Status = Status.Active,URL = "/Employee",IconCss = "fa-home"  },

                      new Function() {KeyId = "PermissionItem", Name = "Phân quyền",ParentId = "ManageContactsItem",SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "RoleItem", Name = "Nhập liệu nhóm quyền",ParentId = "PermissionItem",SortOrder = 1,Status = Status.Active,URL = "/Role",IconCss = "fa-home"  },
                      new Function() {KeyId = "UserRoleItem",Name = "User vào nhóm quyền",ParentId = "PermissionItem",SortOrder = 2,Status = Status.Active,URL = "/User",IconCss = "fa-hom"  },


                      new Function() {KeyId = "ManagerGeneralItem",Name = "Thông tin chung",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-building"  },

                      new Function() {KeyId = "InputDataGeneralItem",Name = "Nhập liệu",ParentId = "ManagerGeneralItem",SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                      new Function() {KeyId = "UnitItem",Name = "Đơn vị tính",ParentId = "InputDataGeneralItem",SortOrder = 1,Status = Status.Active,URL = "/UnitCategory",IconCss = "fa-clone"  },
                      new Function() {KeyId = "WarehouseItem",Name = "Thông tin kho chứa",ParentId = "InputDataGeneralItem",SortOrder = 2,Status = Status.Active,URL = "/Warehouse",IconCss = "fa-table"  },


                      //new Function() {KeyId = "ManagerRetailItem",Name = "Quản lý Bán hàng",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-building"  },

                      //new Function() {KeyId = "InputDataRetailItem",Name = "Nhập liệu",ParentId = "ManagerRetailItem",SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },

                      //new Function() {KeyId = "ProductRetailItem",Name = "Nhập liệu mặt hàng",ParentId = "InputDataRetailItem",SortOrder = 1,Status = Status.Active,URL = "/Product",IconCss = "fa-table"  },
                      //new Function() {KeyId = "InventoryRetailItem",Name = "Quản lý vật tư kho",ParentId = "InputDataRetailItem",SortOrder = 2,Status = Status.Active,URL = "/Inventory",IconCss = "fa-table"  },

                      //new Function() {KeyId = "SalesOrderRetailItem",Name = "Bán hàng",ParentId = "ManagerRetailItem",SortOrder = 2,Status = Status.Active,URL = "/SalesOrder",IconCss = "fa-clone"  },
                      //new Function() {KeyId = "InvoiceRetailItem",Name = "Hóa đơn",ParentId = "ManagerRetailItem",SortOrder = 3,Status = Status.Active,URL = "/Invoice",IconCss = "fa-clone"  },

                      
                      // new Function() {KeyId = "ManagerWoodItem",Name = "Hồng Nghi",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-building"  },

                      //new Function() {KeyId = "InputDataWoodItem",Name = "Nhập liệu",ParentId = "ManagerWoodItem",SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },

                      //new Function() {KeyId = "ProductAttributeWoodItem",Name = "Nhập liệu các thuộc tính của sản phẩm",ParentId = "InputDataWoodItem",SortOrder = 1,Status = Status.Active,URL = "/WoodProductAttribute",IconCss = "fa-table"  },
                      //new Function() {KeyId = "ProductWoodItem",Name = "Nhập liệu sản phẩm",ParentId = "InputDataWoodItem",SortOrder = 2,Status = Status.Active,URL = "/WoodProduct",IconCss = "fa-table"  },
                      //new Function() {KeyId = "InventoryWoodItem",Name = "Quản lý vật tư kho",ParentId = "InputDataWoodItem",SortOrder = 3,Status = Status.Active,URL = "/WoodInventory",IconCss = "fa-table"  },

                      //new Function() {KeyId = "SalesOrderWoodItem",Name = "Bán hàng",ParentId = "ManagerWoodItem",SortOrder = 2,Status = Status.Active,URL = "/WoodSalesOrder",IconCss = "fa-clone"  },
                      //new Function() {KeyId = "InvoiceWoodItem",Name = "Hóa đơn",ParentId = "ManagerWoodItem",SortOrder = 3,Status = Status.Active,URL = "/Invoice",IconCss = "fa-clone"  },


                     new Function() {KeyId = "ManagerConsItem",Name = "Quản lý xây dựng",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-building"  },

                     new Function() {KeyId = "InputDataConsItem",Name = "Nhập liệu",ParentId = "ManagerConsItem",SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },

                      new Function() {KeyId = "ProjectConsItem",Name = "Thông tin công trình",ParentId = "InputDataConsItem",SortOrder = 1,Status = Status.Active,URL = "/Project",IconCss = "fa-clone"  },
                      new Function() {KeyId = "ProductConsItem",Name = "Vật tư, thiết bị",ParentId = "InputDataConsItem",SortOrder = 2,Status = Status.Active,URL = "/ProductCons",IconCss = "fa-table"  },
                      new Function() {KeyId = "BomItem",Name = "Định mức xây dựng",ParentId = "InputDataConsItem",SortOrder = 3,Status = Status.Active,URL = "/Bom",IconCss = "fa-clone"  },
                      new Function() {KeyId = "SynthesisBomItem",Name = "Công tác tổng hợp",ParentId = "InputDataConsItem",SortOrder = 4,Status = Status.Active,URL = "/SynthesisBom",IconCss = "fa-clone"  },

                      new Function() {KeyId=  "SOBoq1SearchItem", Name="Dự thầu, báo giá", ParentId="ManagerConsItem",SortOrder=2,Status= Status.Active,URL="/SalesOrderBOQ1",IconCss="fa-clone"},
                      new Function() {KeyId = "ContractItem",Name = "Hợp đồng với Chủ đầu tư",ParentId = "ManagerConsItem",SortOrder = 3,Status = Status.Active,URL = "/Contract",IconCss = "fa-clone"  },
                      new Function() {KeyId = "SalesOrderBoq2Item",Name = "Tạo BOQ2",ParentId = "ManagerConsItem",SortOrder = 4,Status = Status.Active,URL = "/SalesOrderBoq2",IconCss = "fa-bar-chart-o"  },
                      new Function() {KeyId = "PurchaseOrderItem",Name = "Hợp đồng với thầu phụ",ParentId = "ManagerConsItem",SortOrder = 5,Status = Status.Active,URL = "/PurchaseOrderCons",IconCss = "fa-bar-chart-o"  },


                      new Function() {KeyId = "AcceptanceOfConstructionVolumeItem",Name = "Nghiệm thu khối lượng xây lắp",ParentId = "ManagerBuildItem",SortOrder = 6,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                      new Function() {KeyId = "AcceptanceItem",Name = "Nghiệm thu khối lượng",ParentId = "AcceptanceOfConstructionVolumeItem",SortOrder = 1,Status = Status.Active,URL = "/Acceptance",IconCss = "fa-bar-chart-o"  },
                      new Function() {KeyId = "StatementPaymentItem",Name = "Biên bản thanh toán",ParentId = "AcceptanceOfConstructionVolumeItem",SortOrder = 2,Status = Status.Active,URL = "/StatementPayment",IconCss = "fa-bar-chart-o"  },


                      new Function() {KeyId = "AccountReceivableItem",Name = "Công nợ phải thu",ParentId =null,SortOrder = 7,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                      new Function() {KeyId = "CONTACT",Name = "Theo dõi thông báo thanh toán",ParentId = "AccountReceivableItem",SortOrder = 1,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },


                      new Function() {KeyId = "HumanResourcesItem",Name = "Quản lý nhân sự",ParentId = null,SortOrder = 8,Status = Status.Active,URL = "/",IconCss = "fa-users"  },
                      new Function() {KeyId = "ScoreEmployeeItem",Name = "Chấm công nhân viên",ParentId = "HumanResourcesItem",SortOrder = 1,Status = Status.Active,URL = "/ScoreEmployee",IconCss = "fa-comments"  },
                      new Function() {KeyId = "AllowedVacationItem",Name = "Quản lý nghỉ phép",ParentId = "HumanResourcesItem",SortOrder = 1,Status = Status.Active,URL = "/AllowedVacation",IconCss = "fa-comments"  },
                      new Function() {KeyId = "BussinessApplicationItem",Name = "Quản lý công tác",ParentId = "HumanResourcesItem",SortOrder = 1,Status = Status.Active,URL = "/BussinessApplication",IconCss = "fa-comments"  },
                      new Function() {KeyId = "SalaryCalculatingItem",Name = "Tính lương",ParentId = "HumanResourcesItem",SortOrder = 1,Status = Status.Active,URL = "/SalaryCalculating",IconCss = "fa-comments"  },
                      new Function() {KeyId = "PermitDayItem",Name = "Phép năm",ParentId = "HumanResourcesItem",SortOrder = 1,Status = Status.Active,URL = "/PermitDay",IconCss = "fa-comments"  },
                      


                      new Function() {KeyId = "FeedbackItem",Name = "Phản hồi",ParentId = null,SortOrder = 9,Status = Status.Active,URL = "/",IconCss = "fa-comments"  },
                      new Function() {KeyId = "FeedbackChildItem",Name = "Phản hồi",ParentId = "FeedbackItem",SortOrder = 1,Status = Status.Active,URL = "/FeedBack",IconCss = "fa-comments"  },

                      new Function() {KeyId = "SettingItem",Name = "Cài đặt",ParentId = null,SortOrder = 10,Status = Status.Active,URL = "/",IconCss = "fa-cog"  },
                      new Function() {KeyId = "SystemParameterItem",Name = "Thông số hệ thống",ParentId = "SettingItem",SortOrder = 1,Status = Status.Active,URL = "/SystemParameter",IconCss = "fa-comments"  },
                      new Function() {KeyId = "IncomeTaxListItem",Name = "Danh mục thuế thu nhập",ParentId = "SettingItem",SortOrder = 1,Status = Status.Active,URL = "/IncomeTaxList",IconCss = "fa-comments"  },
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
            if (!_roleManager.Roles.Any())
            {
                var _item1 = new Role()
                {
                    Name = "BoardOfDirectors",
                    NormalizedName = "Ban Giám Đốc",
                    Description = "Bao gồm giám đốc và các cổ động"
                };
                await _roleManager.CreateAsync(_item1);
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "HeadOfProcurementDepartment",
                    NormalizedName = "Trưởng phòng Đấu thầu",
                    Description = "Quản lý phòng đấu thầu, trình lên các bản dự thầu báo giá của công trình."
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "ProcurementDepartment",
                    NormalizedName = "Nhân viên phòng đấu thầu",
                    Description = "Nhân viên thuộc phòng đấu thầu được giao các dự án để làm dự thầu báo giá"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "HeadOfFinanceAndAccounting",
                    NormalizedName = "Trưởng phòng tài chính kế toán",
                    Description = "Chịu trách nhiệm quản lý tài chính của công ty"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "FinanceAndAccounting",
                    NormalizedName = "Nhân viên phòng tài chính kế toán",
                    Description = "chưa điền"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "HeadOfConstruction",
                    NormalizedName = "Trưởng phòng thi công",
                    Description = "Chịu trách nhiệm tạo, BOQ thi công cho công trình, tìm kiếm thầu phụ và quản lý giám sát..."
                });


                await _roleManager.CreateAsync(new Role()
                {
                    Name = "CaptainOfConstruction",
                    NormalizedName = "Chỉ huy trưởng",
                    Description = "Quản lý giám sát các giám sát viên."
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "SupervisorOfConstruction",
                    NormalizedName = "Gián sát viên",
                    Description = "Giám sát các công trình được giao, tìm thầu phụ...."
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "HeadOfHumanResoucesDepartment",
                    NormalizedName = "Trưởng phòng hành chính nhân sự",
                    Description = "Quản lý nhân viên của công ty"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "HumanResoucesDepartment",
                    NormalizedName = "Hành chính nhân sự",
                    Description = "Quản lý nhân viên của công ty"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "MaterialsDepartment",
                    NormalizedName = "Phòng vật tư",
                    Description = "Quản lý vật tư của công ty"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "BusinessDepartment",
                    NormalizedName = "Phòng kinh doanh",
                    Description = "Quản lý kinh doanh của công ty"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "SystemAdmin",
                    NormalizedName = "Quản trị hệ thống",
                    Description = "Quản lý hệ thống của công ty"
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "test",
                    NormalizedName = "nhóm test ",
                    Description = "full chức năng tiện cho qua trình test. sẽ bị xóa trước khi chạy thực tế."
                });
                await _roleManager.CreateAsync(new Role()
                {
                    Name = "General",
                    NormalizedName = "nhóm General ",
                    Description = "Những chức năng chung cho tất cả (nếu có)."
                });
            }

            ////tao user admin (pass word phai lon hon 6 ktu
            //if (_userManager.Users.Count() == 0)
            //{

            //    var result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "thuongnv@gmail.com",
            //        FullName = "Nguyễn Văn Thưởng",
            //        Email = "thuongnv@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("thuongnv@gmail.com"); // tim user admin
            //        _userManager.AddToRoleAsync(user, "BoardOfDirectors").Wait(); // add admin vao role admin
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00001", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });
            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "hann@gmail.com",
            //        FullName = "Nguyễn Ngọc Hà",
            //        Email = "hann@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("hann@gmail.com"); // tim user 
            //        await _userManager.AddToRoleAsync(user, "HeadOfFinanceAndAccounting");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00002", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });
            //    }
            //    // add vào trưởng phòng tài chính kế  toán

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "xuannh@gmail.com",
            //        FullName = "Nguyễn Hoàng Xuân",
            //        Email = "xuannh@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("xuannh@gmail.com"); // tim user 
            //        await _userManager.AddToRoleAsync(user, "HeadOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00003", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });
            //    }
            //    // add admin vao trưởng phòng thi công

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "saotd@gmail.com",
            //        FullName = "Trần Đình Sào",
            //        Email = "saotd@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    // add admin vao role chỉ huy trưởng
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("saotd@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "CaptainOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00004", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "nhutnv@gmail.com",
            //        FullName = "Nguyễn Văn Nhứt",
            //        Email = "nhutnv@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("nhutnv@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "CaptainOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00005", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "congdq@gmail.com",
            //        FullName = "D Q Công",
            //        Email = "congdq@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    // add admin vao role giám sát
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("congdq@gmail.com"); // tim user 
            //        await _userManager.AddToRoleAsync(user, "SupervisorOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00006", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "vuongtd@gmail.com",
            //        FullName = "Trần Đình Vương",
            //        Email = "vuongtd@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    // add admin vao role
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("vuongtd@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "SupervisorOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00007", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "dungnt@gmail.com",
            //        FullName = "Nguyễn Thi Dung",
            //        Email = "dungnt@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    // add admin vao role 
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("dungnt@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "SupervisorOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00008", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "nhannt@gmail.com",
            //        FullName = "Nguyễn Thành Nhân",
            //        Email = "nhannt@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsCustomer = true,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    // add admin vao role admin
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("nhannt@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "SupervisorOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00009", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });
            //        _context.SoCustomer.Add(new SoCustomer() { Id = "C00001", UserFk = user.Id, CreditLimit = 100000, CustomerType = 1, QtyContructions = 2, Status = Status.Active });
            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "thuanvt@gmail.com",
            //        FullName = "Võ Tấn Thuận",
            //        Email = "thuanvt@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("thuanvt@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "SupervisorOfConstruction");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00010", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }
            //    // add admin vao role admin

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "dunghtt@gmail.com",
            //        FullName = "Huỳnh Thị Thùy Dung",
            //        Email = "dunghtt@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    // add admin vao role phòng tài chính kế toán
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("dunghtt@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "FinanceAndAccounting");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00011", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }

            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "admin@gmail.com",
            //        FullName = "admin",
            //        Email = "admin@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("admin@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "SystemAdmin");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00012", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }


            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "test@gmail.com",
            //        FullName = "test",
            //        Email = "test@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("test@gmail.com"); // tim user admin
            //        await _userManager.AddToRoleAsync(user, "test");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00013", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }


            //    result = _userManager.CreateAsync(new User()
            //    {
            //        UserName = "hoangdd@gmail.com",
            //        FullName = "Đào Duy Hoàng",
            //        Email = "hoangdd@gmail.com",
            //        DateCreated = DateTime.Now,
            //        DateModified = DateTime.Now,
            //        IsEmployee = true,
            //        Status = Status.Active
            //    }, CommonConstants.DefaultPW).Result;
            //    if (result.Succeeded)
            //    {
            //        var user = await _userManager.FindByNameAsync("hoangdd@gmail.com"); // tim user 
            //        await _userManager.AddToRoleAsync(user, "HeadOfProcurementDepartment");
            //        _context.HpEmployee.Add(new HpEmployee() { Id = "P00014", User_FK = user.Id, Department_FK = 5, PositionFk = 5, Status = Status.Active });

            //    }
            //}

            
        }
    }
}
