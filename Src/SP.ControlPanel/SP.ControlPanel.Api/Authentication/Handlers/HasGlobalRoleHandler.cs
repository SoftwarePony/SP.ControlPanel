using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SP.ControlPanel.Api.Authentication.Requirements;

namespace SP.ControlPanel.Api.Authentication.Handlers
{
    public class HasGlobalRoleHandler : AuthorizationHandler<HasGlobalRoleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasGlobalRoleRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "global_roles" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var roles = context.User.FindAll(c => c.Type == "global_roles" && c.Issuer == requirement.Issuer).Select(x => x.Value);

            // Succeed if the scope array contains the required scope
            if (roles.Any(s => s == requirement.Claim))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}