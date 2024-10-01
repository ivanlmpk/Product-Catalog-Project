using _1_BaseDTOs.Category;
using MudBlazor;
using Product_Catalog.Client.Components;

namespace Product_Catalog.Client.Pages.CategoryPages;

public partial class CategoryCatalogPage
{
    private IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();

    private int _userSessionId => Convert.ToInt32(UserSession.UserId);

    private string? _searchString;

    private bool _sortNameByLength;

    private bool _isLoading;

    private List<string> _events = new();

    private CategoryDTO _selectedItem = new();

    private Func<CategoryDTO, object?> _sortBy => x =>
    {
        if (_sortNameByLength)
            return x.Titulo!.Length;
        else
            return x.Titulo;
    };

    private Func<CategoryDTO, bool> _quickFilter => x =>
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;

        if ($"{x.Id}".Contains(_searchString))
            return true;

        if (x.Titulo!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Descricao!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;

        if (x.Data.ToShortDateString().Contains(_searchString))
            return true;

        return false;
    };

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        await Task.Delay(3000);
        var result = await CategoryService.GetAllAsync();
        //TODO: Criar end-point para pegar os produtos por Usuario
        var categoriesByUser = result.Where(categories => categories.UserId == _userSessionId);
        _categories = categoriesByUser;

        _isLoading = false;
    }

    private async Task OpenAddCategoryDialog()
    {
        var parameters = new DialogParameters();
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
        var dialog = DialogService.Show<CategoryDialog>("Adicionar Categoria", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            // Atualizar a lista de categorias após adicionar uma nova categoria
            _categories = await CategoryService.GetAllAsync();
        }
    }

    private async Task OpenUpdateCategoryDialog(CategoryDTO selectedCategory)
    {
        var parameters = new DialogParameters
        {
            { "Category", selectedCategory }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
        var dialog = DialogService.Show<CategoryDialog>("Atualizar Categoria", parameters, options);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            // Atualizar a lista de categorias após adicionar um nova categoria
            await RefreshPage();
        }
    }

    private async Task OpenUpdateDialog()
    {
        if (_selectedItem == null || _selectedItem.Id == 0)
        {
            Snackbar.Add("Selecione uma categoria para atualizar.", Severity.Warning);
            return;
        }

        await OpenUpdateCategoryDialog(_selectedItem);
    }

    private async Task DeleteCategory()
    {
        if (_selectedItem == null || _selectedItem.Id == 0)
        {
            Snackbar.Add("Selecione uma categoria para deletar.", Severity.Warning);
            return;
        }

        await CategoryService.DeleteAsync(_selectedItem.Id);

        Snackbar.Add("categoria excluída com sucesso.", Severity.Success);
        await RefreshPage();
    }

    // events
    void RowClicked(DataGridRowClickEventArgs<CategoryDTO> args)
    {
        _events.Insert(0, $"Event = RowClick, Index = {args.RowIndex}, Data = {System.Text.Json.JsonSerializer.Serialize(args.Item)}");

        _selectedItem = args.Item;
    }

    void RowRightClicked(DataGridRowClickEventArgs<CategoryDTO> args)
    {
        _events.Insert(0, $"Event = RowRightClick, Index = {args.RowIndex}, Data = {System.Text.Json.JsonSerializer.Serialize(args.Item)}");
    }

    void SelectedItemsChanged(HashSet<CategoryDTO> items)
    {
        _events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
        _selectedItem = items.Any() ? items.First() : new CategoryDTO();
    }

    async Task RefreshPage()
    {
        _categories = await CategoryService.GetAllAsync();
        StateHasChanged();
    }
}
