using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;

namespace AppLoginAspCoreHL.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly string _conexaoMySQL;
        // Metodo construtor da classe ClienteRepository
        public CategoriaRepository(IConfiguration conf)
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
        public void Atualizar(CategoriaLiv cat)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update categoria set nm_cat=@Nome where Id_cat=@Id", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = cat.id;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cat.nome;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(CategoriaLiv cat)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("insert into categoria values(default, @Nome)", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cat.nome;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from categoria where Id_cat=@Id ", conexao);
                cmd.Parameters.AddWithValue("Id", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public CategoriaLiv ObterCategoria(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from categoria where Id_cat=@Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                CategoriaLiv cat = new CategoriaLiv();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cat.id = (Int32)(dr["Id_cat"]);
                    cat.nome = (string)(dr["nm_cat"]);
                }
                return cat;
            }
        }

        public IEnumerable<CategoriaLiv> ObterTodasCategorias()
        {
            List<CategoriaLiv> colabList = new List<CategoriaLiv>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from categoria", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    colabList.Add(
                        new CategoriaLiv
                        {
                            id = Convert.ToInt32(dr["Id_cat"]),
                            nome = (string)(dr["nm_cat"]),
                        });
                }
                return colabList;
            }
        }
    }
}
