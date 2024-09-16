using _1_BaseDTOs.Login;
using _1_BaseDTOs.Session;
using ExternalServices.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Product_Catalog.Client.Pages.AccountPages
{
    public partial class LoginPage
    {
        Login UserLogin = new Login();
        bool success;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("setPasswordField", "passwordField");
            }
        }

        private async Task HandleLogin()
        {
            success = true;
            StateHasChanged();

            var result = await AuthenticationService.Login(UserLogin);

            if (result.Flag)
            {
                var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;
                await customAuthStateProvider.UpdateAuthenticationState(new UserSession()
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken
                });
                NavigationManager.NavigateTo("/home", forceLoad: true);
            }
        }

        private void NavToRegisterPage()
        {
            NavigationManager.NavigateTo("/cadastro", forceLoad: true);
        }
    }
}

