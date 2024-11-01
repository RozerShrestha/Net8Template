﻿using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class RoleController : BaseController
    {
        private IWebHostEnvironment _env;
        private ResponseDto<Role> _responseDto;
        private ILogger<RoleController> _logger;
        public  ModalView _modalView;

        public RoleController(IWebHostEnvironment env, IBusinessLayer businessLayer, INotyfService notyf, IEmailSender emailSender, ILogger<RoleController> logger, JavaScriptEncoder javaScriptEncoder) : base(businessLayer, notyf, emailSender, javaScriptEncoder)
        {
            _env = env;
            _responseDto = new ResponseDto<Role>();
            _logger = logger;
            _modalView = new ModalView("Delete Confirmation !", "Delete", "Are you sure to delete the selected Role?", "");
            
        }
        public IActionResult Index()
        {
            ViewBag.ModalInformation = _modalView;
            _responseDto = _businessLayer.RoleService.GetAllRoles();
            return View(_responseDto.Datas);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                _responseDto = _businessLayer.RoleService.CreateRole(role);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    _notyf.Success(_responseDto.Message);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notyf.Error(_responseDto.Message);
                    return View(role);
                }
            }
            else
            {
                IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in errors)
                {
                    _notyf.Error(error.ErrorMessage);
                }
                return RedirectToAction(nameof(Index));
            }
        }
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                _notyf.Warning("Cannot Update Superadmin");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _responseDto = _businessLayer.RoleService.GetRoleById(id);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    return View(_responseDto.Data);
                }
                else
                {
                    _notyf.Error(_responseDto.Message);
                    _logger.LogError($"## {this.GetType().Name} Edit: Not Found {_responseDto.Message}");
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            _responseDto = _businessLayer.RoleService.UpdateRole(role);
            if (_responseDto.StatusCode == HttpStatusCode.OK)
            {
                _notyf.Success(_responseDto.Message);
                return RedirectToAction(nameof(Index)); ;
            }
            else
            {
                _notyf.Error(_responseDto.Message);
                return View(role);
            }
        }
        public ActionResult Delete(int id)
        {
            if (roleName != SD.Role_Superadmin)
            {
                _notyf.Warning("Only Super Admin can delete");
                return RedirectToAction(nameof(Index));
            }
            var item = _businessLayer.RoleService.GetRoleById(id);
            _responseDto = _businessLayer.RoleService.DeleteRole(item.Data);

            if (_responseDto.StatusCode == HttpStatusCode.OK)
            {
                _notyf.Success(_responseDto.Message);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notyf.Error("Role Not Found");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
