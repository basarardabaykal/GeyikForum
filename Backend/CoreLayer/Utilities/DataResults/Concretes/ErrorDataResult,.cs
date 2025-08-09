using CoreLayer.Utilities.DataResults.Abstracts;

namespace CoreLayer.Utilities.DataResults.Concretes;

public class ErrorDataResult<T> : DataResult<T>
{
  public ErrorDataResult(int statusCode, string errorMessage, T data)
    : base(true, statusCode, errorMessage, data) { }
  
  public ErrorDataResult(int statusCode, string errorMessage)
    : base(true, statusCode, errorMessage, default(T)) { }
  
  public ErrorDataResult(int statusCode)
    : base(true, statusCode, "Operation failed.", default(T)) { }
}