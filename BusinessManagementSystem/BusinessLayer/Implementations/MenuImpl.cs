using AspNetCore;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Net;

namespace BusinessManagementSystem.BusinessLayer.Implementations
{
    public class MenuImpl : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        ResponseDto<Menu> _responseDto;

        public MenuImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _responseDto = new ResponseDto<Menu>();
        }

        public dynamic ParentList()
        {
            var parentList=_unitOfWork.Menu.ParentList();
            return parentList;
        }

        public Multiselect RoleList()
        {
            var roles = _unitOfWork.Role.GetRoles();
            var roleLists = new Multiselect();
            var listItems = new List<SelectListItem>();
            foreach (var role in roles)
            {
                listItems.Add(new SelectListItem { Value = role.Id.ToString(), Text = role.Name });
            }
            roleLists.Items = listItems;
            return roleLists;
        }
        public ResponseDto<Menu> GetAllActiveMenus()
        {
            _responseDto = _unitOfWork.Menu.GetAll(p => p.Status == true);
            return _responseDto;
        }

        public ResponseDto<Menu> GetAllInactiveMenus()
        {
            _responseDto = _unitOfWork.Menu.GetAll(p => p.Status == false);
            return _responseDto;
        }

        public ResponseDto<Menu> GetAllMenu()
        {
            _responseDto = _unitOfWork.Menu.GetAll();
            return _responseDto;
        }

        public ResponseDto<Menu> GetMenuById(int id)
        {
            _responseDto = _unitOfWork.Menu.GetFirstOrDefault(p => p.Id == id, includeProperties: "MenuRoles");
            var roles = _unitOfWork.Role.GetRoles();
            var roleLists = new Multiselect();
            var selectedItems = new List<int>();
            var listItems = new List<SelectListItem>();
            foreach (var role in roles)
            {
                //checking if MenuRole is within available Roles
                var check = _responseDto.Data.MenuRoles.Where(p => p.RoleId == role.Id);
                if (check.Any())
                {
                    selectedItems.Add(role.Id);
                    listItems.Add(new SelectListItem { Value = role.Id.ToString(), Text = role.Name, Selected = true });
                }
                else
                {
                    listItems.Add(new SelectListItem { Value = role.Id.ToString(), Text = role.Name, Selected = false });
                }
            }
           
            roleLists.SelectedItems = selectedItems;
            roleLists.Items = listItems;
            _responseDto.Data.Multiselect = roleLists;
            return _responseDto;
        }

        public ResponseDto<Menu> CreateMenu(Menu menu)
        {
            _responseDto = _unitOfWork.Menu.CreateMenu(menu);
            return _responseDto;
        }

        public ResponseDto<Menu> UpdateMenu(Menu menu)
        {
            _responseDto = _unitOfWork.Menu.UpdateMenu(menu);
            return _responseDto;
        }

        public ResponseDto<Menu> DeleteMenu(int id)
        {
            Menu menu=_unitOfWork.Menu.GetById(id).Data;
            if(menu != null)
            {
                _responseDto = _unitOfWork.Menu.Delete(menu);
            }
            else
            {
                _responseDto.StatusCode = HttpStatusCode.NotFound;
                _responseDto.Message = "Menu not found";
            }
            return _responseDto;

        }

        public dynamic GetMenuRoles(int id)
        {
            var menuAssignedRoles = _unitOfWork.MenuRole.GetRolesAssignedToMenu(id);
            return menuAssignedRoles;
        }
    }
}
