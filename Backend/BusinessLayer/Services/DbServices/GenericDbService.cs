using AutoMapper;
using BusinessLayer.Interfaces.Repositories;
using BusinessLayer.Interfaces.Services.DbServices;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Services.DbServices;

public class GenericDbService<TDto, TEntity> : IGenericDbService<TDto>
{
    protected readonly IGenericRepository<TEntity> _repository;
    private readonly IMapper _mapper;

    public GenericDbService(IGenericRepository<TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IDataResult<List<TDto>>> GetAll()
    {
        var result = _repository.GetAll();
        if (!result.Success)
        {
            return result;
        }

        var dtoList = _mapper.Map<List<TDto>>(result.Data);
        return new SuccessDataResult<List<TDto>>(result.Message, dtoList);
    }
}