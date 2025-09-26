using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class UserController(IAccountService _accountService) : ApiController
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto input)
        {
            var result = await _accountService.Register(input);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto input)
        {
            var result = await _accountService.Login(input);

            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }



        [Authorize(Roles = "Admin")]
        [HttpPost("add-to-role")]
        public async Task<IActionResult> UpdateRolesDto([FromBody] UpdateRolesDto input)
        {
            var result = await _accountService.UpdateUserRolesAsync(input);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto input)
        {
            var result = await _accountService.ForgotPassword(input);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto input)
        {
            var result = await _accountService.ResetPassword(input);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok("Password has been reset successfully");
        }




     [HttpGet("GetUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {

            var result = await _accountService.GetRolesOfUser(userId);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }


        [HttpPost("UpdateRoles")]
        public async Task<IActionResult> UpdateRoles([FromBody] UpdateRolesDto input)
        {
            var result = await _accountService.UpdateUserRolesAsync(input);

            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok(result.Data);
        }


    }
}

