using System.Net.Http.Json;
using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using YammerReader.Shared;

namespace YammerReader.Client.Providers;
//[AddAuthorization]
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
    private IHttpClientFactory HttpClientFactory;
    public CustomAuthStateProvider(IHttpClientFactory httpClientFactory)
    {
        HttpClientFactory = httpClientFactory;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var Http = HttpClientFactory.CreateClient("YammerReader.PublicServerAPI");
        var response = await Http.GetAsync("auth/userinfo");
        if (response.IsSuccessStatusCode)
        {
            //ReadAsStringAsync 可以先讀出Json 再轉，比較好debug
            //var result = await response.Content.ReadFromJsonAsync<T>();
            string json = await response.Content.ReadAsStringAsync();
            if (json != "")
            {
                var userDto = JsonSerializer.Deserialize<AppUserDto>(json);
                SetAuthInfo(userDto);
            }

        }
        return await GetClaimsPrincipalAsync();
    }

    public async Task<AuthenticationState> GetClaimsPrincipalAsync()
    {
        await Task.Delay(0);
        return new AuthenticationState(claimsPrincipal);
    }


    public void SetAuthInfo(AppUserDto user)
    {
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

        var identity = new ClaimsIdentity(claims, "AuthCookie");
        claimsPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(GetClaimsPrincipalAsync());
    }

    public void ClearAuthInfo()
    {
        claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetClaimsPrincipalAsync());
    }
}