using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Client.Providers;
using YammerReader.Client.Shared;
using YammerReader.Shared;

namespace YammerReader.Client.Pages;
//[AddAuthorization]
public partial class Login : CommonBlazorBase
{

    private LoginDto Model { get; set; } = new LoginDto();
    bool LoginFail = false;


    [Inject]
    AuthenticationStateProvider _authStateProvider { get; set; }
    [Inject]
    NavigationManager _navigationManager { get; set; }
    private async Task UserLogin()
    {
        AppUserDto? result = await PostAsJsonAsync<AppUserDto>("auth/login", Model);
        if (result != null)
        {
            (_authStateProvider as CustomAuthStateProvider).SetAuthInfo(result);
            //await _localStorageService.SetItemAsStringAsync("isauthenticated", "true");
            _navigationManager.NavigateTo("/", true);
        }

        LoginFail = result == null;


    }


}

