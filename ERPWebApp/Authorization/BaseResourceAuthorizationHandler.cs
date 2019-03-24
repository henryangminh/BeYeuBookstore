using BeYeuBookstore.Application.Interfaces.Acc;
using BeYeuBookstore.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeYeuBookstore.Authorization
{
    public class BaseResourceAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        private readonly IRoleService _roleService;

        public BaseResourceAuthorizationHandler(IRoleService roleService)
        {
            _roleService = roleService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, string resource)
        {
            var roles = ((ClaimsIdentity)context.User.Identity).Claims.FirstOrDefault(x => x.Type == CommonConstants.UserClaims.Roles); // get all role
            if (roles != null)
            {
                 var listRole= roles.Value.Split(";");
                // kiểm tra quyền
                var hasPermission = await _roleService.CheckPermission(resource, requirement.Name, listRole);// listRole);
                // test voi giam doc dc full quyen
                if (hasPermission || listRole.Contains(CommonConstants.AppRole.AdminRole) || listRole.Contains(CommonConstants.AppRole.Directors))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
        }
    }
}
