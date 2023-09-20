using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using static ControleEstoques.EstoqueController;

namespace ControleEstoques.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string NomeProduto { get; set; }

        [BindProperty]
        public int Quantidade { get; set; }

        [BindProperty]
        public decimal Preco { get; set; }

        [BindProperty]
        public string Categoria { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public List<Produto> Produtos { get; set; }
        public List<Categorias> Categorias { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Produtos = await _context.Produto.ToListAsync();
            Categorias = await _context.Categorias.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var novoProduto = new Produto
                {
                    Nome = NomeProduto,
                    Quantidade = Quantidade,
                    Preco = Preco,
                    Categorias = Categoria
                };

                _context.Produto.Add(novoProduto);
                await _context.SaveChangesAsync();
            }

            Produtos = await _context.Produto.ToListAsync();
            Categorias = await _context.Categorias.ToListAsync();

            return Page();
        }

        public IActionResult OnPostExcluirProduto(int Id)
        {
            var produtoParaExcluir = _context.Produto.Find(Id);
            if (produtoParaExcluir != null)
            {
                _context.Produto.Remove(produtoParaExcluir);
                _context.SaveChanges();
            }

            return Page();
        }

    }
}
