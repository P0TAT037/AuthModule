using AuthModule.Controllers.Abstract;
using AuthModule.Data;
using AuthModule.Data.Models;
using AuthModule.DTOs;
using AuthModule.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthModule.Controllers;

public class AuthController : Auth<User, UserDTO, int>
{
    public AuthController(IMapper mapper, AuthDbContxt<User, int> authDbContxt, ITokenService tokenService) : 
        base(mapper, authDbContxt, tokenService)
    {
    }
}
