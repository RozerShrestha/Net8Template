using AutoMapper;
using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace BusinessManagementSystem.BusinessLayer.Implementations
{
    public class BusinessLayerImpl : IBusinessLayer
    {
        protected readonly IUnitOfWork _unitOfWork=null;
        protected readonly IMapper _mapper = null;
        public BusinessLayerImpl(IUnitOfWork unitOfWork, IMapper mapper, ApplicationDBContext dBContext) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            BaseService = new BaseImpl(_unitOfWork);
            UserService = new UserImpl(_unitOfWork, mapper);
            BasicConfigurationService = new BasicConfigurationImpl(_unitOfWork);
            MenuService = new MenuImpl(_unitOfWork);
            RoleService=new RoleImpl(_unitOfWork);
            
        }
        public IBaseService BaseService { get; private set; }

        public IUserService UserService { get; private set; }

        public IBasicConfigurationService BasicConfigurationService { get; private set; }

        public IMenuService MenuService { get; private set; }
        public IRoleService RoleService { get; set; }

    }
}
