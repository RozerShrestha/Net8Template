using AspNetCore;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.BusinessLayer.Implementations
{
    public class RoleImpl : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        ResponseDto<Role> _responseDto;

        public RoleImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _responseDto = new ResponseDto<Role>();
        }
        public ResponseDto<Role> CreateRole(Role role)
        {
            _responseDto = _unitOfWork.Role.Insert(role);
            return _responseDto;
        }

        public ResponseDto<Role> DeleteRole(Role role)
        {
            _responseDto = _unitOfWork.Role.Delete(role);
            return _responseDto;
        }

        public ResponseDto<Role> GetAllRoles()
        {
            _responseDto = _unitOfWork.Role.GetAll();
            return _responseDto;
        }

        public ResponseDto<Role> GetRoleById(int id)
        {
            _responseDto = _unitOfWork.Role.GetById(id);
            return _responseDto;
        }

        public ResponseDto<Role> UpdateRole(Role role)
        {
            _responseDto=_unitOfWork.Role.Update(role);
            return _responseDto;
        }
    }
}
