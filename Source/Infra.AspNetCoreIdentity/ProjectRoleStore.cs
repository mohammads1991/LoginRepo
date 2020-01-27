using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Infra.EfCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infra.AspNetCoreIdentity
{
    public class ProjectRoleStore : IQueryableRoleStore<Role>
    {
        private readonly ProjectDbContext _projectDbContext;

        public ProjectRoleStore(ProjectDbContext projectDbContext)
        {
            this._projectDbContext = projectDbContext;
        }

        public IQueryable<Role> Roles => _projectDbContext.Roles.Include(usr=>usr.Users);

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            IdentityResult result;
            try
            {
                await _projectDbContext.Roles.AddAsync(role, cancellationToken);
                await _projectDbContext.SaveChangesAsync(cancellationToken);
                result=IdentityResult.Success;
            }
            catch (Exception e)
            {
                    var error=new IdentityError()
                    {
                        Description = e.Message,
                    };
                    result=IdentityResult.Failed(error);
            }

            return result;
        }

        public Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public async Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = await _projectDbContext.Roles
                .Include(usr=>usr.Users)
                .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken: cancellationToken);
            return await Task.FromResult(role);
        }

        public async Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var role = await _projectDbContext.Roles
                .Include(usr=>usr.Users)
                .FirstOrDefaultAsync(r => r.Id == normalizedRoleName, cancellationToken: cancellationToken);
            return await Task.FromResult(role);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            role.Id = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            role.Id = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            IdentityResult result;
            try
            {
                _projectDbContext.Roles.Update(role);
              await  _projectDbContext.SaveChangesAsync(cancellationToken);
                result=IdentityResult.Success;
            }
            catch (Exception e)
            {
                var error=new IdentityError(){Description = e.Message};
                result=IdentityResult.Failed(error);
            }

            return result;
        }
    }
}
