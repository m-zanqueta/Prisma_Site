using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface ILivroRepository
    {
        void Cadastrar(Livro livro);
        void Atualizar(Livro livro);
        void Excluir(int Id);
        void Habilitar(int Id);
        void Desabilitar(int Id);

        Livro ObterLivro(int Id);
        IEnumerable<Livro> ObterTodosLivros();
    }
}
