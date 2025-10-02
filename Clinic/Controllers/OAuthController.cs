using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinic.Controllers
{
    public class OAuthController : ApiController
    {
        private readonly IOAuthService _auth;
        private readonly SignInManager<User> _signInManager;

        public OAuthController(IOAuthService auth, SignInManager<Domain.Entities.User> signInManager)
        {
            _auth = auth;
            _signInManager = signInManager;
        }

        [HttpGet("signin-google")]
        public IActionResult SignInGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl!);
            return Challenge(properties, "Google");
        }

        
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
                return BadRequest("External authentication failed");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name =  result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Email claim not found");
            


            var loginResult = await _auth.HandleExternalLoginAsync(email, name);

            if (!loginResult.Succeeded)
                return BadRequest(new { error = loginResult.Error });


            return Ok(new 
            {
                Token = loginResult.Data!,
                UserName = email,
                Email = email
            });
        }






        //[HttpGet("signin-facebook")]
        //public IActionResult SignInFacebook()
        //{
        //    var redirectUrl = Url.Action(nameof(FacebookResponse));
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl!);
        //    return Challenge(properties, "Facebook");
        //}

        //[HttpGet("facebook-response")]
        //public async Task<IActionResult> FacebookResponse()
        //{
        //    var result = await HttpContext.AuthenticateAsync("Facebook");
        //    if (!result.Succeeded)
        //        return BadRequest("External authentication failed");

        //    var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        //    var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
        //    var picture = result.Principal.FindFirst("picture")?.Value;

        //    if (string.IsNullOrEmpty(email))
        //        return BadRequest("Email claim not found");

        //    var token = await _auth.HandleExternalLoginAsync(email, name,picture);
        //    return Ok(new CurrentUser
        //    {
        //        Token = token,
        //        UserName = email,
        //        PhotoUrl = picture
        //    });
        //}
    }
}



//https://localhost:7089/api/OAuth/signin-google



//https://localhost:7089/signin-google



//https://localhost:7089/api/OAuth/google-response

