using BusinessManagementSystem.BusinessLayer.Services;
using BusinessManagementSystem.Controllers;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.BusinessLayer.Implementations
{
    public class BasicConfigurationImpl : IBasicConfigurationService
    {
        private readonly IUnitOfWork _unitOfWork;
        ResponseDto<BasicConfiguration> _responseDto;

        public BasicConfigurationImpl(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _responseDto = new ResponseDto<BasicConfiguration>();
        }
        public ResponseDto<BasicConfiguration> GetBasicConfig()
        {
            var responseDto = _unitOfWork.BasicConfiguration.GetSingleOrDefault();
            return responseDto;
        }
        
        public ResponseDto<BasicConfiguration> Update(BasicConfiguration basicConfiguration)
        {
            var response=_unitOfWork.BasicConfiguration.Update(basicConfiguration);
            return response;
        }
    }
}
