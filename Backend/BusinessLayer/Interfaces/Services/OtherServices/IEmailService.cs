using CoreLayer.Utilities.DataResults.Interfaces;

namespace BusinessLayer.Interfaces.Services.OtherServices;

public interface IEmailService
{
  public Task<IDataResult<string>>  SendEmailAsync(string to, string subject, string body);
}