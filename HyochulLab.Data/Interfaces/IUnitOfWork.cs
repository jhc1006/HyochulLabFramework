using HyochulLab.Data.Entities;

namespace HyochulLab.Data.Interfaces;

public interface IUnitOfWork
{
    IRepository<User> Users { get; }

    Task<int> SaveChangesAsync();
}
