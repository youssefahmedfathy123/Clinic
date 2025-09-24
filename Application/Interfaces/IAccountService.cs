using Application.Dtos;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<Result<CurrentUser>> Register(RegisterDto input);
        Task<Result<CurrentUser>> Login(LoginDto input);
        Task<Result<bool>> AddToRoleAsync(string userId, string roleName);
        Task<Result<string>> ForgotPassword(ForgotPasswordDto input);
        Task<Result<bool>> ResetPassword(ResetPasswordDto input);

    }
}


