using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Plugins
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public MyUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,IOptions<IdentityOptions> optionsAccessor): base(userManager, optionsAccessor)
        {

        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            //identity.AddClaim(new Claim("ContactName", user.ContactName ?? "[Click to edit profile]"));
            identity.AddClaim(new Claim("UserIdentity", user.UserIdentity??"login"));
            return identity;
        }
    }
}
