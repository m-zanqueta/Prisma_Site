using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginAspCoreHL.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class PedidoController : Controller
    {
        private IPedidoRepository _pedidoRepository;
        private IItemRepository _itemRepository;

        public PedidoController(IPedidoRepository pedidoRepository, IItemRepository itemRepository)
        {
            _pedidoRepository = pedidoRepository;
            _itemRepository = itemRepository;
        }
        public IActionResult Index()
        {
            return View(_pedidoRepository.ObterTodosPedidos());
        }
        public IActionResult FinalizarPedido(int id)
        {
            _pedidoRepository.Finalizar(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ReabrirPedido(int id)
        {
            _pedidoRepository.Reabrir(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Detalhes(int id)
        {
            Pedido ped = _pedidoRepository.ObterPedido(id);
            int idUsu = ped.Id_usu;
            ViewData["Nm_ped"] = ped.Id_pedido;
            return View(_itemRepository.ObterTodosItensPedido(id, idUsu));
        }
    }
}
