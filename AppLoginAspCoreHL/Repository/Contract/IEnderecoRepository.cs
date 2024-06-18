using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IEnderecoRepository
    {
        void Atualizar(Endereco endereco);

        Endereco ObterEndereco(int id);

        IEnumerable<Estado> ObterTodosEstados();
    }
}
