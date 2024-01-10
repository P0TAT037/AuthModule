using AuthModule.Data;
using AuthModule.Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AuthDbContxt<User, int>>(options => options.UseNpgsql("Server=localhost;Port=5432;Database=AuthModule;User Id=postgres; Password=superuser"));
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
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
