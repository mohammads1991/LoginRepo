using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Infra.EfCore;
using Microsoft.AspNetCore.Identity;

namespace Infra.AspNetCoreIdentity
{
   public class ClaimFactory:IUserClaimsPrincipalFactory<User>
    {
        private readonly UserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        private readonly ProjectDbContext _projectDbContext;

        public ClaimFactory(UserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,ProjectDbContext projectDbContext)
        {
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _projectDbContext = projectDbContext;
        }
        public async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var defaultPrincipal =await _userClaimsPrincipalFactory.CreateAsync(user);
            var identity = defaultPrincipal.Identities.Single();
            var role = _projectDbContext.Roles.SingleOrDefault(r => r.Users.Any(u => u.Id == user.Id));
            if (role!=null)
            {
                var roleClaim=new Claim(ClaimTypes.Role,role.Id);
                identity.AddClaim(roleClaim);
            }

            return defaultPrincipal;

        }
    }
}
