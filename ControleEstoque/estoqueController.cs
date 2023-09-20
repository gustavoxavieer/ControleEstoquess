using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static ControleEstoques.Pages.IndexModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ControleEstoques
{
    public class EstoqueController : PageModel
    {
        private readonly ILogger<EstoqueController> _logger;
        private readonly AppDbContext _context;

        public EstoqueController(ILogger<EstoqueController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public List<Produto> Produtos { get; set; }
        public List<Categorias> Categoria { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Produtos = new List<Produto>();
            Categoria = new List<Categorias>();

            Produtos = await _context.Produto.ToListAsync();
            Categoria = await _context.Categorias.ToListAsync();

            return Page();
        }


        public class Produto
        {
            [Key]

            public int Id { get; set; }

            public string Nome { get; set; }

            public decimal Preco { get; set; }

            public int Quantidade { get; set; }

            public string Categorias { get; set; }
        }

        public class Categorias
        {
            public int Id { get; set; }
            public string Categoria { get; set; }
        }


        [HttpPost]
        public IActionResult AdicionarCategoria(string campoCategoria)
        {

            try
            {
                if (!string.IsNullOrEmpty(campoCategoria))
                {
                    var novaCategoria = new Categorias
                    {
                        Categoria = campoCategoria
                    };

                    _context.Categorias.Add(novaCategoria);
                    _context.SaveChanges();

                    return new JsonResult(new { success = true, message = "Categoria adicionada com sucesso" });

                }
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = "Erro ao adicionar a categoria" });

            }

            return new JsonResult(new { success = false, message = "Nenhuma categoria adicionada" });

        }

        [HttpPost]
        public IActionResult ExcluirProduto(int Id)
        {
            try
            {
                Console.WriteLine("Chegou em excluir");
                //int teste = id;
                Convert.ToInt32(Id);
                var produtoParaExcluir = _context.Produto.Find(Id);
                if (produtoParaExcluir != null)
                {
                    _context.Produto.Remove(produtoParaExcluir);
                    _context.SaveChanges();
                    return new JsonResult(new { success = true, message = "Produto excluido com sucesso!" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Erro ao excluir a Produto" });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = "Erro ao excluir a Produto" });
            }
        }


        [HttpPost]
        public IActionResult AdicionarProduto(string nomeProduto, int quantidade, decimal preco, string categoria, string categorias)
        {
            //Console.WriteLine("teste");
            try
            {
                if (!string.IsNullOrEmpty(nomeProduto) && quantidade > 0 && preco > 0 && !string.IsNullOrEmpty(categoria))
                {
                    var novoProduto = new Produto
                    {
                        Nome = nomeProduto,
                        Preco = preco,
                        Quantidade = quantidade,                       
                        Categorias = categoria
                    };

                    _context.Produto.Add(novoProduto);
                    _context.SaveChanges();

                    return new JsonResult(new { success = true, message = "Produto adicionado com sucesso" });

                }else
                    return new JsonResult(new { success = false, message = "E necessario colocar Nome, Quantidade, Preço e Categoria." });

            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = "Erro ao adicionar um Produto" });

            }

            return new JsonResult(new { success = false, message = "Nenhum produto adicionada" });
        }

        [HttpPost]
        public IActionResult ExcluirCategoria(int Id)
        {
            try
            {
                var categoriaParaExcluir = _context.Categorias.Find(Id);

                if (categoriaParaExcluir != null)
                {
                    var produtosNaCategoria = _context.Produto.Where(p => p.Categorias == categoriaParaExcluir.Categoria).ToList();

                    if (produtosNaCategoria.Count > 0)
                    {
                        foreach (var produto in produtosNaCategoria)
                        {
                            _context.Produto.Remove(produto);
                        }

                    }

                    _context.Categorias.Remove(categoriaParaExcluir);
                    _context.SaveChanges();

                    return new JsonResult(new { success = true, message = "Categoria e produto's excluídos com sucesso!" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Categoria não encontrada." });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = "Erro ao excluir a categoria." });
            }
        }


        [HttpGet]
        public IActionResult VerificarProdutosNaCategoria(string categoria, int categoriaId)
        {
            try
            {
                var categoriaParaExcluir = _context.Categorias.FirstOrDefault(c => c.Id == categoriaId);

                if (categoriaParaExcluir != null)
                {
                    var produtosNaCategoria = _context.Produto.Where(p => p.Categorias == categoria).ToList();

                    if (produtosNaCategoria.Count > 0)
                    {
                        return new JsonResult(new { success = true, temProdutos = true });
                    }
                    else
                    {
                        return new JsonResult(new { success = true});
                    }

                }else
                {
                    return new JsonResult(new { sucess = false, message = "Erro ao excluir a categoria." });
                }
                
            }
            catch (Exception e)
            {
                return new JsonResult(new { sucess = false, message = "Erro ao excluir a categoria." });
            }

        }


        [HttpPost]
        public IActionResult EditarProduto(int Id, string NomeProduto, int Quantidade, decimal Preco, string Categoria)
        {
            try
            {
                var produto = _context.Produto.Find(Id);

                if (produto != null)
                {
                    produto.Nome = NomeProduto;
                    produto.Quantidade = Quantidade;
                    produto.Preco = Preco;

                    _context.SaveChanges();

                    return new JsonResult(new { success = true, message = "Produto editado com sucesso" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Produto não encontrado" });
                }
            }
            catch (Exception e)
            {
                return new JsonResult(new { success = false, message = "Erro ao editar o produto" });
            }
        }


        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<Produto> Produto { get; set; }
            public DbSet<Categorias> Categorias { get; set; }
        }
    }
}
