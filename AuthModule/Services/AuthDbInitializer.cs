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
                    var userTableName = _authDbContext.Model.FindEntityType(typeof(TUser)).GetSchemaQualifiedTableName();
                    _authDbContext.Database.ExecuteSqlRaw($"SELECT 1 From {userTableName}");
                }
                catch
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
