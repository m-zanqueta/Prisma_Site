using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IClienteRepository
    {
        // Login Cliente
        Cliente Login(string Email, string Senha);
        //CRUD
        void Cadastrar(Cliente cliente);
        void Atualizar (Cliente cliente);
        void Excluir(int Id);

        Cliente ObterCliente(int Id);
        IEnumerable<Cliente> ObterTodosClientes();
    }
}
