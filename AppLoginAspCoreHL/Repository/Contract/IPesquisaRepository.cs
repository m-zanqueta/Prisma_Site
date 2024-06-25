using AppLoginAspCoreHL.Models;

namespace AppLoginAspCoreHL.Repository.Contract
{
    public interface IPesquisaRepository
    {
        List<PesquisaLivro> PesquisarLivros(string searchString);
        public List<PesquisaLivro> PesquisarLivrosPorCategoria(int id);
    }
}
