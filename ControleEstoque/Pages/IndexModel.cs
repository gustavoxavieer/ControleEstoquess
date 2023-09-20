//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.Extensions.Logging;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using static ControleEstoque.Pages.IndexModel;
//using System.ComponentModel.DataAnnotations;

//namespace ControleEstoque.Pages
//{
//    public class IndexModel : PageModel
//    {
//        private readonly ILogger<IndexModel> _logger;
//        private readonly AppDbContext _context;

//        public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
//        {
//            _logger = logger;
//            _context = context;
//        }

//        public List<Produto> Produtos { get; set; }
//        public List<Categoria> Categorias { get; set; }

//        public async Task<IActionResult> OnGetAsync()
//        {
//            Produtos = new List<Produto>();
//            Categorias = new List<Categoria>();

//            Produtos = await _context.Produto.ToListAsync();
//            Categorias = await _context.Categorias.ToListAsync();

//            return Page();
//        }


//        public class Produto
//        {
//            [Key]
//            [Display(Name = "Nome do Produto")]
//            public string Nome { get; set; }

//            public int Id { get; set; }

//            [Display(Name = "Quantidade")]
//            public int Quantidade { get; set; }

//            [Display(Name = "Preço")]
//            public decimal Preco { get; set; }
//            public string categoria { get; set; }
//        }

//        public class Categoria
//        {
//            public int Id { get; set; }
//            public string categoria { get; set; }
//        }


//        [HttpPost]
//        public IActionResult AdicionarCategoria(string campoCategoria)
//        {

//            try
//            {
//                if (!string.IsNullOrEmpty(campoCategoria))
//                {
//                    var novaCategoria = new Categoria
//                    {
//                        categoria = campoCategoria
//                    };

//                    _context.Categorias.Add(novaCategoria);
//                    _context.SaveChanges();
//                }
//            }
//            catch (Exception e)
//            {

//            }

//            return RedirectToPage();
//        }


//        [HttpPost]
//        public IActionResult ExcluirProduto(int id)
//        {
//            try
//            {
//                var produtoParaExcluir = _context.Produto.Find(id);
//                if (produtoParaExcluir != null)
//                {
//                    _context.Produto.Remove(produtoParaExcluir);
//                    _context.SaveChanges();
//                    return Ok();
//                }
//                else
//                {
//                    return NotFound();
//                }
//            }
//            catch (Exception e)
//            {
//                return StatusCode(500);
//            }
//        }


//        private IActionResult Ok()
//        {
//            throw new NotImplementedException();
//        }



//        [HttpPost]
//        public IActionResult AdicionarProduto(string nomeProduto, int quantidade, decimal preco, string categoria)
//        {
//            Console.WriteLine("teste");
//            try
//            {
//                if (!string.IsNullOrEmpty(nomeProduto) && quantidade > 0 && preco > 0 && !string.IsNullOrEmpty(categoria))
//                {
//                    var novoProduto = new Produto
//                    {
//                        Nome = nomeProduto,
//                        Quantidade = quantidade,
//                        Preco = preco,
//                        categoria = categoria
//                    };

//                    _context.Produto.Add(novoProduto);
//                    _context.SaveChanges();
//                }
//            }
//            catch (Exception e)
//            {
//            }

//            return RedirectToPage();
//        }


//        public class AppDbContext : DbContext
//        {
//            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
//            {
//            }

//            public DbSet<Produto> Produto { get; set; }
//            public DbSet<Categoria> Categorias { get; set; }
//        }
//    }



//}