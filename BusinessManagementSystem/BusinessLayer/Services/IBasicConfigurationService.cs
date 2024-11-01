using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.BusinessLayer.Services
{
    public interface IBasicConfigurationService
    {
        ResponseDto<BasicConfiguration> GetBasicConfig();
        ResponseDto<BasicConfiguration> Update(BasicConfiguration basicConfiguration);
    }
}
