namespace BusinessManagementSystem.BusinessLayer.Services
{
    public interface IBusinessLayer
    {
        IBaseService BaseService { get; }
        IUserService UserService { get; }
        IBasicConfigurationService BasicConfigurationService { get; }
        IMenuService MenuService { get; }
        IRoleService RoleService { get; }
    }
}
