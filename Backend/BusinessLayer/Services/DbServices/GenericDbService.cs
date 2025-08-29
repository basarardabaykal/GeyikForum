using AutoMapper;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.DbServices;

public class GenericDbService<TDto, TEntity> : IGenericDbService<TDto>
{
    protected readonly IGenericRepository<TEntity> _repository;
    protected readonly IMapper _mapper;

    public GenericDbService(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IDataResult<List<TDto>>> GetAll()
    {
        var result = await _repository.GetAll();
        if (!result.Success)
        {
            return new ErrorDataResult<List<TDto>>(result.StatusCode,  result.Message);
        }

        var dtoList = _mapper.Map<List<TDto>>(result.Data);
        return new SuccessDataResult<List<TDto>>(result.Message, dtoList);
    }
}