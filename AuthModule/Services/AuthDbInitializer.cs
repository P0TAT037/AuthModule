using AuthModule.Data;
using AuthModule.Data.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthModule.Services
{
    internal class AuthDbInitializer<TUser, TUserId>
        where TUser : class, IUser<TUser, TUserId>
    {
        private readonly AuthDbContxt<TUser, TUserId> _authDbContext;
        private readonly AuthSettings<TUser, TUserId> _authSettings;

        public AuthDbInitializer(AuthDbContxt<TUser, TUserId> authDbContext, AuthSettings<TUser, TUserId> authSettings)
        {
            _authDbContext = authDbContext;
            _authSettings = authSettings;
        }

        public void Run()
        {
            _authDbContext.Database.EnsureCreated();
            _authSettings.AuthDbInitializer?.Invoke(_authDbContext);
        }
    }
}
