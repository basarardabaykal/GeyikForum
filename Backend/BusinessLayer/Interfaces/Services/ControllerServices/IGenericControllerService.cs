using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.ControllerServices;

public interface IGenericControllerService<TDto>
{
    public Task<IDataResult<List<TDto>>>  GetAll();
}