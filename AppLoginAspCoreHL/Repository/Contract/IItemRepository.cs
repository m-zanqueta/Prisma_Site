using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IItemRepository
    {
        IEnumerable<ItensPedido> ObterTodosItensPedido(int id);
        void Cadastrar(ItensPedido itensPedido);
        void Atualizar(ItensPedido itensPedido);
        ItensPedido ObterItens(int Id);
        void Excluir(int Id);
    }
}
