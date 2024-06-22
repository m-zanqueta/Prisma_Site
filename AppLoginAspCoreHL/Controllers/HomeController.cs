using AppLoginAspCoreHL.CarrinhoCompra;
using AppLoginAspCoreHL.Libraries.Filtro;
using AppLoginAspCoreHL.Libraries.Login;
using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace AppLoginAspCoreHL.Controllers
{
    public class HomeController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IEnderecoRepository _enderecoRepository;
        private ILivroRepository _livroRepository;
        private LoginCliente _loginCliente;
        private CookieCarrinhoCompra _cookieCarrinhoCompra;
        private IPedidoRepository _pedidoRepository;
        private IItemRepository _itemRepository;
        private IPesquisaRepository _pesquisaRepository;

        

        public HomeController(IClienteRepository clienteRepository, LoginCliente loginCliente, IEnderecoRepository enderecoRepository, CookieCarrinhoCompra cookieCarrinhoCompra, ILivroRepository livroRepository, IPedidoRepository pedidoRepository, IItemRepository itemRepository, IPesquisaRepository pesquisaRepository)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _enderecoRepository = enderecoRepository;
            _cookieCarrinhoCompra = cookieCarrinhoCompra;
            _livroRepository = livroRepository;
            _pedidoRepository = pedidoRepository;
            _itemRepository = itemRepository;
            _pesquisaRepository = pesquisaRepository;
        }

        public IActionResult Index()
        {
            return View(_livroRepository.ObterTodosLivros());
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Login, cliente.Senha);

            if(clienteDB.Login != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(PainelUsuario)));
            }
            else
            {
                ViewData["MSG_E"] = "Usuário não localizado, por favor verifique e-mail e senha digitado";
                return View();            
            }
        }

        public IActionResult PainelUsuario()
        {
            Cliente cli = _clienteRepository.ObterCliente(_loginCliente.GetCliente().Id);
            return View(cli);
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult EditUsuario(int id)
        {
            Cliente cli = _clienteRepository.ObterCliente(id);
            return View(cli);
        }
        [HttpPost]
        public IActionResult EditUsuario([FromForm] Cliente cli)
        {
            if (ModelState.IsValid)
            {
                _clienteRepository.Atualizar(cli);
                TempData["MSG_S"] = "Registro salvo com sucesso!";
                return RedirectToAction(nameof(PainelUsuario));
            }
            return View();
        }
        [ClienteAutorizacao]
        public IActionResult LogoutCliente()
        {
            _loginCliente.Logout();
            return new RedirectResult(Url.Action(nameof(Index)));
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            IEnumerable<Estado> listaEstados = _enderecoRepository.ObterTodosEstados();
            ViewBag.ListaEstados = new SelectList(listaEstados, "uf", "nome");
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente) 
        {
            var listaEstados = _enderecoRepository.ObterTodosEstados();
            ViewBag.ListaEstados = new SelectList(listaEstados, "uf", "nome");
            if (ModelState.IsValid)
            {
                _clienteRepository.Cadastrar(cliente);
                return RedirectToAction(nameof(Login));
            }
            return View();
                
        }

        public IActionResult PainelEndereco()
        {
            Endereco en = _enderecoRepository.ObterEndereco(_loginCliente.GetCliente().Id);
            return View(en);
        }

        [HttpGet]
        public IActionResult EditEndereco(int id)
        {
            Endereco en = _enderecoRepository.ObterEndereco(id);
            IEnumerable<Estado> listaEstados = _enderecoRepository.ObterTodosEstados();
            ViewBag.ListaEstados = new SelectList(listaEstados, "uf", "nome");
            return View(en);
        }

        [HttpPost]
        public IActionResult EditEndereco([FromForm] Endereco endereco)
        {
            var listaEstados = _enderecoRepository.ObterTodosEstados();
            ViewBag.ListaEstados = new SelectList(listaEstados, "uf", "nome");
            if (ModelState.IsValid)
            {
                _enderecoRepository.Atualizar(endereco);
                return RedirectToAction(nameof(PainelEndereco));
            }
            return View();
        }
        public IActionResult AdicionarItem(int id)
        {
            Livro produto = _livroRepository.ObterLivro(id);

            if (produto == null)
            {
                return View("Não existe item");
            }
            else
            {
                var item = new Livro()
                {
                    Id = id,
                    QuantidadeEstq = 1,
                    Imagem = produto.Imagem,
                    Titulo = produto.Titulo,
                    Preco = produto.Preco
                };
                _cookieCarrinhoCompra.Cadastrar(item);
                return RedirectToAction(nameof(Carrinho));
            }
        }
        public IActionResult DiminuirItem(int id)
        {
            Livro produto = _livroRepository.ObterLivro(id);

            if (produto == null)
            {
                return View("Não existe item");
            }
            else
            {
                var item = new Livro()
                {
                    Id = id,
                    QuantidadeEstq = 1,
                    Imagem = produto.Imagem,
                    Titulo = produto.Titulo,
                    Preco = produto.Preco
                };
                _cookieCarrinhoCompra.Diminuir(item);
                return RedirectToAction(nameof(Carrinho));
            }
        }
        public IActionResult Carrinho()
        {
            return View(_cookieCarrinhoCompra.Consultar());
        }
        public IActionResult RemoverItem(int id)
        {
            _cookieCarrinhoCompra.Remover(new Livro() { Id = id });
            return RedirectToAction(nameof(Carrinho));
        }

        DateTime data;
        [ClienteAutorizacao]
        public IActionResult SalvarCarrinho(Pedido pedido)
        {
            List<Livro> carrinho = _cookieCarrinhoCompra.Consultar();

            Pedido mdE = new Pedido();
            ItensPedido mdI = new ItensPedido();

            mdE.Id_usu = _loginCliente.GetCliente().Id;
            mdE.Horario_ped = data;
            mdE.Situacao = PedidoTipoConstant.Andamento;


            _pedidoRepository.Cadastrar(mdE);
            _pedidoRepository.BuscarPedidoPorId(pedido);

            double valorTotalItens = 0;
            for (int i = 0; i < carrinho.Count; i++)
            {
                mdI.Id_pedido = Convert.ToInt32(pedido.Id_pedido);
                mdI.Id_liv = Convert.ToInt32(carrinho[i].Id);
                mdI.QtItens = Convert.ToInt32(carrinho[i].QuantidadeEstq);
                mdI.VlTotal = Convert.ToDouble(carrinho[i].Preco * carrinho[i].QuantidadeEstq);
                valorTotalItens += mdI.VlTotal;
                _itemRepository.Cadastrar(mdI);
            }
            
            _pedidoRepository.InputValor(Math.Round(valorTotalItens, 2), pedido.Id_pedido);

            _cookieCarrinhoCompra.RemoverTodos();
            return View(_itemRepository.ObterTodosItensPedido(pedido.Id_pedido, mdE.Id_usu));

        }
        [ClienteAutorizacao]
        public IActionResult DetalhesPedido()
        {
            return View();
        }
        //public IActionResult Search(string searchString)
        //{
        //    var livros = new List<PesquisaLivro>();

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        using (MySqlConnection conexao = new MySqlConnection(_connectionString))
        //        {
        //            conexao.Open();
        //            string query = "SELECT titulo_liv, Desc_liv, Autor_liv, nm_cat FROM pesquisaLivro WHERE titulo_liv LIKE @search OR Desc_liv LIKE @search OR Autor_liv LIKE @search OR nm_cat LIKE @search";
        //            using (MySqlCommand cmd = new MySqlCommand(query, conexao))
        //            {
        //                cmd.Parameters.AddWithValue("@search", "%" + searchString + "%");

        //                using (MySqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var livro = new PesquisaLivro
        //                        {
        //                            Titulo_liv = reader["titulo_liv"].ToString(),
        //                            Desc_liv = reader["Desc_liv"].ToString(),
        //                            Autor_liv = reader["Autor_liv"].ToString(),
        //                            Nm_cat = reader["nm_cat"].ToString()
        //                        };
        //                        livros.Add(livro);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return View(livros);
        //}
        [HttpGet]
        public IActionResult Search(string searchString)
        {
            var livros = _pesquisaRepository.PesquisarLivros(searchString);
            return View(livros);
        }
        public IActionResult Detalhes(int Id)
        {
            Livro liv = _livroRepository.ObterLivro(Id);
            return View(liv);
        }

        public IActionResult SaibaMais()
        {
            return View();
        }
    }
}
