using AuthModule.Controllers.Abstract;
using AuthModule.Data;
using AuthModule.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthModule.Controllers;

public class RoleController : Roles<User, int>
{
    public RoleController(AuthDbContxt<User, int> authDbContxt) : base(authDbContxt)
    {
    }
}
