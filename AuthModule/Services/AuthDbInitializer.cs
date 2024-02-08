using AuthModule.Data;
using AuthModule.Data.Models.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
            bool newDbCreated = _authDbContext.Database.EnsureCreated();
            
            if (!newDbCreated)
            {
                try 
                {
                    _authDbContext.Users.FirstOrDefault(x => x.Handle == x.Handle); // making sure the table exists
                }
                catch // the tables don't exist so we need to create them
                {
                    string script = _authDbContext.Database.GenerateCreateScript();
                    
                    string commandText = script.Replace("\r\nGO\r\n", "\n");  // removes the "GO" commands if sqlserver is used
                    
                    _authDbContext.Database.ExecuteSqlRaw(commandText);
                    
                }
            }
            
            _authSettings.AuthDbInitializer?.Invoke(_authDbContext);
        }
    }
}
