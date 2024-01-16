using AuthModule.Controllers.Abstract;
using AuthModule.Data;
using AuthModule.Data.Models;
using AuthModule.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthModule.Controllers
{
    [ApiController]
    public class AuthController : Auth<User, UserDTO, int>
    {
        public AuthController(IMapper mapper, AuthDbContxt<User, int> authDbContxt) : base(mapper, authDbContxt)
        {
        }
    }
}
