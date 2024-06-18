using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IPedidoRepository
    {
        IEnumerable<Pedido> ObterTodosPedidos();
        void Cadastrar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Pedido ObterPedido(int Id);
        void InputValor(int Id);
        void BuscarPedidoPorId (Pedido pedido);
    }
}
