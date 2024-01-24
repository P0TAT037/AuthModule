using AuthModule.Controllers.Abstract;
using AuthModule.Data;
using AuthModule.Data.Models;
using AuthModule.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthModule.Controllers;

public class ClaimController : Claims<User, int>
{
    public ClaimController(AuthDbContxt<User, int> authDbContxt) : base(authDbContxt)
    {
    }

}
