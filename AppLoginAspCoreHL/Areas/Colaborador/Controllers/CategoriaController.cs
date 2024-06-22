using AppLoginAspCoreHL.Libraries.Filtro;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginAspCoreHL.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class CategoriaController : Controller
    {
        
        private ICategoriaRepository _categoriaRepository;

        public CategoriaController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        public IActionResult Index()
        {
            return View(_categoriaRepository.ObterTodasCategorias());
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar(Models.CategoriaLiv categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaRepository.Cadastrar(categoria);
                TempData["MSG_S"] = "Categoria salva com sucesso!";
                return RedirectToAction(nameof(Index));
            }
               return View();
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Atualizar(int id)
        {
            Models.CategoriaLiv cat = _categoriaRepository.ObterCategoria(id);
            return View(cat);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.CategoriaLiv categoria)
        {
            if (ModelState.IsValid)
            {
                _categoriaRepository.Atualizar(categoria);
                TempData["MSG_S"] = "Categoria salva com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
