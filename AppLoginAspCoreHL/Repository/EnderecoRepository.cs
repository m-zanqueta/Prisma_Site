using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;

namespace AppLoginAspCoreHL.Repository
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly string _conexaoMySQL;
        // Metodo construtor da classe ClienteRepository
        public EnderecoRepository(IConfiguration conf)
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }
        public void Atualizar(Endereco endereco)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("update endereco_usu set cep_usu=@cep, uf_estado=@uf, bairro_usu=@bairro, " +
                    "rua_usu=@rua, logradouro_usu=@log, complemento=@complemento where Id_usu = @Id", conexao);

                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = endereco.id;
                cmd.Parameters.Add("@cep", MySqlDbType.VarChar).Value = endereco.cep;
                cmd.Parameters.Add("@uf", MySqlDbType.VarChar).Value = endereco.estado.uf;
                cmd.Parameters.Add("@bairro", MySqlDbType.VarChar).Value = endereco.bairro;
                cmd.Parameters.Add("@rua", MySqlDbType.VarChar).Value = endereco.rua;
                cmd.Parameters.Add("@log", MySqlDbType.VarChar).Value = endereco.logradouro;
                cmd.Parameters.Add("@complemento", MySqlDbType.VarChar).Value = endereco.complemento;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Endereco ObterEndereco(int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * from endereco_usu as t1 " +
                " INNER JOIN estado as t2 ON t1.uf_estado = t2.uf_estado where Id_usu=@Id", conexao);
                cmd.Parameters.AddWithValue("@Id", id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Endereco e = new Endereco();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    e.id = (Int32)(dr["Id_usu"]);
                    e.cep = (string)(dr["cep_usu"]);
                    e.bairro = (string)(dr["bairro_usu"]);
                    e.rua = (string)(dr["rua_usu"]);
                    e.logradouro = (Int32)(dr["logradouro_usu"]);
                    if (dr["complemento"] is not DBNull)
                    {
                        e.complemento = (string)(dr["complemento"]);
                    }
                    e.estado = new Estado()
                    {
                        uf = Convert.ToString(dr["uf_estado"]),
                        nome = Convert.ToString(dr["nm_estado"]),
                    };
                }
                return e;
            }
        }

        public IEnumerable<Estado> ObterTodosEstados()
        {
            List<Estado> estadoList = new List<Estado>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from estado", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    estadoList.Add(
                        new Estado
                        {
                            uf = (string)(dr["uf_estado"]),
                            nome = (string)(dr["nm_estado"])
                        });
                }
                return estadoList;
            }
        }
    }
}
