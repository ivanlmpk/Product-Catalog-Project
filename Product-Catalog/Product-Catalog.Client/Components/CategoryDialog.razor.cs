using _1_BaseDTOs.Category;
using _1_BaseDTOs.Session;
using ExternalServices.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;

namespace Product_Catalog.Client.Components;

public partial class CategoryDialog
{
    private CategoryDTO newCategory = new CategoryDTO();
    [Parameter]
    public CategoryDTO? Category { get; set; } // Recebe a categoria selecionado do diálogo

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = new();

    private string buttonText => Category?.Id != 0 && Category != null ? "Atualizar" : "Adicionar";

    private string? _userId;

    protected override async Task OnInitializedAsync()
    {
        var userId = await GetUserId();

        if (!string.IsNullOrEmpty(userId))
            _userId = userId;

        if (Category?.Id != 0 && Category != null)
        {
            newCategory = await CategoryService.GetByIdAsync(Category.Id);
        }
        else
        {
            newCategory = new CategoryDTO();
        }
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            if (newCategory.Id == 0)
            {
                var novaCategoria = new CategoryDTO
                {
                    Titulo = newCategory.Titulo,
                    Descricao = newCategory.Descricao,
                    UserId = Convert.ToInt32(_userId),
                    Data = DateTime.Now
                };

                await CategoryService.CreateAsync(novaCategoria);
                Snackbar.Add("Categoria adicionada com sucesso.", Severity.Success);

                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                await CategoryService.UpdateAsync(newCategory);
                Snackbar.Add("Categoria atualizada com sucesso.", Severity.Success);

                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add("Erro ao adicionar categoria.", Severity.Warning);
            Console.WriteLine($"Erro ao adicionar categoria: {ex.Message}");
        }
    }

    private void Cancel()
    {
        MudDialog.Close(DialogResult.Cancel());
    }

    private async Task<string> GetUserId()
    {
        var customAuthStateProvider = (CustomAuthenticationStateProvider)AuthStateProvider;

        var getAuthenticationState = await customAuthStateProvider.GetAuthenticationStateAsync();

        if (!getAuthenticationState.User!.Identity!.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("Usuário deslogado.");
        }

        var userId = getAuthenticationState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("ID do usuário não encontrado.");
        }

        return userId!;
    }
}
