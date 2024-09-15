using _1_BaseDTOs.Login;
using _2_ExternalServices.Authentication;
using Microsoft.AspNetCore.Components;

namespace Product_Catalog.Client.Pages.AccountPages
{
    public partial class LoginPage
    {
        [Inject] protected NavigationManager NavigationManager { get; set; }
        private readonly IESAuthenticationService _authenticationService;
        public Login UserLogin = new Login();

        public LoginPage(NavigationManager navigationManager, IESAuthenticationService authenticationService)
        {
            NavigationManager = navigationManager;
            _authenticationService = authenticationService;
        }

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
    }
}

