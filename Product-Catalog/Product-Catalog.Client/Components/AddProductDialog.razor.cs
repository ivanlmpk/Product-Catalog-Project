using _1_BaseDTOs.Category;
using _1_BaseDTOs.Product;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Product_Catalog.Client.Components;

public partial class AddProductDialog
{
    private ProductDTO newProduct = new ProductDTO();
    [Parameter]
    public ProductDTO? Product { get; set; } // Recebe o produto selecionado do diálogo

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    private string buttonText => Product?.Id != 0 && Product != null ? "Atualizar" : "Adicionar";

    private int _userSessionId => Convert.ToInt32(UserSession.UserId);

    private IEnumerable<CategoryDTO> _categories = new List<CategoryDTO>();
    private string _comboBoxLabel = "Categorias";
    private string? _category;
    private CategoryDTO _category2 = new();
    Margin _margin;
    bool _dense;
    bool _disabled;
    bool _readonly;
    bool _placeholder;
    bool _clearable;

    protected override async Task OnInitializedAsync()
    {
        if (Product?.Id != 0 && Product != null)
        {
            newProduct = await ProductService.GetByIdAsync(Product.Id);
        }
        else
        {
            newProduct = new ProductDTO();
        }
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            if (await ProductAlreadyExists())
            {
                Snackbar.Add("Já existe um produto com esse nome.", Severity.Error);
                return;
            }

            if (newProduct.Id == 0)
            {
                var produtoNovo = new ProductDTO
                {
                    Titulo = newProduct.Titulo,
                    Descricao = newProduct.Descricao,
                    Preco = newProduct.Preco,
                    UserId = Convert.ToInt32(UserSession.UserId),
                    CategoryId = GetCategoryIdByName(_category),
                    Data = DateTime.Now
                };

                await ProductService.CreateAsync(produtoNovo);
                Snackbar.Add("Produto adicionado com sucesso.", Severity.Success);

                MudDialog.Close(DialogResult.Ok(true));
            }
            else
            {
                await ProductService.UpdateAsync(newProduct);
                Snackbar.Add("Produto atualizado com sucesso.", Severity.Success);

                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        catch (Exception)
        {
            Snackbar.Add($"Erro ao adicionar produto.", Severity.Warning);
        }
    }

    private void Cancel()
    {
        MudDialog.Close(DialogResult.Cancel());
    }

    private async Task<IEnumerable<string?>> Search(string value, CancellationToken token)
    {
        _categories = await CategoryService.GetAllAsync();

        var resultWithoutStringEmpty = _categories.Where(c => !string.IsNullOrEmpty(c.Titulo));

        var categoriesByUser = resultWithoutStringEmpty.Where(categories => categories.UserId == _userSessionId);

        return categoriesByUser.Select(c => c.Titulo);
    }

    private int GetCategoryIdByName(string? categoryName)
    {
        var category = _categories.Where(x => x.Titulo == categoryName).First();

        return category.Id;
    }

    private Task<bool> ProductAlreadyExists()
    {
        return ProductService.CheckIfExists(newProduct.Titulo!);
    }
}
