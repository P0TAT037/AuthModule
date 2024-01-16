using AuthModule.Data;
using AuthModule.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using AuthModule.Data.Models.Abstract;
using AuthModule.DTOs.Abstract;
using AuthModule.Services.Interfaces;
using System.Security.Claims;

namespace AuthModule.Controllers.Abstract
{
    [ApiController]
    [Route("api/auth")]
    public class Auth<TUser, TUserDto, TUserId> : ControllerBase
        where TUser : class, IUser<TUser, TUserId>
        where TUserDto : class, IUserDto
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly AuthDbContxt<TUser, TUserId> _authDbContxt;

        public Auth(IMapper mapper, AuthDbContxt<TUser, TUserId> authDbContxt, ITokenService tokenService)
        {
            _mapper = mapper;
            _authDbContxt = authDbContxt;
            _tokenService = tokenService;
        }


        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp(TUserDto user)
        {
            user.Password  = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var u = _mapper.Map<TUser>(user);
            _authDbContxt.Users.Add(u);
            await _authDbContxt.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Login([FromForm] UserDTO userDto)
        {
            TUser user;
            
            try
            {
                user = _authDbContxt.Users.Single(x => x.Handle == userDto.Handle);

                if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                    throw new InvalidOperationException("wrong password");
            }
            catch (InvalidOperationException)
            {
                return Unauthorized("wrong username or password");
            }
            /* do this somehow >>>
            var allclaims = user.Claims.Concat(User.Roles);
            var token = _tokenService.GetTokenString(allClaims);
            (make Role convertable to Claim and our Claim model Claim convertable to ClaimsPrinciple Claim)
            */
            return Ok(token);
        }

    }
}
