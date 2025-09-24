using Application.Dtos;
using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums; 
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    internal class AccountService(UserManager<User> _userManager, ICloudinaryService _photo, IJwt _jwt) : IAccountService
    {
        public async Task<Result<CurrentUser>> Register(RegisterDto input)
        {
            var checkEmail = await _userManager.FindByEmailAsync(input.Email);
            var checkUserName = await _userManager.FindByNameAsync(input.UserName);

            if (checkEmail is not null)
                return Result<CurrentUser>.Fail("Email already exists");

            if (checkUserName is not null)
                return Result<CurrentUser>.Fail("Username already exists");

            var photo = await _photo.AddPhotoAsync(input.Photo);

            var newUser = new User
            {
                UserName = input.UserName,
                Email = input.Email,
                Name = input.Name,
                Nickname = input.Nickname,
                DateOfBirth = input.DateOfBirth,
                Gender = input.Gender,
                PhotoUrl = photo.Url.ToString()
            };

            var pass = await _userManager.CreateAsync(newUser, input.Password);

            if (!pass.Succeeded)
            {
                var errors = string.Join(",", pass.Errors.Select(e => e.Description));
                return Result<CurrentUser>.Fail(errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, Roles.User.ToString());
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Result<CurrentUser>.Fail($"Failed to assign role: {errors}");
            }

            var token = await _jwt.GenerateToken(newUser);

            var currentUser = new CurrentUser
            {
                UserName = newUser.UserName,
                Token = token,
                PhotoUrl = newUser.PhotoUrl,
                Email = newUser.Email
            };

            return Result<CurrentUser>.Success(currentUser);
        }

        public async Task<Result<CurrentUser>> Login(LoginDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
                return Result<CurrentUser>.Fail("Email or password is incorrect!");

            var passwordValid = await _userManager.CheckPasswordAsync(user, input.Password);
            if (!passwordValid)
                return Result<CurrentUser>.Fail("Email or password is incorrect!");

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(Roles.User.ToString()))
            {
                var addRole = await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                if (!addRole.Succeeded)
                {
                    var errors = string.Join(", ", addRole.Errors.Select(e => e.Description));
                    return Result<CurrentUser>.Fail($"Failed to assign role: {errors}");
                }
            }

            var token = await _jwt.GenerateToken(user);

            var currentUser = new CurrentUser
            {
                UserName = user.UserName,
                Token = token,
                PhotoUrl = user.PhotoUrl,
                Email = user.Email
            };

            return Result<CurrentUser>.Success(currentUser);
        }


        public async Task<Result<bool>> AddToRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result<bool>.Fail("User not found");

            var roleExists = await _userManager.IsInRoleAsync(user, roleName);
            if (roleExists)
                return Result<bool>.Fail("User already in this role");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Fail(errors);
            }

            return Result<bool>.Success(true);
        }




        public async Task<Result<string>> ForgotPassword(ForgotPasswordDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
                return Result<string>.Fail("User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Result<string>.Success(token);
        }

        public async Task<Result<bool>> ResetPassword(ResetPasswordDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
                return Result<bool>.Fail("User not found");

            var result = await _userManager.ResetPasswordAsync(user, input.Token, input.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<bool>.Fail(errors);
            }

            return Result<bool>.Success(true);
        }
    }
}
