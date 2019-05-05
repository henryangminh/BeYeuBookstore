using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, Const_FunctionId.User, Operations.Read);
            //check truy cập
            if (result.Succeeded == false)
                return new RedirectResult("/Home/Index");

            return View();
        }
        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();

            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await _userService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging( string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync( keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(UserViewModel userVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                bool chk = false;
                if (userVm.Id == null)
                {
                   chk= await _userService.AddAsync(userVm);
                }
                else
                {
                   chk= await _userService.UpdateAsync(userVm);
                }
                return new OkObjectResult(chk);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                await _userService.DeleteAsync(id);

                return new OkObjectResult(id);
            }
        }

        //[HttpPost]
        //public IActionResult UpdateIsCustomer(string id)
        //{
        //    if (id !="")
        //    {
        //        _userService.UpdateIsCustomer(id);
        //    }
        //    return new OkResult();
        //}

        //[HttpPost]
        //public IActionResult UpdateIsVendor(string id)
        //{
        //    if (id != "")
        //    {
        //        _userService.UpdateIsVendor(id);
        //    }
          
        //    return new OkResult();
        //}

        //[HttpPost]
        //public IActionResult UpdateIsEmployee(string id)
        //{
        //    if (id != "")
        //    {
        //        _userService.UpdateIsEmployee(id);
        //    }
        //    return new OkResult();
        //}

        //[HttpGet]
        //public IActionResult GetListFullName(bool status, string type)
        //{
        //    var model = _userService.GetListIdAndFullName(status, type);

        //    return new OkObjectResult(model);
        //}

        [HttpGet]
        public IActionResult GetListUserId(string roleName)
        {
            var model = _userService.getListUserId(roleName);
            return new OkObjectResult(model);
        }
    }
}