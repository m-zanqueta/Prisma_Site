using AppLoginAspCoreHL.Libraries.Filtro;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginAspCoreHL.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao(ColaboradorTipoConstant.Gerente)]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;

        public ColaboradorController(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }
        public IActionResult Index()
        {
            return View(_colaboradorRepository.ObterTodosColaboradores());
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar(Models.Colaborador colaborador)
        {
            colaborador.Tipo = ColaboradorTipoConstant.Comum;
            _colaboradorRepository.Cadastrar(colaborador);
            TempData["MSG_S"] = "Registro salvo com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colab = _colaboradorRepository.ObterColaborador(id);
            return View(colab);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador)
        {
            if (ModelState.IsValid)
            {
                _colaboradorRepository.Atualizar(colaborador);
                TempData["MSG_S"] = "Registro salvo com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [ValidateHttpReferer]
        public IActionResult Excluir(int Id)
        {
            _colaboradorRepository.Excluir(Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Promover(int Id)
        {
            _colaboradorRepository.Promover(Id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Rebaixar(int Id)
        {
            _colaboradorRepository.Rebaixar(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}