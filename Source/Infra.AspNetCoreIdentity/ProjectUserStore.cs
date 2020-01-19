using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Infra.EfCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infra.AspNetCoreIdentity
{
    public class ProjectUserStore : IQueryableUserStore<User>,IUserPasswordStore<User>
    {
        private readonly ProjectDbContext _projectDbContext;
        
        public ProjectUserStore(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        public IQueryable<User> Users => _projectDbContext.Users;

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            IdentityResult result;
            try
            {
                await _projectDbContext.Users.AddAsync(user, cancellationToken);

                await _projectDbContext.SaveChangesAsync(cancellationToken);
                result=IdentityResult.Success;
            }
            catch (Exception e)
            {
                var error= new IdentityError()
                {
                    Description = e.Message,
                };
                result=IdentityResult.Failed(error);
            }

            return result;
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            var user = await _projectDbContext.Users.SingleOrDefaultAsync(usr => usr.Id == userId, cancellationToken);
            return await Task.FromResult(user);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await _projectDbContext.Users.SingleOrDefaultAsync(usr =>
                usr.Id == normalizedUserName, cancellationToken: cancellationToken);
            return user;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            
            return Task.FromResult(user.Password);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Id = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
