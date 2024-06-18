using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface ICategoriaRepository
    {
        void Cadastrar(CategoriaLiv cat);
        void Atualizar(CategoriaLiv cat);
        void Excluir(int Id);

        CategoriaLiv ObterCategoria(int Id);
        IEnumerable<CategoriaLiv> ObterTodasCategorias();
    }
}
