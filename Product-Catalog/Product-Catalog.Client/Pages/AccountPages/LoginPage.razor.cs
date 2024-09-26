using _1_BaseDTOs.Login;
using _1_BaseDTOs.Session;
using ExternalServices.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace Product_Catalog.Client.Pages.AccountPages
{
    public partial class LoginPage
    {
        Login UserLogin = new Login();
        bool success;
        private bool _validatingLoging;
        private Color _loginButtonColor => _validatingLoging ? Color.Dark : Color.Primary;

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
            _validatingLoging = true;
            StateHasChanged();

            var result = await AuthenticationService.Login(UserLogin);

            await Task.Delay(3000);

            if (result.Flag)
            {
                var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;
                await customAuthStateProvider.UpdateAuthenticationState(new UserSession()
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken
                });
                _validatingLoging = false;
                NavigationManager.NavigateTo("/home", forceLoad: true);
            } 
            else
            {
                _validatingLoging = false;
                Snackbar.Add($"{result.Message}", Severity.Error);
            }
        }

        private void NavToRegisterPage()
        {
            NavigationManager.NavigateTo("/cadastro", forceLoad: true);
        }
    }
}

