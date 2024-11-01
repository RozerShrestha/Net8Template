using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BusinessManagementSystem.Services
{
    public interface IUnitOfWork
    {
        IUser Users { get; }
        IRole Role { get; }
        IMenu Menu { get; }
        IMenuRole MenuRole { get; }
        IUserRole UserRole { get; }
        IBasicConfiguration BasicConfiguration { get; }      
        IDashboard Dashboard { get; }
        IBase Base { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();

        void SaveChanges();
    }
}
