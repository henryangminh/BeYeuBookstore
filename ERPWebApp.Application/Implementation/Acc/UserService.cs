using AutoMapper;
using AutoMapper.QueryableExtensions;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Data.Enums;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.IRepositories;
using BeYeuBookstore.Infrastructure.Interfaces;
using BeYeuBookstore.Utilities.Constants;
using BeYeuBookstore.Utilities.DTOs;
using BeYeuBookstore.Utilities.GeneralFunction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeYeuBookstore.Application.Implementation.Acc
{
    public class UserService : IUserService
    {

        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager, IUserRolesRepository userRolesRepository, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRolesRepository = userRolesRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GetFullName(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user != null)
                return user.FullName;
            return "";
        }
        public async Task<bool> AddAsync(UserViewModel userVm)
        {
            var user = new User()
            {
                UserName = userVm.UserName,
                Avatar = userVm.Avatar,
                Email = userVm.Email,
                FullName = userVm.FullName,
                DateCreated = DateTime.Now,
                PhoneNumber = userVm.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userVm.Password);
            if (result.Succeeded && userVm.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                    await _userManager.AddToRolesAsync(appUser, userVm.Roles);

            }
            return true;
        }
        public async Task<bool> AddAddressBookAsync(UserViewModel userVm)
        {
            var user = Mapper.Map<UserViewModel, User>(userVm);
            user.UserName = userVm.Email;
            user.DateCreated = DateTime.Now;
            user.DateModified = DateTime.Now;
            var result = await _userManager.CreateAsync(user, CommonConstants.DefaultPW);
            return result.Succeeded;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<UserViewModel>> GetAllAsync()
        {
            return await _userManager.Users.ProjectTo<UserViewModel>().ToListAsync();
        }

        public PagedResult<UserViewModel> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrEmpty(keyword))
            {
                var keysearch = keyword.Trim().ToUpper();
                query = query.Where(x => (x.FullName+ x.UserName + x.Email).ToUpper().Contains(keysearch));
            }
            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize)
                .Include(x => x.UserTypeFKNavigation)
               .Take(pageSize);
            var data = query.Select(x => new UserViewModel()
            {

                UserName = x.UserName,
                Avatar = x.Avatar,
                Email = x.Email,
                FullName = x.FullName,
                IdNumber = x.IdNumber,
                Gender = x.Gender,
                Dob = x.Dob,
                Id = x.Id,
                PhoneNumber = x.PhoneNumber,
                Status = x.Status,
                DateModified = x.DateModified,
                UserTypeFK = x.UserTypeFK,
                UserTypeName = x.UserTypeFKNavigation.UserTypeName,
            })
            .ToList();
            var paginationSet = new PagedResult<UserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<UserViewModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = Mapper.Map<User, UserViewModel>(user);
            userVm.Roles = roles.ToList();
            return userVm;
        }
        public async Task<UserViewModel> GetByIdAddressbook(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var userVm = Mapper.Map<User, UserViewModel>(user);
            return userVm;
        }
        public async Task<bool> UpdateAsync(UserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            if (user != null)
            {
                //get current roles in db
                var currentRoles = await _userManager.GetRolesAsync(user);

                var needAddRoles = userVm.Roles.Except(currentRoles).ToArray();
                var needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();

                //remove roles
                if (needRemoveRoles.Length > 0)
                {
                    List<Guid> listDel = new List<Guid>();
                    foreach (var itemdel in needRemoveRoles)
                    {
                        var rolesId = await _roleManager.FindByNameAsync(itemdel);
                        if (rolesId != null)
                        {
                            listDel.Add(rolesId.Id);
                        }
                    }
                    _userRolesRepository.RemoveMultiple(listDel, user.Id);
                    if (!_unitOfWork.Commit()) // xóa bị lỗi
                        return false;
                }
                // add roles new
                if (needAddRoles.Length > 0)
                {
                    var result1 = await _userManager.AddToRolesAsync(user, needAddRoles);
                    if (!result1.Succeeded)
                        return false;
                }

                //Update user detail
                user.FullName = userVm.FullName;
                user.Status = userVm.Status;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                user.Avatar = userVm.Avatar;
                var result = await _userManager.UpdateAsync(user);

                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> UpdateByCustomer(UserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            if (user != null)
            {
                user.FullName = userVm.FullName;
                user.Address = userVm.Address;
                user.PhoneNumber = userVm.PhoneNumber;
                user.Gender = userVm.Gender;
                user.Status = userVm.Status;
                var result = await _userManager.UpdateAsync(user);

                return result.Succeeded;
            }
                return false;
        }

        public async Task<bool> UpdateAddressBookAsync(UserViewModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            /*
            if (user!=null)
            {
                //Update user 
                user.Dob = userVm.Dob;
                user.IdNumber = userVm.IdNumber;
                user.FullName = userVm.FullName;
                user.Avatar = userVm.Avatar;
                user.IdDate = userVm.IdDate;
                user.PhoneNumber = userVm.PhoneNumber;
                if (user.Email != userVm.Email)
                {
                    user.UserName = userVm.Email;
                    user.Email = userVm.Email;
                }
                user.TaxIdnumber = userVm.TaxIdnumber;
                user.Street = userVm.Street;
                user.Ward = userVm.Ward;
                user.District = userVm.District;
                user.City = userVm.City;
                user.Country = userVm.Country;
                user.Origin_FK = userVm.Origin_FK;
                user.Gender = userVm.Gender;
                user.Fax = userVm.Fax;
                user.Notes = userVm.Notes;
                // không update
                //user.IsCustomer = userVm.IsCustomer;
                //user.IsVendor = userVm.IsVendor;
                //user.IsEmployee = userVm.IsEmployee;
                // user.IsPersonal = userVm.IsPersonal;
                user.DateModified = DateTime.Now;

               // user.Status = userVm.Status;
                user.LastupdatedFk = userVm.LastupdatedFk;
                user.LastupdatedName = userVm.LastupdatedName;
               var kt= await _userManager.UpdateAsync(user);
                return kt.Succeeded;
            }
            */
            return false;
        }

        //public async Task<List<IdAndName>> GetListIdAndFullName(bool status, string type)
        //{
        //    throw new Exception();
        //    /*
        //    var query = _userManager.Users.AsNoTracking();
        //    if(type == const_AddressbookType.Customer)
        //        query= query.Where(x => x.IsCustomer == status);
        //    else
        //         if (type == const_AddressbookType.Employee)
        //        query= query.Where(x => x.IsEmployee == status);
        //    else
        //        query= query.Where(x => x.IsVendor == status);

        //    return await query.Select(p => new IdAndName
        //    {
               
        //        sId = p.Id.ToString(),
        //        Name = p.FullName
        //    }
        //     ).ToListAsync();
        //     */
        //}

        private string createUserName(string FullName)
        {
            FullName = _convertFunction.Instance.RemoveSign4VietnameseString(FullName);
            FullName = FullName.Trim().Replace("  ", " ");
            var _userName = "";
            if (FullName != "")
            {
                var listItem = FullName.Split(' ');
                var len = listItem.Count();
               
                for (int i = 0; i < len - 1; i++)
                    if(listItem[i]!="")
                {
                    _userName += listItem[i][0];
                }
                _userName = listItem[len-1][0] + _userName;
            }
            return _userName.ToLower();
        }

        public async  Task<List<string>> GetAllRolesById(string Id)
        {
            var user =await _userManager.FindByIdAsync(Id.ToString());
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
            }
            else
                return new List<string>();
        }

        public async Task<List<Guid>> getListUserId(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            if (users.Count > 0)
            {
                return users.Select(p => p.Id).ToList();
            }
            else
                return new List<Guid>();
        }

        public bool checkUserInRole(string userid, string roleName)
        {
            var user = _userManager.FindByIdAsync(userid);
            user.Wait();
            var kt = _userManager.IsInRoleAsync(user.Result, roleName);
            kt.Wait();
            if (kt.Result)
                return true;
            return false;

        }

        public bool checkExistEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email);
            user.Wait();
            return user.Result == null;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
