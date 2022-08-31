using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;
using YammerReader.Client.DataModel;
using YammerReader.Client.Library;
using YammerReader.Client.Providers;
using YammerReader.Client.Shared;
using YammerReader.Shared;

namespace YammerReader.Client.Shared;
//[AddAuthorization]
public partial class LoginDisplay : CommonBlazorBase
{


    [Inject]
    AuthenticationStateProvider _authStateProvider { get; set; }
    [Inject]
    NavigationManager _navigationManager { get; set; }

    string UserId = "";
    string LoginId = "";

    protected override async Task OnInitializedAsync()
    {
        var user = (await _authStateProvider.GetAuthenticationStateAsync()).User;
        if (user.Identity.IsAuthenticated)
        {
            UserId = user.Claims.SingleOrDefault(o => o.Type == "UserId")?.Value;
            LoginId = user.Claims.SingleOrDefault(o => o.Type == "LoginId")?.Value;
        }
        await base.OnInitializedAsync();
    }


    private async Task SignOut()
    {
        var result = await PostAsJsonAsync<bool>("auth/logout", null);
        if (result == true)
        {
            (_authStateProvider as CustomAuthStateProvider).ClearAuthInfo();
            _navigationManager.NavigateTo("/login");
        }


    }


}

