using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;

namespace AppLoginAspCoreHL.Repository
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly string _conexaoMySQL;
        // Metodo construtor da classe ClienteRepository
        public ColaboradorRepository(IConfiguration conf)
        {
            // Injeção de dependencia do banco de dados
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Colaborador colaborador)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update administrador set Nome_adm=@Nome, Email_adm=@Email, Senha_adm=@Senha, Tipo_adm=@Tipo where Id_adm=@Id", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = colaborador.Id;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.Nome;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = colaborador.Tipo;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Colaborador colaborador)
        {
            string tipo = ColaboradorTipoConstant.Comum;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("insert into administrador values(default, @Nome, @Email, @Senha, @Tipo)", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = colaborador.Nome;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = colaborador.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = colaborador.Senha;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = tipo;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("delete from administrador where Id_adm=@Id ", conexao);
                cmd.Parameters.AddWithValue("Id", Id);
                int i = cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public Colaborador Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from administrador where Email_adm = @Email and Senha_adm = @Senha", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = Senha;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Colaborador colaborador = new Colaborador();
                dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    colaborador.Id = (Int32)(dr["Id_adm"]);
                    colaborador.Nome = (string)(dr["Nome_adm"]);
                    colaborador.Tipo = (string)(dr["Tipo_adm"]);
                    colaborador.Email = (string)(dr["Email_adm"]);
                    colaborador.Senha = (string)(dr["Senha_adm"]);
                }
                return colaborador;
            }
        }

        public Colaborador ObterColaborador(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from administrador where Id_adm=@Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Colaborador colaborador = new Colaborador();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read()) 
                {
                    colaborador.Id = (Int32)(dr["Id_adm"]);
                    colaborador.Nome = (string)(dr["Nome_adm"]);
                    colaborador.Email = (string)(dr["Email_adm"]);
                    colaborador.Senha = (string)(dr["Senha_adm"]);
                    colaborador.Tipo = (string)(dr["Tipo_adm"]);
                }
                return colaborador;
            }
        }

        public IEnumerable<Colaborador> ObterTodosColaboradores()
        {
            List<Colaborador> colabList = new List<Colaborador>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from administrador", conexao);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    colabList.Add(
                        new Colaborador
                        {
                            Id = Convert.ToInt32(dr["Id_adm"]),
                            Nome = (string)(dr["Nome_adm"]),
                            Email = (string)(dr["Email_adm"]),
                            Senha = (string)(dr["Senha_adm"]),
                            Tipo = (string)(dr["Tipo_adm"])
                        });
                }
                return colabList;
            }
        }

        public void Promover(int Id)
        {
            string tipo = ColaboradorTipoConstant.Gerente;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update administrador set Tipo_adm = @Tipo where Id_adm=@Id ", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = tipo;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
        public void Rebaixar(int Id)
        {
            string tipo = ColaboradorTipoConstant.Comum;
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("update administrador set Tipo_adm = @Tipo where Id_adm=@Id ", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@Tipo", MySqlDbType.VarChar).Value = tipo;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }
    }
}
