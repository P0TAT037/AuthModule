using AuthModule.Controllers;
using AuthModule.Data.Models.Abstract;
using AuthModule.DTOs.Abstract;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuthModule;

internal class GenericControllerFeatureProvider<TUser, TUserRegistrationDto, TUserId> : IApplicationFeatureProvider<ControllerFeature>
        where TUser : class, IUser<TUser, TUserId>
        where TUserRegistrationDto : class, IUserDto
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        List<Type> controllerTypes = 
        [
            typeof(AuthController<TUser, TUserRegistrationDto, TUserId>), 
            typeof(ClaimsController<TUser, TUserId>), 
            typeof(RolesController<TUser, TUserId>)
        ];

        // This is designed to run after the default ControllerTypeProvider, 
        // so the list of 'real' controllers has already been populated.
        foreach (var controllerType in controllerTypes)
        {
            var typeName = controllerType.Name;
            if (!feature.Controllers.Any(t => t.Name == typeName))
            {
                // There's no 'real' controller for this entity, so add the generic version.
                feature.Controllers.Add(controllerType.GetTypeInfo());
            }
        }
    }
}
