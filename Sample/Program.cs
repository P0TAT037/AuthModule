using AuthModule;
using AuthModule.Data;
using AuthModule.Data.Models;
using AuthModule.DTOs;
using AuthModule.Services;
using AuthModule.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using Sample.Models;
using Sample.DTOs;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var authSettings = new AuthSettings<User, int>
{
    //UseCookies = true,
    ConfigureDbOptions = (options) => options.UseNpgsql("Server=localhost;Port=5432;Database=AuthModule;User Id=postgres; Password=superuser"),
    JwtTokenSettings = new()
    {

        SecurityAlgorithm = SecurityAlgorithms.HmacSha256,
        Expiration = TimeSpan.FromHours(1),
        ConfigOptions = (o) =>
        {
            o.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateActor = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,

                ValidIssuer = "elbatates",
                ValidAudience = "3oshaqElBatates",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"]!)),

            };

        }
        
    }
};

authSettings
    .AddUserInfoClaim(nameof(User.Id))
    .AddUserInfoClaim(nameof(User.Handle));

builder.Services.AddAuthModule<User, UserDTO, int>(authSettings);

builder.Services.AddAuthorization(opt => opt.AddPolicy(AuthModuleConstValues.AdminPolicy, b =>
{
    b.RequireClaim(ClaimTypes.NameIdentifier);
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.InitAuthModuleDb<User, int>();

app.Use(async (context, _next) =>
{
    try
    {
        await _next(context);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
