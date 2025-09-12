using System.Net;
using System.Net.Mail;
using BusinessLayer.Interfaces.Services.OtherServices;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services.OtherServices;

public class EmailService :  IEmailService
{
  private readonly IConfiguration _configuration;

  public EmailService(IConfiguration configuration)
  {
    _configuration = configuration;
  }
  
  public async Task<IDataResult<string>> SendEmailAsync(string to, string subject, string body)
  {
    using (var client = new SmtpClient(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"])))
    {
      client.Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
      client.EnableSsl = true;

      var mailMessage = new MailMessage
      {
        From = new MailAddress(_configuration["Smtp:From"]),
        Subject = subject,
        Body = body,
        IsBodyHtml = true
      };

      mailMessage.To.Add(to);
      await client.SendMailAsync(mailMessage);

      return new SuccessDataResult<string>("Doğrulama E-postası gönderildi.");
    }
  }
}