using HyochulLab.Data.Context;
using HyochulLab.Data.Entities;
using HyochulLab.Data.Interfaces;
using HyochulLab.Data.Repositories;

namespace HyochulLab.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IRepository<User> Users { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new Repository<User>(context);
    }

    public async Task<int> SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
