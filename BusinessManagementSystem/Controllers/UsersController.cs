﻿using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Helper;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        public ResponseDto<User> _responseDto;
        public ResponseDto<UserDto> _responseUserDto;
        public ResponseDto<UserRoleDto> _responseUserRoleDto;
        private ILogger<UsersController> _logger;
        private readonly ModalView _modalView;
        private readonly dynamic roleList;
        public UsersController(IBusinessLayer businessLayer, INotyfService notyf, IEmailSender emailSender, ILogger<UsersController> logger, JavaScriptEncoder javaScriptEncoder) : base(businessLayer, notyf, emailSender, javaScriptEncoder)
        {
            roleList = _businessLayer.UserService.RoleList();
            
            _responseDto = new ResponseDto<User>();
            _responseUserDto = new ResponseDto<UserDto>();
            _responseUserRoleDto = new ResponseDto<UserRoleDto>();
            _modalView = new ModalView("Delete Confirmation !", "Delete", "Are you sure to delete the selected User?", "");
            _logger = logger;
            
        }
        [HttpGet]
        [Authorize(Roles = "superadmin,admin_tattoo,admin_kaffe,admin_apartment")]
        public IActionResult Index()
        {
            ViewBag.ModalInformation = _modalView;
            return View(_responseDto);
        }

        [HttpGet]
        public IActionResult Detail(Guid guid)
        {
            if(guid == Guid.Empty)
            {
                _responseDto = _businessLayer.UserService.GetUserById(userId);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    guid = _responseDto.Data.Guid;
                    _responseUserDto = _businessLayer.UserService.GetUserByGuid(guid);
                    if(_responseUserDto.StatusCode == HttpStatusCode.OK)
                    {
                        return View(_responseUserDto.Data);
                    }
                }
                return NotFound();
            }
            else
            {
                _responseUserDto = _businessLayer.UserService.GetUserByGuid(guid);
                if (_responseUserDto.StatusCode == HttpStatusCode.OK)
                {
                    return View(_responseUserDto.Data);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "superadmin")]
        public IActionResult Create()
        {
            ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");
            ViewBag.OccupationList = new SelectList(SD.Occupations, "Value", "Value");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public IActionResult Create(UserDto userDto, IFormFile? ProfilePictureLink)
        {
            ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");
            ViewBag.OccupationList = new SelectList(SD.Occupations, "Value", "Value");

            //validating document upload
            if (!Helpers.ValidateDocumentUpload(ProfilePictureLink))
            {
                _notyf.Warning("Profile Picture Upload Error: Valid files are of extension pdf or jpg or jpeg");
                return BadRequest("Error saving Profile Picture. Please check valid extensions(pdf,jpeg,jpg,png)");
            }


            if (ModelState.IsValid)
            {
                userDto.ProfilePictureLink =ProfilePictureLink==null?string.Empty: Helpers.DocUpload(ProfilePictureLink, "ProfilePicture", username);
                _responseDto = _businessLayer.UserService.CreateUser(userDto);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    _notyf.Success(_responseDto.Message);
                    return RedirectToAction(nameof(Index));
                } 
                else
                {
                    _notyf.Error(_responseDto.Message);
                    return View(userDto);
                }
            }
            else
            {
                IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in errors)
                {
                    _notyf.Error(error.ErrorMessage);
                }
                return View(userDto);
            }
        }

        public IActionResult Edit(Guid guid)
        {
            if (guid==Guid.Empty)
            {
                return NotFound();
            }
            ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");
            ViewBag.OccupationList = new SelectList(SD.Occupations, "Value", "Value");
            var _responseDto = _businessLayer.UserService.GetUserByGuid(guid);
            if (_responseDto == null)
             {
                return NotFound();
            }
            return View(_responseDto.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserDto userDto, IFormFile? ProfilePictureLink)
      {
            ModelState.Remove(nameof(userDto.Password)); //just to ignore ConfirmPassword to validate
            ModelState.Remove(nameof(userDto.ConfirmPassword)); //just to ignore ConfirmPassword to validate
            ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");
            ViewBag.OccupationList = new SelectList(SD.Occupations, "Value", "Value");
            if(roleName==SD.Role_Superadmin || userId== userDto.UserId)
            {
                if (ModelState.IsValid)
                {
                    userDto.ProfilePictureLink = ProfilePictureLink == null ? string.Empty : Helpers.DocUpload(ProfilePictureLink, "ProfilePicture", username);
                    _responseDto = _businessLayer.UserService.UpdateUser(userDto);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        _notyf.Success(_responseDto.Message);
                    }
                    else
                    {
                        _notyf.Error(_responseDto.Message);
                        return View(userDto);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                    foreach (var error in errors)
                    {
                        _notyf.Error(error.ErrorMessage);
                    }
                    return View(_responseDto.Data);
                }
            }
            else
            {
                _notyf.Warning($"{fullName} is not authroized to perform this task");
                return RedirectToAction(nameof(Index));
            }
            
           
        }
        
        

        [HttpGet]
        public IActionResult Test(Guid id)
        {
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "superadmin")]
        public IActionResult DeleteConfirmed(int UserId)
        {
            _responseDto = _businessLayer.UserService.GetUserById(UserId);
            if (_responseDto.Data != null)
            {
                try
                {
                    _responseDto=_businessLayer.UserService.DeleteUser(UserId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                  _notyf.Error($"Error deleting User due to : {ex.Message}");
                    return View(_responseDto.Data.Guid);
                }
            }
            else
            {
                _notyf.Error("Error: User not Found");
                return NotFound();
            }
            
        }



        #region API CALLS

        [HttpGet]
        [Authorize(Roles = "superadmin,admin_tattoo,admin_kaffe,admin_apartment")]
        public IActionResult GetAllUser()
        {
            string who = roleName;
            if(who==SD.Role_Superadmin)
                _responseUserRoleDto = _businessLayer.UserService.GetAllUser(SD.Role_Superadmin);
            else
            {
                _responseUserRoleDto = _businessLayer.UserService.GetAllUser(SD.Role_ApartmentAdmin);
            }

            if (_responseUserRoleDto.StatusCode == HttpStatusCode.OK)
            {
                return Ok(_responseUserRoleDto);
            }
            else
            {
                return BadRequest(_responseUserRoleDto);
            }
            
        }
        [Authorize(Roles = "superadmin")]
        [HttpGet]
        public IActionResult Delete(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                _notyf.Error("Something went wrong");
                return NotFound();
            }
            var item = _businessLayer.UserService.GetUserByGuid(guid);
            if (item.StatusCode == HttpStatusCode.OK)
            {
                _responseDto = _businessLayer.UserService.DeleteUser(item.Data.UserId);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
