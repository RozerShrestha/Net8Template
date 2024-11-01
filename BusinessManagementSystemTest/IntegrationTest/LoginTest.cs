using AutoMapper;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessManagementSystemTest.IntegrationTest
{
    [TestFixture]
    public class LoginTest
    {
        ILogin<LoginResponseDto> _iLogin;
        private ApplicationDBContext _dbContext;
        public readonly IConfiguration _config;
        private readonly IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var connectionString = "Data Source=ROZERSHRESTHA;Initial Catalog=BusinessManagement;TrustServerCertificate=True;User Id=sa;Password=P@ssw0rd";
            var options = new DbContextOptionsBuilder<ApplicationDBContext>().UseSqlServer(connectionString);
            _dbContext = new ApplicationDBContext(options);
            _iLogin = new LoginRepository(_dbContext, _config, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            // Clear the in-memory database after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public void RegisterCustomer_Success()
        {
            UserDto userDto = new UserDto()
            {
                UserId = 111,
                UserName = "test",
                Email = "test@gmail.com",
                Password = "P@ssw0rd",
                FullName = "Test Shrestha",
                Address = "Bhimsensthan",
                DateOfBirth = DateOnly.Parse("1991/03/01") ,
                PhoneNumber = "9818136562",
                Gender = "Male",
                Occupation = "IT",
                RoleId = 1,
                RoleName = SD.Role_Superadmin
            };

            var response = _iLogin.Register_User(userDto);
            Assert.Pass();
        }
    }
}