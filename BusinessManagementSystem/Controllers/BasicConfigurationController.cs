using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize(Roles = "superadmin,admin_tattoo,admin_kaffe,admin_apartment")]
    public class BasicConfigurationController : BaseController
    {
        private IWebHostEnvironment _env;
        private ResponseDto<BasicConfiguration> _responseDto;
        private ILogger<BasicConfigurationController> _logger;
        //private ModalView _modalView;
        public BasicConfigurationController(IWebHostEnvironment env, IBusinessLayer businessLayer, INotyfService notyf, IEmailSender emailSender, ILogger<BasicConfigurationController> logger, JavaScriptEncoder javaScriptEncoder) : base(businessLayer, notyf, emailSender, javaScriptEncoder)
        {
            _env = env;
            _responseDto = new ResponseDto<BasicConfiguration>();
            _logger = logger;
            //_modalView = new ModalView();
        }
        public IActionResult Index()
        {
            _responseDto = _businessLayer.BasicConfigurationService.GetBasicConfig();
            return View(_responseDto.Data);
        }

        public IActionResult Update(BasicConfiguration basicConfiguration)
        {
            if (ModelState.IsValid)
            {
                _responseDto = _businessLayer.BasicConfigurationService.Update(basicConfiguration);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    _notyf.Success("Update Success");
                }
                else
                {
                    _notyf.Error(_responseDto.Message);
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
                return RedirectToAction(nameof(Index));
            }
            
        }
    }
}
