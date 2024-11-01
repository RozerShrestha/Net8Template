using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using BusinessManagementSystem.Services;
using Azure;
using System.Security.Claims;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Dto;

namespace BusinessManagementSystem.Helper
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected UserDto userDto;
        public RolesAuthorizationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
            userDto = new UserDto();
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var validRole = false;
            var loggedInEmail = context.User.Identities.First().Claims.ToList().FirstOrDefault(x => x.Type.Contains("emailaddress", StringComparison.OrdinalIgnoreCase)).Value;
            var loggedInUserName = loggedInEmail.Split("@")[0];
            var userInfo = _unitOfWork.Users.GetFirstOrDefault(p => p.Email == loggedInEmail, includeProperties: "UserRoles");


            userDto = _unitOfWork.Base.UserDetail(loggedInEmail);

            //if (requirement.AllowedRoles == null || requirement.AllowedRoles.Any() == false)
            //{
            //    validRole = true;
            //}
            //else
            
            validRole = requirement.AllowedRoles.Contains(userDto.RoleName) ? true : false;

            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
