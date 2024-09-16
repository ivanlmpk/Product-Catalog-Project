using _1_BaseDTOs.Login;
using _1_BaseDTOs.Session;
using ExternalServices.Helpers;
using Microsoft.AspNetCore.Components;

namespace Product_Catalog.Client.Pages.AccountPages
{
    public partial class LoginPage
    {
        Login UserLogin = new Login();

        private async Task HandleLogin()
        {
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

