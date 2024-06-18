using AppLoginAspCoreHL.GerenciadorArquivos;
using AppLoginAspCoreHL.Libraries.Filtro;
using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppLoginAspCoreHL.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class LivroController : Controller
    {
        
        private ILivroRepository _livroRepository;
        private ICategoriaRepository _categoriaRepository;


        public LivroController(ILivroRepository livroRepository, ICategoriaRepository categoriaRepository)
        {
            _livroRepository = livroRepository;
            _categoriaRepository = categoriaRepository;
        }
        public IActionResult Index()
        {
            return View(_livroRepository.ObterTodosLivros());
        }
        [HttpGet]
        //[ValidateHttpReferer]
        public IActionResult Cadastrar()
        {
            IEnumerable<CategoriaLiv> listaCategoria = _categoriaRepository.ObterTodasCategorias();
            ViewBag.ListaCategorias = new SelectList(listaCategoria, "id", "nome");
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Livro livro, IFormFile file)
        {
            IEnumerable<CategoriaLiv> listaCategoria = _categoriaRepository.ObterTodasCategorias();
            ViewBag.ListaCategorias = new SelectList(listaCategoria, "id", "nome");
            livro.Situacao = LivroTipoConstant.Habilitado;
            if(ModelState.IsValid)
            {
                var Caminho = GerenciadorArquivo.CadastrarImagemProduto(file);
                livro.Imagem = Caminho;
                _livroRepository.Cadastrar(livro);
                TempData["MSG_S"] = "Livro salvo com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Atualizar(int id)
        {
            IEnumerable<CategoriaLiv> listaCategoria = _categoriaRepository.ObterTodasCategorias();
            ViewBag.ListaCategorias = new SelectList(listaCategoria, "id", "nome");
            Models.Livro liv = _livroRepository.ObterLivro(id);
            return View(liv);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Livro livro)
        {
            IEnumerable<CategoriaLiv> listaCategoria = _categoriaRepository.ObterTodasCategorias();
            ViewBag.ListaCategorias = new SelectList(listaCategoria, "id", "nome");
            if (ModelState.IsValid)
            {
                _livroRepository.Atualizar(livro);
                TempData["MSG_S"] = "Livro atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult EditImage(int id)
        {
            Livro liv = _livroRepository.ObterLivro(id);
            return View(liv);
        }
        [HttpPost]
        public IActionResult EditImage([FromForm] Livro livro, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var Caminho = GerenciadorArquivo.CadastrarImagemProduto(file);
                livro.Imagem = Caminho;
                _livroRepository.Atualizar(livro);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        //public IActionResult Excluir(int Id)
        //{
        //    _livroRepository.Excluir(Id);
        //    return RedirectToAction(nameof(Index));

        [ValidateHttpReferer]
        public IActionResult Habilitar(int Id)
        {
            _livroRepository.Habilitar(Id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Desabilitar(int Id)
        {
            _livroRepository.Desabilitar(Id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detalhes(int Id)
        {
            Livro liv = _livroRepository.ObterLivro(Id);
            return View(liv);
        }

    }
}
