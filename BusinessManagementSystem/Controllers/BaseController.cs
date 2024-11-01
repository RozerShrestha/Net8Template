 using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Numerics;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize]
    public class BaseController:Controller
    {
        protected readonly IBusinessLayer _businessLayer;
        protected readonly INotyfService _notyf;
        protected readonly IEmailSender _emailSender;
        public static int roleId;
        public static int userId;
        public static string roleName = string.Empty;
        public static string username = string.Empty;
        public static string email = string.Empty;
        public static string fullName = string.Empty;
        public static string PhoneNumber = string.Empty;
        protected UserDto userDto;
        JavaScriptEncoder _javaScriptEncoder;

        
        public BaseController(IBusinessLayer businessLayer, INotyfService notyf, IEmailSender emailSender, JavaScriptEncoder javaScriptEncoder)
        {
            _notyf = notyf;
            _emailSender= emailSender;
            _businessLayer = businessLayer;
            this.userDto = new UserDto();
            _javaScriptEncoder = javaScriptEncoder;
        }
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            ViewData["UserDetail"] = UserDetail();
            ViewData["Menu"] = MenuList();
            ViewData["Title"] = _businessLayer.BasicConfigurationService.GetBasicConfig().Data.ApplicationTitle;
        }
         protected IActionResult HandleError(Exception ex)
        {
            return StatusCode(500, new {message= ex.Message });
        }
        private UserDto UserDetail()
        {
            try
            {
                var claims = User.Identities.First().Claims;
                var loggedInEmail = claims.FirstOrDefault(x => x.Type.Contains("emailaddress", StringComparison.OrdinalIgnoreCase)).Value;
                var loggedInUserName = loggedInEmail.Split("@")[0].Trim();

                userDto = _businessLayer.BaseService.UserDetail(loggedInEmail);
                userId = userDto.UserId;
                username = userDto.UserName;
                email = userDto.Email;
                PhoneNumber = userDto.PhoneNumber;
                roleId = userDto.RoleId;
                roleName = userDto.RoleName;
                fullName = userDto.FullName;
                
            }
            catch (Exception ex)
            {

            }
            return userDto;
        }
        private List<MenuDto> MenuList()
        {
            var menuFilter = _businessLayer.BaseService.MenuList(roleName);
            return menuFilter;
        }
        protected bool isAuthorized(int _userId)
        {
            if ((roleName == "admin" || roleName == "hradmin") || userId == _userId)
                return true;
            else
                return false;
        }
        protected string EncodedString(string text)
        {
            return _javaScriptEncoder.Encode(text);
        }
    }
}
