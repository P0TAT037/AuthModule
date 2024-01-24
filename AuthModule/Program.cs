using AuthModule;
using AuthModule.Data;
using AuthModule.Data.Models;
using AuthModule.Services;
using AuthModule.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authSettings = new AuthSettings
{
    UseJWT = true,
    JwtTokenSettings = new()
    {
        SecurityAlgorithm = SecurityAlgorithms.HmacSha256,
        Expiration = TimeSpan.FromHours(1),
        TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateActor = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            
            ValidIssuer = "elbatates",
            ValidAudience = "3oshaqElBatates",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HrafCOb3jt045IBZn1Z6RPUAxDkavf_INZzE9BwN3I0cQzuElDShtNCSXub5Ef7JazFot3iCJ3UBpIbIrHbtzA")),
        }
    }
};

builder.Services.AddSingleton(authSettings);

builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddDbContext<AuthDbContxt<User, int>>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=AuthModule;User Id=postgres; Password=superuser"));
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options=> 
    {
        options.TokenValidationParameters = authSettings.JwtTokenSettings.TokenValidationParameters;
    })
    .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            string authorization = context.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                return JwtBearerDefaults.AuthenticationScheme;

            return CookieAuthenticationDefaults.AuthenticationScheme;
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
