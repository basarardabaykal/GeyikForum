using BusinessLayer.Interfaces.Repositories;
using CoreLayer.Entities;
using CoreLayer.Utilities.DataResults.Concretes;
using CoreLayer.Utilities.DataResults.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Repositories;

public class AuthRepository : IAuthRepository
{
  private readonly UserManager<AppUser> _userManager;

  public AuthRepository(UserManager<AppUser> userManager)
  {
    _userManager = userManager;
  }
  
  public async Task<IDataResult<AppUser>> GetUserByEmail(string email)
     {
       try
       {
         var user = await _userManager.FindByEmailAsync(email);
         if (user == null)
         {
           throw new Exception("No user was found with this email.");
         }
         else
         {
           return new SuccessDataResult<AppUser>( "User with this email has been found successfully.", user);
         }
       }
       catch (Exception exception)
       {
         {
           return new ErrorDataResult<AppUser>(500, "An unexpected error occurred while looking for a user with this email.");
         }
       }
     }
  
  public async Task<IDataResult<AppUser>> CreateUser(AppUser user, string password)
  {
    try
    {
      var result = await _userManager.CreateAsync(user, password);

      if (result.Succeeded)
      {
        return new SuccessDataResult<AppUser>("User has been created successfully.", user);
      }
      else
      {
        return new ErrorDataResult<AppUser>(400, "User creation failed.");
      }
    }
    catch (Exception exception)
    {
      return new ErrorDataResult<AppUser>(500, "An unexpected error occurred while creating the user.");
    }
  }
  
  public async Task<IDataResult<List<string>>> GetUserRoles(string email)
  {
    try
    {
      var user = await _userManager.FindByEmailAsync(email);
      if (user == null)
      {
        return new ErrorDataResult<List<string>>(404, "User not found.");
      }

      var roles = await _userManager.GetRolesAsync(user);
      return new SuccessDataResult<List<string>>("User roles retrieved successfully.", roles.ToList());
    }
    catch (Exception exception)
    {
      return new ErrorDataResult<List<string>>(500, "An unexpected error occurred while retrieving user roles.");
    }
  }
}