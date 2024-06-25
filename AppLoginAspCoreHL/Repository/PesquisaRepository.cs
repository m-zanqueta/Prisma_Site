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
                    string query = "SELECT Id_liv, titulo_liv, Preco_liv, Image_liv FROM pesquisaLivro WHERE (titulo_liv LIKE @search OR " + 
                    " Autor_liv LIKE @search OR nm_cat LIKE @search) AND Situacao_liv = 'H';";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchString + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var livro = new PesquisaLivro
                                {
                                    Id_liv = (Int32)reader["Id_liv"],
                                    Titulo_liv = reader["titulo_liv"].ToString(),
                                    Image_liv = reader["Image_liv"].ToString(),
                                    Preco_liv = (double)reader["Preco_liv"]
                                };
                                livros.Add(livro);
                            }
                        }
                    }
                }
            }
            return livros;
        }
        public List<PesquisaLivro> PesquisarLivrosPorCategoria(int id)
        {
            var livros = new List<PesquisaLivro>();

            if (id != 0)
            {
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    conexao.Open();
                    string query = "SELECT Id_liv, titulo_liv, Preco_liv, Image_liv FROM pesquisaLivro WHERE Id_cat = @Id AND Situacao_liv = 'H';";
                    using (MySqlCommand cmd = new MySqlCommand(query, conexao))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var livro = new PesquisaLivro
                                {
                                    Id_liv = (Int32)reader["Id_liv"],
                                    Titulo_liv = reader["titulo_liv"].ToString(),
                                    Image_liv = reader["Image_liv"].ToString(),
                                    Preco_liv = (double)reader["Preco_liv"]
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
