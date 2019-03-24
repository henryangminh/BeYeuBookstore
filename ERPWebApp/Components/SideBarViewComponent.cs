using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Application.ViewModels.Acc;
using BeYeuBookstore.Extensions;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeYeuBookstore.Components
{
    public class SideBarViewComponent:ViewComponent
    {
        private IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Role");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAll(string.Empty);
            }
            else
            {
                //TODO: Get by permission
                // viết 1 hàm truyền vào list role trả về những function nào được truy xuất từ roles đó.
                // lấy con thì phải lấy cha nó để hiểu thị hehe // thử nghiệm lấy con thôi ?
                functions =  _functionService.GetAllByRoles(roles);
            }
            return View(functions);
        }
    }
}
