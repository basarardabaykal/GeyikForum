using System.Net;
using System.Text;
using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Repositories;

public class AuthRepository : IAuthRepository
{
  protected readonly AppDbContext _dbContext;
  private readonly UserManager<AppUser> _userManager;
  private readonly RoleManager<IdentityRole<Guid>> _roleManager;

  public AuthRepository(AppDbContext dbContext, UserManager<AppUser> userManager , RoleManager<IdentityRole<Guid>> roleManager)
  {
    _dbContext = dbContext;
    _userManager = userManager;
    _roleManager = roleManager;
  }
  
  public async Task<IDataResult<AppUser>> GetUserByEmail(string email)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
      return new ErrorDataResult<AppUser>(404, "Bu e-postaya sahip kullanıcı bulunamadı.");
    }
    else
    {
      return new SuccessDataResult<AppUser>( "Bu e-postaya sahip kullanıcı başarıyla bulundu.", user);
    }
  }

  public async Task<IDataResult<AppUser>> GetUserByNickname(string nickname)
  {
    var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Nickname == nickname);
    if (user == null)
    {
      return new ErrorDataResult<AppUser>(404, "Bu kullanıcı adına sahip kullanıcı bulunamadı.");
    }
    else
    {
      return new SuccessDataResult<AppUser>( "Bu kullanıcı adına sahip kullanıcı başarıyla bulundu.", user);
    }
  }

  public async Task<IDataResult<AppUser>> GetUserByUid(string uid)
  {
    if (string.IsNullOrEmpty(uid))
    {
      return new ErrorDataResult<AppUser>(400, "Geçerli kullanıcı bulunamadı.");
    }
    var user = await _userManager.FindByIdAsync(uid);
    if (user == null)
    {
      return new ErrorDataResult<AppUser>(404, "Geçerli kullanıcı bulunamadı.");
    }
    else
    {
      return new SuccessDataResult<AppUser>( "Geçerli kullanıcı başarıyla bulundu.", user);
    }
  }
  
  public async Task<IDataResult<AppUser>> CheckPassword(AppUser user, string password)
  {
    var hasMatchedPasswords = await _userManager.CheckPasswordAsync(user, password);
    if (hasMatchedPasswords)
    {
      return new SuccessDataResult<AppUser>("Başarıyla giriş yapıldı.", user);
    }
    else
    {
      return new ErrorDataResult<AppUser>(500, "Şifre doğru değil.", user);
    }
  }
  
  public async Task<IDataResult<AppUser>> CreateUser(AppUser user, string password)
  {
    var result = await _userManager.CreateAsync(user, password);
      
    if (result.Succeeded)
    {
      var createdUser = await _userManager.FindByEmailAsync(user.Email);
      return new SuccessDataResult<AppUser>("Kullanıcı başarıyla oluşturuldu.", createdUser);
    }
    else
    {
      Console.WriteLine(result.Errors.First());
      return new ErrorDataResult<AppUser>(400, "Kullanıcı oluşturma başarısız oldu.");
    }
  }
  
  public async Task<IDataResult<List<string>>> GetUserRoles(string email)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
      return new ErrorDataResult<List<string>>(404, "Kullanıcı bulunamadı.");
    }

    var roles = await _userManager.GetRolesAsync(user);
    if (roles == null)
    {
      return new ErrorDataResult<List<string>>(404, "Roles for this user was not found.");
    }
    
    return new SuccessDataResult<List<string>>("Kullanıcı rolleri başarıyla alındı.", roles.ToList());
  }
  
  public async Task<IDataResult<bool>> AssignRole(AppUser user, string role)
  {
    if (!await _roleManager.RoleExistsAsync(role))
    {
      await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
    }

    if (await _userManager.IsInRoleAsync(user, role))
    {
      return new SuccessDataResult<bool>($"Kullanıcı zaten '{role}' rolüne sahip.");
    }

    var result = await _userManager.AddToRoleAsync(user, role);
    if (result.Succeeded)
    {
      return new SuccessDataResult<bool>($"'{role}' rolü kullanıcıya başarıyla atandı.");
    }
    else
    {
      return new ErrorDataResult<bool>(400, "Rol atama başarısız oldu.");
    }
  }

  public async Task<IDataResult<string>> GenerateEmailConfirmationToken(AppUser user)
  {
    var result = await  _userManager.GenerateEmailConfirmationTokenAsync(user);
    return new SuccessDataResult<string>("E-posta doğrulama tokeni başarıyla oluşturuldu.", result);
  }

  public async Task<IDataResult<object>> ConfirmEmail(string userId, string token)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
    {
      return new ErrorDataResult<object>(404, "Kullanıcı bulunamadı.");
    }

    if (user.EmailConfirmed)
    {
      return new SuccessDataResult<object>("E-posta zaten doğrulanmış.", new { userId = user.Id });
    }

    string decodedToken = token?.Trim() ?? string.Empty;
    try
    {
      decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(decodedToken));
    }
    catch
    {
      // Fallback for old links or mail clients
      decodedToken = WebUtility.UrlDecode(decodedToken).Replace(' ', '+');
    }

    var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
    if (!result.Succeeded)
    {
      var errors = string.Join("; ", result.Errors.Select(e => e.Description));
      return new ErrorDataResult<object>(400, $"Doğrulama başarısız.");
    }

    return new SuccessDataResult<object>("E-posta başarıyla doğrulandı.", new { userId = user.Id });
  }

  public async Task<IDataResult<string>> GeneratePasswordResetToken(AppUser user)
  {
    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
    return new SuccessDataResult<string>("Token üretildi.", token);
  }

  public async Task<IDataResult<object>> ResetPassword(string userId, string token, string newPassword)
  {
    var user = await _userManager.FindByIdAsync(userId);
    if (user is null)
      return new ErrorDataResult<object>(404, "Kullanıcı bulunamadı.");

    string decoded = token?.Trim() ?? string.Empty;
    try
    {
      decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(decoded));
    }
    catch
    {
      decoded = WebUtility.UrlDecode(decoded).Replace(' ', '+');
    }

    var result = await _userManager.ResetPasswordAsync(user, decoded, newPassword);
    if (!result.Succeeded)
    {
      var errors = string.Join("; ", result.Errors.Select(e => e.Description));
      return new ErrorDataResult<object>(400, $"Şifre sıfırlama başarısız. {errors}");
    }

    return new SuccessDataResult<object>("Şifre başarıyla sıfırlandı.", new { userId = user.Id });
  }
}