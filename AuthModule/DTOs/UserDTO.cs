﻿using AuthModule.DTOs.Abstract;

namespace AuthModule.DTOs
{
    public class UserDTO : IUserDto
    {
        public string Handle { get; set; }
        public string Password { get; set; }
    }
}