using AuthModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Sample.Models;
using Sample.DTOs;
using System.Security.Claims;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

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
    ConfigureDbOptions = (options) => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")),
    
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

                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]!)),

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

app.InitAuthModuleDb<User, int>();

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
