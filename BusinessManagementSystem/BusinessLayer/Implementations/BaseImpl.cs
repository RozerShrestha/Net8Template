using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.BusinessLayer.Implementations
{
    public class BaseImpl : IBaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BaseImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<MenuDto> MenuList(string roleName)
        {
            var menuDtoList=_unitOfWork.Base.MenuList(roleName);
            return menuDtoList;
        }

        public UserDto UserDetail(string userName)
        {
            var userDto= _unitOfWork.Base.UserDetail(userName);
            return userDto;
        }

        public dynamic RoleList()
        {
            var roleLIst = _unitOfWork.Base.RoleList();
            return roleLIst;
        }
    }
}
