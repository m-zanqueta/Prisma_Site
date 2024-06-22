using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IPedidoRepository
    {
        IEnumerable<Pedido> ObterTodosPedidos();
        void Cadastrar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Pedido ObterPedido(int Id);
        void InputValor(double valor, int id);
        void BuscarPedidoPorId (Pedido pedido);
        public void Reabrir(int Id);
        public void Finalizar(int Id);
    }
}
