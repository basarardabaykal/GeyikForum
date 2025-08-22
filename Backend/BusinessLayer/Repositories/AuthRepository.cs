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
      return new ErrorDataResult<AppUser>(404, "No user was found with this email.");
    }
    else
    {
      return new SuccessDataResult<AppUser>( "User with this email has been found successfully.", user);
    }
  }
  
  public async Task<IDataResult<AppUser>> CreateUser(AppUser user, string password)
  {
    var result = await _userManager.CreateAsync(user, password);
      
    if (result.Succeeded)
    {
      var createdUser = await _userManager.FindByEmailAsync(user.Email);
      return new SuccessDataResult<AppUser>("User has been created successfully.", createdUser);
    }
    else
    {
      return new ErrorDataResult<AppUser>(400, "User creation failed.");
    }
  }
  
  public async Task<IDataResult<List<string>>> GetUserRoles(string email)
  {
    var user = await _userManager.FindByEmailAsync(email);
    if (user == null)
    {
      return new ErrorDataResult<List<string>>(404, "User not found.");
    }

    var roles = await _userManager.GetRolesAsync(user);
    if (roles == null)
    {
      return new ErrorDataResult<List<string>>(404, "Roles for this user was not found.");
    }
    
    return new SuccessDataResult<List<string>>("User roles retrieved successfully.", roles.ToList());
  }
  
  public async Task<IDataResult<bool>> AssignRole(AppUser user, string role)
  {
    if (!await _roleManager.RoleExistsAsync(role))
    {
      await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
    }

    if (await _userManager.IsInRoleAsync(user, role))
    {
      return new SuccessDataResult<bool>($"User already has role '{role}'.");
    }

    var result = await _userManager.AddToRoleAsync(user, role);
    if (result.Succeeded)
    {
      return new SuccessDataResult<bool>($"Role '{role}' has been assigned to user successfully.");
    }
    else
    {
      return new ErrorDataResult<bool>(400, "Role assignment failed.");
    }
  }
}