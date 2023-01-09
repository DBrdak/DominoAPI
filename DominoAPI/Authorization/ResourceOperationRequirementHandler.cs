using System.Security.Claims;
using DominoAPI.Entities.Accounts;
using Microsoft.AspNetCore.Authorization;

namespace DominoAPI.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement,
            User user)
        {
            if (requirement.ResourceOperation == ResourceOperation.Create ||
                requirement.ResourceOperation == ResourceOperation.Read)
            {
                context.Succeed(requirement);
            }

            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (user.Id == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}