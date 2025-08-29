using BusinessLayer.Interfaces.Services.ControllerServices;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.ControllerServices;

public class GenericControllerService<TDto> : IGenericControllerService<TDto>
{
    private readonly IGenericDbService<TDto> _genericDbService;

    public GenericControllerService(IGenericDbService<TDto> genericDbService)
    {
        _genericDbService = genericDbService;
    }
    public async Task<IDataResult<List<TDto>>> GetAll()
    {
        return await _genericDbService.GetAll();
    }
}