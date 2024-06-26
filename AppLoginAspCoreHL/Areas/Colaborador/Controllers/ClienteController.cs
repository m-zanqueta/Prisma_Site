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
        private IEnderecoRepository _enderecoRepository;

        public ClienteController(IClienteRepository clienteRepository, IEnderecoRepository enderecoRepository)
        {
            _clienteRepository = clienteRepository;
            _enderecoRepository = enderecoRepository;
        }

        public IActionResult Index()
        {
            return View(_clienteRepository.ObterTodosClientes());
        }

        public IActionResult Endereco(int id)
        {
            return View(_enderecoRepository.ObterEndereco(id));
        }

    }
}
