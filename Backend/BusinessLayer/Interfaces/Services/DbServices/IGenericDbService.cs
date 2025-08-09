using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.DbServices;

public interface IGenericDbService<TDto>
{
    public Task<IDataResult<List<TDto>>> GetAll();
}