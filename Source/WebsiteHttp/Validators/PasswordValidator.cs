using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace WebsiteHttp.Validators
{
    public class PasswordValidator : IPasswordValidator<User>
    {
        private readonly IOptions<IdentityOptions> _options;

        public PasswordValidator(IOptions<IdentityOptions> options)
        {
            _options = options;
        }
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            IdentityResult result;
            if (password.Length >= _options.Value.Password.RequiredLength)
            {
                result=IdentityResult.Success;
            }
            else
            {
                var error=new IdentityError(){Description = $"Password Length Must Be Over {_options.Value.Password.RequiredLength} Characters"};
                result=IdentityResult.Failed(error);
            }

            return Task.FromResult(result);
        }
    }
}
