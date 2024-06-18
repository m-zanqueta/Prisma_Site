using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLoginAspCoreHL.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao]
    public class ClienteController : Controller
    {

        private IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public IActionResult Index()
        {
            return View(_clienteRepository.ObterTodosClientes());
        }

    }
}
