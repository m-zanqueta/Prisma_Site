using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository.Contract;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;

namespace AppLoginAspCoreHL.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _conexaoMySQL;
        // Metodo construtor da classe ClienteRepository
        public ClienteRepository(IConfiguration conf) 
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("update usuario set Nome_usu=@Nome, DataNasc=@Nascimento, CPF_usu=@CPF, " +
                    " tel_usu=@Telefone, Login_usu=@Login, Senha_usu=@Senha where Id_usu=@Id", conexao);

                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = cliente.Id;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@Nascimento", MySqlDbType.DateTime).Value = cliente.Nascimento.ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Login", MySqlDbType.VarChar).Value = cliente.Login;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cliente.Senha;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }

        }

        public void Cadastrar(Cliente cliente)
        {
            string id;
            using(var conexao = new MySqlConnection(_conexaoMySQL)) 
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("call pcd_CadUsu(@Nome, @Login, @Senha, @Nascimento, @CPF, @Telefone);SELECT LAST_INSERT_ID();", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@Nascimento", MySqlDbType.DateTime).Value = cliente.Nascimento.ToString("yyyy/MM/dd");
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Login", MySqlDbType.VarChar).Value = cliente.Login;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cliente.Senha;

                id = Convert.ToString(cmd.ExecuteScalar());

                MySqlCommand cmd1 = new MySqlCommand("call pcd_CadEndereco(@Id, @CEP, @UF, @Bairro, @Rua, @Log, @Complemento);", conexao);

                cmd1.Parameters.Add("@Id", MySqlDbType.VarChar).Value = id;
                cmd1.Parameters.Add("@CEP", MySqlDbType.VarChar).Value = cliente.Endereco.cep;
                cmd1.Parameters.Add("@UF", MySqlDbType.VarChar).Value = cliente.Endereco.estado.uf;
                cmd1.Parameters.Add("@Bairro", MySqlDbType.VarChar).Value = cliente.Endereco.bairro;
                cmd1.Parameters.Add("@Rua", MySqlDbType.VarChar).Value = cliente.Endereco.rua;
                cmd1.Parameters.Add("@Log", MySqlDbType.VarChar).Value = cliente.Endereco.logradouro;
                cmd1.Parameters.Add("@Complemento", MySqlDbType.VarChar).Value = cliente.Endereco.complemento;

                cmd1.ExecuteNonQuery();

                conexao.Close();
            }

        }

        public void Excluir(int Id)
        {
            using (var  conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from usuario where Id_usu=@Id", conexao);
                cmd.Parameters.AddWithValue("Id", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Cliente Login(string Login, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from usuario where Login_usu = @Login and Senha_usu = @Senha", conexao);

                cmd.Parameters.Add("@Login", MySqlDbType.VarChar).Value = Login;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read()) 
                {
                    cliente.Id = Convert.ToInt32(dr["Id_usu"]);
                    cliente.Nome = Convert.ToString(dr["Nome_usu"]);
                    cliente.Nascimento = Convert.ToDateTime(dr["DataNasc"]);

                    cliente.CPF = Convert.ToString(dr["CPF_usu"]);
                    cliente.Telefone = Convert.ToString(dr["tel_usu"]);

                    cliente.Login = Convert.ToString(dr["Login_usu"]);
                    cliente.Senha = Convert.ToString(dr["Senha_usu"]);
                }
                return cliente;
            }
        }

        public Cliente ObterCliente(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from usuario where Id_usu=@Id", conexao);
                cmd.Parameters.AddWithValue("Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    cliente.Id = (Int32)(dr["Id_usu"]);
                    cliente.Nome = (string)(dr["Nome_usu"]);
                    cliente.Nascimento = (DateTime)(dr["DataNasc"]);
                    cliente.CPF = (string)(dr["CPF_usu"]);
                    cliente.Telefone = (string)(dr["tel_usu"]);
                    cliente.Login = (string)(dr["Login_usu"]);
                    cliente.Senha = (string)(dr["Senha_usu"]);
                }
                return cliente;
            }
        }

        public IEnumerable<Cliente> ObterTodosClientes()
        {
            List<Cliente> cliList = new List<Cliente>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from usuario", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    cliList.Add(
                        new Cliente
                        {
                            Id = Convert.ToInt32(dr["Id_usu"]),
                            Nome = Convert.ToString(dr["Nome_usu"]),
                            Nascimento = Convert.ToDateTime(dr["DataNasc"]),
                            CPF = Convert.ToString(dr["CPF_usu"]),
                            Telefone = Convert.ToString(dr["tel_usu"]),
                            Login = Convert.ToString(dr["Login_usu"]),
                            Senha = Convert.ToString(dr["Senha_usu"])
                        });
                }
                return cliList;
            }
        }
    }
}