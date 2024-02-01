using AuthModule.Security.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthModule.Security.Handlesr
{
    internal class SatisfiedRequirementHandler : AuthorizationHandler<SatisfiedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SatisfiedRequirement requirement)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
