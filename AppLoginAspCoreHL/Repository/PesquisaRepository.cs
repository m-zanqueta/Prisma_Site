using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;

namespace AppLoginAspCoreHL.Repository
{
    public class PesquisaRepository : IPesquisaRepository
    {
        private readonly string _conexaoMySQL;
        // Metodo construtor da classe ClienteRepository
        public PesquisaRepository(IConfiguration conf)
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
        public List<PesquisaLivro> PesquisarLivros(string searchString)
        {
            var livros = new List<PesquisaLivro>();

            if (!string.IsNullOrEmpty(searchString))
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    string query = "SELECT titulo_liv, Desc_liv, Autor_liv, nm_cat FROM pesquisaLivro WHERE titulo_liv LIKE @search OR Desc_liv LIKE @search OR Autor_liv LIKE @search OR nm_cat LIKE @search";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchString + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var livro = new PesquisaLivro
                                {
                                    Titulo_liv = reader["titulo_liv"].ToString(),
                                    Desc_liv = reader["Desc_liv"].ToString(),
                                    Autor_liv = reader["Autor_liv"].ToString(),
                                    Nm_cat = reader["nm_cat"].ToString()
                                };
                                livros.Add(livro);
                            }
                        }
                    }
                }
            }

            return livros;
        }
    }
}
