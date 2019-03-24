using BeYeuBookstore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeYeuBookstore.Helpers
{
    public class CustomClaimsPrincipalFactory: UserClaimsPrincipalFactory<User,Role>
    {
        UserManager<User> _userManger;

        public CustomClaimsPrincipalFactory(UserManager<User> userManager,
            RoleManager<Role> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
            _userManger = userManager;
        }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);
            var roles = await _userManger.GetRolesAsync(user);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim("Email",user.Email),
                new Claim("FullName",user.FullName),
                new Claim("Avatar",user.Avatar??string.Empty),
                new Claim("Role",string.Join(";",roles)),
                new Claim("Id",user.Id.ToString())
            });
            return principal;
        }
    }
}
