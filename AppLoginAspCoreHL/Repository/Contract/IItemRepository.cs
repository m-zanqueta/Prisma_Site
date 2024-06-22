using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IItemRepository
    {
        IEnumerable<ItensPedido> ObterTodosItensPedido(int idPed, int idUsu);
        void Cadastrar(ItensPedido itensPedido);
        void Atualizar(ItensPedido itensPedido);
        ItensPedido ObterItens(int Id);
        void Excluir(int Id);
    }
}
