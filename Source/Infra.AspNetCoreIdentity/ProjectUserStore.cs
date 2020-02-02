using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Infra.EfCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infra.AspNetCoreIdentity
{
    public class ProjectUserStore : IQueryableUserStore<User>, IUserPasswordStore<User>, IUserClaimStore<User>,IUserEmailStore<User>
    {
        private readonly ProjectDbContext _projectDbContext;

        public ProjectUserStore(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        public IQueryable<User> Users => _projectDbContext.Users;

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            IdentityResult result;
            try
            {
                await _projectDbContext.Users.AddAsync(user, cancellationToken);

                await _projectDbContext.SaveChangesAsync(cancellationToken);
                result = IdentityResult.Success;
            }
            catch (Exception e)
            {
                var error = new IdentityError()
                {
                    Description = e.Message,
                };
                result = IdentityResult.Failed(error);
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
            var user = await _projectDbContext.Users
                .Include(usr => usr.Claims)
                .FirstOrDefaultAsync(usr => usr.Id == userId, cancellationToken);
            return await Task.FromResult(user);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            var user = await _projectDbContext.Users
                .Include(usr => usr.Claims)
                .SingleOrDefaultAsync(usr =>
                usr.Id == normalizedUserName, cancellationToken: cancellationToken);
            return user;
        }

        public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
        {
            throw new NotImplementedException();
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
            return Task.FromResult<bool>(true);
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
            IdentityResult result=IdentityResult.Success;

            return Task.FromResult(result);
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            var claim = user.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
            return Task.FromResult((IList<Claim>)claim);
        }

        public async Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                var appClaim = new AppClaim() { Type = claim.Type, Value = claim.Value };
                user.Claims.Add(appClaim);
            }

            _projectDbContext.Users.Update(user);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            await RemoveClaimsAsync(user, new List<Claim> { claim }, cancellationToken);
            await AddClaimsAsync(user, new List<Claim> { newClaim }, cancellationToken);
            _projectDbContext.Users.Update(user);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            
        }

        public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var removeCandidate =
                from claim in claims
                join appClaim in user.Claims on new { claim.Type, claim.Value } equals new { appClaim.Type, appClaim.Value }
                select appClaim;
            foreach (var item in removeCandidate)
            {
                user.Claims.Remove(item);
            }

            _projectDbContext.Users.Update(user);
            await _projectDbContext.SaveChangesAsync(cancellationToken);


        }

        public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var users = await _projectDbContext.Users
                .Include(usr=>usr.Claims)
                .Where(usr => usr.Claims.Any(clm => clm.Type == claim.Type && clm.Value == claim.Value))
                .ToListAsync(cancellationToken: cancellationToken);
            return users;

        }


        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            user.Id = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            var email = user.Id;
            return Task.FromResult(email);
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;


        }

        public async Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            var user = await _projectDbContext.Users
                .Include(usr => usr.Claims)
                .SingleOrDefaultAsync(usr =>
                usr.Id == normalizedEmail, cancellationToken: cancellationToken);
            return user;
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            
            return Task.FromResult(user.Id);
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.Id = normalizedEmail;
            return Task.CompletedTask;
        }
    }
}
