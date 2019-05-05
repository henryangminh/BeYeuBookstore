using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeYeuBookstore.Application.Interfaces;
using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels.System;
using BeYeuBookstore.Authorization;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BeYeuBookstore.Controllers
{
    public class AddressBookController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public AddressBookController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, Const_FunctionId.Addressbook, Operations.Read);
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
            var model = await _userService.GetByIdAddressbook(id);

            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPagingAsync(keyword, page, pageSize);
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
				var userid = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.Key);

				if (Guid.TryParse(userid, out var guid))
				{
                    var chk = false;
					userVm.LastupdatedName = _generalFunctionController.Instance.getClaimType(User, CommonConstants.UserClaims.FullName);
					userVm.LastupdatedFk = guid;
					if (userVm.Id == null)
					{
                        chk = await _userService.AddAddressBookAsync(userVm);
					}
					else
					{
                        chk= await _userService.UpdateAddressBookAsync(userVm);
					}
                    
					return new OkObjectResult(chk);
				}
				return new BadRequestResult();
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
        //    if (id != "")
        //    {
        //        _userService.UpdateIsCustomer(id).Wait();
        //    }
        //    return new OkResult();
        //}




        //[HttpGet]
        //public IActionResult GetListFullName(bool status, string type)
        //{
        //    var model = _userService.GetListIdAndFullName(status, type);
        //    model.Wait();
        //    return new OkObjectResult(model.Result);
        //}
       [HttpGet]
       public IActionResult CheckExistEmail(string email)
        {
            // có nên check quyền truy cập không ?
            var result =_authorizationService.AuthorizeAsync(User, Const_FunctionId.Addressbook, Operations.Create); // có quyền tạo
            result.Wait();// chờ cho tiền trình hoàn tất
            
            if (result.Result.Succeeded == false)
                return new OkObjectResult(false); // trả về false để ko cho tạo !

            var model = _userService.checkExistEmail(email);
            return new OkObjectResult(model);
        }
    }
}