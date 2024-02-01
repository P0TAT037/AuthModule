using AuthModule.AutoMapperProfiles;
using AuthModule.Data;
using AuthModule.Data.Models.Abstract;
using AuthModule.DTOs.Abstract;
using AuthModule.Security;
using AuthModule.Security.Handlesr;
using AuthModule.Security.Requirements;
using AuthModule.Services;
using AuthModule.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace AuthModule;

public static class Extensions
{
    public static AuthenticationBuilder AddAuthModule<TUser, TUserRegistrationDto, TUserId>(this IServiceCollection services, AuthSettings<TUser, TUserId> authSettings)
        where TUser : class, IUser<TUser, TUserId>
        where TUserRegistrationDto : class, IUserDto
    {

        var defaultAuthScheme = authSettings.UseCookies? CookieAuthenticationDefaults.AuthenticationScheme: JwtBearerDefaults.AuthenticationScheme;

        services.AddSingleton(authSettings.JwtTokenSettings!);

        services.AddSingleton(authSettings);

        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddSingleton<IAuthorizationHandler, SatisfiedRequirementHandler>();

        services.AddDbContext<AuthDbContxt<TUser, TUserId>>(options => authSettings.ConfigureDbOptions(options));

        services.AddTransient<AuthDbInitializer<TUser, TUserId>>();

        services.AddAutoMapper(options => options.AddProfile(typeof(DefaultProfile<TUser, TUserRegistrationDto>)));

        services.AddControllers()
            .PartManager.FeatureProviders.Add(new GenericControllerFeatureProvider<TUser, TUserRegistrationDto, TUserId>());
        
        services.AddAuthorization(opt => opt.AddPolicy(AuthModuleConstValues.AdminPolicy, builder =>
        {
            builder.AddRequirements(new SatisfiedRequirement());
        }));
        
        var autheBuilder = services.AddAuthentication("JWT_OR_COOKIE")
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o=>authSettings.CookieSettings!.ConfigureCookieAuthenticationOptions(o))
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o => authSettings.JwtTokenSettings!.ConfigureJwtBearerOptions(o))
            .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string authorization = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        return JwtBearerDefaults.AuthenticationScheme;

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

        return autheBuilder;

    }

    public static WebApplication? InitAuthModuleDb<TUser, TUserId>(this WebApplication app)
        where TUser : class, IUser<TUser, TUserId>
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<AuthDbInitializer<TUser, TUserId>>();
        initializer.Run();
        return app;
    }
}
