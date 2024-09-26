using _1_BaseDTOs.Login;
using MudBlazor;

namespace Product_Catalog.Client.Pages.AccountPages;

public partial class RegisterPage
{
    Register User = new Register();

    private bool _isRegistering;
    private Color _registerButtonColor => _isRegistering ? Color.Tertiary : Color.Surface;

    private async Task HandleRegister()
    {
        _isRegistering = true;
        var result = await AuthenticationService.Register(User);

        await Task.Delay(3000);

        if (result.Flag)
        {
            User = new Register();
            _isRegistering = false;
            Snackbar.Add("Usuário registrado com sucesso!", Severity.Success);
        }
        else
        {
            Snackbar.Add($"Erro ao efetuar registro: {result.Message}");
        }
    }
}
