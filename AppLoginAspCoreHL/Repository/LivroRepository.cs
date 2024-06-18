using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;



namespace AppLoginAspCoreHL.Repository
{
    public class LivroRepository : ILivroRepository
    {
        private readonly string _conexaoMySQL;
        // Metodo construtor da classe ClienteRepository
        public LivroRepository(IConfiguration conf)
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
        public void Atualizar(Livro livro)
        {
            var preco = livro.Preco.ToString();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update livros set Id_cat=@Id_cat, QtEstoque=@QtEstoque, Desc_liv=@Desc_liv, Image_liv=@Image_liv, Nu_paginas=@Nu_paginas, Preco_liv=@Preco_liv, Titulo_liv=@Titulo_liv, Autor_liv=@Autor_liv, Situacao_liv=@Situacao_liv where Id_liv=@Id_liv", conexao);
                cmd.Parameters.Add("@Id_liv", MySqlDbType.VarChar).Value = livro.Id;
                cmd.Parameters.Add("@Id_cat", MySqlDbType.VarChar).Value = livro.categoria.id;
                cmd.Parameters.Add("@QtEstoque", MySqlDbType.VarChar).Value = livro.QuantidadeEstq;
                cmd.Parameters.Add("@Desc_liv", MySqlDbType.VarChar).Value = livro.Descricao;
                cmd.Parameters.Add("@Image_liv", MySqlDbType.VarChar).Value = livro.Imagem;
                cmd.Parameters.Add("@Nu_paginas", MySqlDbType.VarChar).Value = livro.NumeroPags;
                cmd.Parameters.Add("@Preco_liv", MySqlDbType.VarChar).Value = preco.Replace(".", "").Replace(",", ".");
                cmd.Parameters.Add("@Titulo_liv", MySqlDbType.VarChar).Value = livro.Titulo;
                cmd.Parameters.Add("@Autor_liv", MySqlDbType.VarChar).Value = livro.Autor;
                cmd.Parameters.Add("@Situacao_liv", MySqlDbType.VarChar).Value = livro.Situacao;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Livro livro)
        {
            string situacao = LivroTipoConstant.Habilitado;
            var preco = livro.Preco.ToString();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("call pcd_CadLivro(@Id_cat, @QtEstoque, @Desc_liv, @Image_liv, @Nu_paginas, @Preco_liv, @Titulo_liv, @Autor_liv, @Situacao);", conexao);

                cmd.Parameters.Add("@Id_cat", MySqlDbType.VarChar).Value = livro.categoria.id;
                cmd.Parameters.Add("@QtEstoque", MySqlDbType.VarChar).Value = livro.QuantidadeEstq;
                cmd.Parameters.Add("@Desc_liv", MySqlDbType.VarChar).Value = livro.Descricao;
                cmd.Parameters.Add("@Image_liv", MySqlDbType.VarChar).Value = livro.Imagem;
                cmd.Parameters.Add("@Nu_paginas", MySqlDbType.VarChar).Value = livro.NumeroPags;
                cmd.Parameters.Add("@Preco_liv", MySqlDbType.VarChar).Value = preco.Replace(".", "").Replace(",", ".");
                cmd.Parameters.Add("@Titulo_liv", MySqlDbType.VarChar).Value = livro.Titulo;
                cmd.Parameters.Add("@Autor_liv", MySqlDbType.VarChar).Value = livro.Autor;
                cmd.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = situacao;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from livros where Id_liv=@Id_liv", conexao);
                cmd.Parameters.AddWithValue("Id_liv", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Livro ObterLivro(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from livros as t1 inner join categoria as t2 where t1.Id_liv=@Id_liv and t1.Id_cat = t2.Id_cat", conexao);
                cmd.Parameters.AddWithValue("@Id_liv", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Livro livro = new Livro();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    livro.Id = (Int32)(dr["Id_liv"]);
                    livro.categoria = new CategoriaLiv()
                    {
                        id = (Int32)(dr["Id_cat"]),
                        nome = (string)(dr["nm_cat"]),
                    };
                    livro.QuantidadeEstq = (Int32)(dr["QtEstoque"]);
                    livro.Descricao = (string)(dr["Desc_liv"]);
                    livro.Imagem = (string)(dr["Image_liv"]);
                    livro.NumeroPags = (Int32)(dr["Nu_paginas"]);
                    livro.Preco = (double)(dr["Preco_liv"]);
                    livro.Titulo = (string)(dr["Titulo_liv"]);
                    livro.Autor = (string)(dr["Autor_liv"]);
                    livro.Situacao = (string)(dr["Situacao_liv"]);
                }
                return livro;
            }
        }

        public IEnumerable<Livro> ObterTodosLivros()
        {
            List<Livro> LivroList = new List<Livro>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from livros as t1 inner join categoria as t2 where t1.Id_cat = t2.Id_cat", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    LivroList.Add(
                        new Livro
                        {
                            Id = (Int32)(dr["Id_liv"]),
                            categoria = new CategoriaLiv()
                            {
                                id = (Int32)(dr["Id_cat"]),
                                nome = (string)(dr["nm_cat"]),
                            },
                            QuantidadeEstq = (Int32)(dr["QtEstoque"]),
                            Descricao = (string)(dr["Desc_liv"]),
                            Imagem = (string)(dr["Image_liv"]),
                            NumeroPags = (Int32)(dr["Nu_paginas"]),
                            Preco = (double)(dr["Preco_liv"]),
                            Titulo = (string)(dr["Titulo_liv"]),
                            Autor = (string)(dr["Autor_liv"]),
                            Situacao = (string)(dr["Situacao_liv"])
                        });
                }
                return LivroList;
            }
        }
        public void Habilitar(int Id)
        {
            string situacao = LivroTipoConstant.Habilitado;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update livros set Situacao_liv = @Situacao_liv where Id_liv=@Id", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@Situacao_liv", MySqlDbType.VarChar).Value = situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Desabilitar(int Id)
        {
            string situacao = LivroTipoConstant.Desabilitado;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update livros set Situacao_liv = @Situacao_liv where Id_liv=@Id", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@Situacao_liv", MySqlDbType.VarChar).Value = situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
    }
}
