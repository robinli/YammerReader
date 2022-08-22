using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using YammerReader.Shared;
using YammerReader.Server.DAL;

namespace YammerReader.Server.Controllers;
[ApiController]
[Route("[controller]")]
//[AddAuthorization]
public class AuthController : ControllerBase
{

    private AuthDAL AuthDAL;
    public AuthController(AuthDAL authDal)
    {
        AuthDAL = authDal;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync(LoginDto login)
    {
        var user = await AuthDAL.ValidateLogin(login);
        if (user == null)
        {
            return BadRequest("Invalid Credentials");
        }

        var claims = new List<Claim>
        {
           new Claim(ClaimTypes.Name, user.USER_NAME),
            new Claim(ClaimTypes.Email, user.USER_EMAIL),
            new Claim("UserId", user.ID.ToString()),
            new Claim("LoginId", user.LOGIN_ID)

        };
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
        return Ok(user);
    }

    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return Ok(true);
    }

    /// <summary>
    /// 從目前的登入Cookie取回使用者資訊，讓Blazor 第一次讀取時驗證(CustomAuthStateProvider.cs)
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("userinfo")]
    public async Task<IActionResult> UserInfoAsync()
    {
        var user = new AppUserDto();
        user.ID = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(o => o.Type == "UserId")?.Value);
        user.USER_NAME = HttpContext.User.Identity.Name;
        user.USER_EMAIL = HttpContext.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value;
        user.LOGIN_ID = HttpContext.User.Claims.FirstOrDefault(o => o.Type == "LoginId")?.Value;
        foreach (var role in HttpContext.User.Claims.Where(o => o.Type == ClaimTypes.Role))
        {
            user.Roles.Add(role.Value);
        }
        return Ok(user);
    }



    //private async Task RefreshExternalSignIn(User user)
    //{
    //    var claims = new List<Claim>
    //    {
    //        new Claim("userid", user.Id.ToString()),
    //        new Claim(ClaimTypes.Email, user.Email)
    //    };

    //    var claimsIdentity = new ClaimsIdentity(
    //        claims, CookieAuthenticationDefaults.AuthenticationScheme);

    //    var authProperties = new AuthenticationProperties();

    //    HttpContext.User.AddIdentity(claimsIdentity);

    //    await HttpContext.SignOutAsync();

    //    await HttpContext.SignInAsync(
    //        CookieAuthenticationDefaults.AuthenticationScheme,
    //        new ClaimsPrincipal(claimsIdentity),
    //        authProperties);
    //}


}