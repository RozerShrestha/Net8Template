using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.BusinessLayer.Services
{
    public interface IMenuService
    {
        dynamic ParentList();
        Multiselect RoleList();
        ResponseDto<Menu> GetAllMenu();
        ResponseDto<Menu> GetMenuById(int id);
        ResponseDto<Menu> GetAllActiveMenus();
        ResponseDto<Menu> GetAllInactiveMenus();
        ResponseDto<Menu> CreateMenu(Menu menu);
        ResponseDto<Menu> UpdateMenu(Menu u);
        ResponseDto<Menu> DeleteMenu(int id);
    }
}
