using BusinessManagementSystem.Services;
using BusinessManagementSystem.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using BusinessManagementSystem.Dto;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
//using System.Data.Entity;

namespace BusinessManagementSystem.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly bool _disposed;
        private string _errorMessage = string.Empty;
        private readonly ApplicationDBContext _dbContext = null;
        private IDbContextTransaction _objTran = null;

        public UnitOfWork(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
            Users = new UserRepository(_dbContext);
            Base = new BaseRepository(_dbContext);
            //Dashboard = new DashboardRepository(_db);
            Role =new RoleRepository(_dbContext);
            Menu = new MenuRepository(_dbContext);
            MenuRole=new MenuRoleRepository(_dbContext);
            UserRole=new UserRoleRepository(_dbContext);
            BasicConfiguration = new BasicConfigurationRepository(_dbContext);
        }

        public IUser Users { get; private set; }

        public IBase Base { get; private set; }
        public IDashboard Dashboard { get; private set; }
        public IRole Role { get; private set; }
        public IMenu Menu { get; private set; }
        public IMenuRole MenuRole { get; private set; }
        public IUserRole UserRole { get; private set; }

        public IBasicConfiguration BasicConfiguration { get; private set; }
        
        public void BeginTransaction()
        {
            //It will Begin the transaction on the underlying connection
            _objTran =_dbContext.Database.BeginTransaction();
        }
        public void Commit()
        {
            //Commits the underlying store transaction
            _objTran.Commit();
            _objTran.Dispose();
        }
        public void Rollback()
        {
            //Rolls back the underlying store transaction
            _objTran.Rollback();
            //The Dispose Method will clean up this transaction object and ensures Entity Framework
            //is no longer using that transaction.
            _objTran.Dispose();
        }
        public void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message, ex);
                //foreach (var validationErrors in ex.EntityValidationErrors)
                //{
                //    foreach (var validationError in validationErrors.ValidationErrors)
                //    {
                //        _errorMessage = _errorMessage + $"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage} {Environment.NewLine}";
                //    }
                //}
                //throw new Exception(_errorMessage, ex);

            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            _dbContext.Dispose();
        }
    }
}
