using BusinessLayer.Dtos.Auth;
using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Repositories;

public class AuthRepository : IAuthRepository
{
  private readonly UserManager<AppUser> _userManager;
  private readonly RoleManager<IdentityRole<Guid>> _roleManager;

  public AuthRepository(UserManager<AppUser> userManager , RoleManager<IdentityRole<Guid>> roleManager)
  {
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

  public async Task<IDataResult<object>> ConfirmEmail(ConfirmEmailRequestDto confirmEmailRequestDto)
  {
    var userResult = await GetUserByUid(confirmEmailRequestDto.UserId.ToString());
    if (!userResult.Success)
    {
      return new ErrorDataResult<object>(404, userResult.Message);
    }
    
    var result = await _userManager.ConfirmEmailAsync(userResult.Data, confirmEmailRequestDto.Token);

    if (!result.Succeeded)
    {
      return new ErrorDataResult<object>(500, "E-posta doğrulanamadı. Tekrar giriş yapmayı deneyin.");
    }
    return new SuccessDataResult<object>("E-posta başarıyla doğrulandı.");
  }
}