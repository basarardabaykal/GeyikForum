using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Repositories;

public interface IGenericRepository<TEntity>
{
    public Task<IDataResult<List<TEntity>>> GetAll();
}