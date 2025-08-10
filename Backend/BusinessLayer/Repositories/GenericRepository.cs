using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where  TEntity : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet; // represents which table this repo will interact with.

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }
    public async Task<IDataResult<List<TEntity>>> GetAll()
    {
        try
        {
            var result = await _dbSet.ToListAsync();
            return new SuccessDataResult<List<TEntity>>("All posts retrieved successfully.", result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new ErrorDataResult<List<TEntity>>(500, "Error while retrieving all items from the database.");
        }
        
        
    }
    
}