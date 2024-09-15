using _1_BaseDTOs.Login;
using ExternalServices.Authentication;
using Microsoft.AspNetCore.Components;

namespace Product_Catalog.Client.Pages.AccountPages
{
    public partial class LoginPage
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IESAuthenticationService _authenticationService { get; set; }

        public Login UserLogin = new Login();

        private async Task HandleLogin()
        {
            var result = await _authenticationService.Login(UserLogin);

            if (result.Flag)
            {
                //var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;
                //await customAuthStateProvider.UpdateAuthenticationState(new UserSession()
                //{
                //    Token = result.Token,
                //    RefreshToken = result.RefreshToken
                //});
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
        }

        private void NavToRegisterPage()
        {
            NavigationManager.NavigateTo("/cadastro", forceLoad: true);
        }
    }
}

