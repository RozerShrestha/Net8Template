using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.BusinessLayer.Services
{
    public interface IBaseService
    {
        UserDto UserDetail(string userName);
        List<MenuDto> MenuList(string roleName);
    }
}
