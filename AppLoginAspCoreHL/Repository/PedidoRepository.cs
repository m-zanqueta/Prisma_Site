using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Models.Constant;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;

namespace AppLoginAspCoreHL.Contract
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly string _conexaoMySQl;

        public PedidoRepository(IConfiguration conf)
        {
            _conexaoMySQl = conf.GetConnectionString("ConexaoMySQL");
        }

        public void Atualizar(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        public void BuscarPedidoPorId(Pedido pedido)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {
                conexao.Open();
                MySqlDataReader dr;

                MySqlCommand cmd = new MySqlCommand("select Id_ped from Pedido order by Id_ped desc limit 1", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    pedido.Id_pedido = Convert.ToInt32(dr[0].ToString());
                }
                conexao.Close();
            }
        }

        public void Cadastrar(Pedido pedido)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into pedido" +
                    " values(default, @Id_usu, CURRENT_TIMESTAMP(), 0, @Situacao_ped)", conexao);

                cmd.Parameters.Add("@Id_usu", MySqlDbType.VarChar).Value = pedido.Id_usu;
                cmd.Parameters.Add("@Situacao_ped", MySqlDbType.VarChar).Value = pedido.Situacao;
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }


        public void InputValor(double valor, int id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {
                conexao.Open();
                MySqlCommand cmd2 = new MySqlCommand("update pedido set Valor=@Valor where id_ped=@Id_ped;", conexao);
                cmd2.Parameters.Add("@Valor", MySqlDbType.VarChar).Value = valor.ToString().Replace(".", "").Replace(",", "."); ;
                cmd2.Parameters.Add("@Id_ped", MySqlDbType.VarChar).Value = id;
                cmd2.ExecuteNonQuery();
                conexao.Close();

            }
        }

        public Pedido ObterPedido(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Id_ped, Id_usu, Horario_ped, ROUND(Valor, 2) as Valor  from pedido where Id_ped = @Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Pedido pedido = new Pedido();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    pedido.Id_pedido = (Int32)(dr["Id_ped"]);
                    pedido.Id_usu = (Int32)(dr["Id_usu"]);
                    pedido.Horario_ped = Convert.ToDateTime(dr["Horario_ped"]);
                    pedido.Valor = (double)(dr["Valor"]);
                }
                return pedido;
            }
        }
            public IEnumerable<Pedido> ObterTodosPedidos()
            {
                List<Pedido> PedidoList = new List<Pedido>();
                using (var conexao = new MySqlConnection(_conexaoMySQl))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("select Id_ped, t1.Id_usu, t2.Nome_usu, Horario_ped, ROUND(Valor, 2) as Valor, Situacao_ped " + 
                    " from pedido as t1 inner join usuario as t2 on t1.Id_usu = t2.Id_usu;", conexao);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    conexao.Close();

                    foreach (DataRow dr in dt.Rows)
                    {
                        PedidoList.Add(
                            new Pedido
                            {
                                Id_pedido = (Int32)(dr["Id_ped"]),
                                Id_usu = (Int32)(dr["Id_usu"]),
                                Horario_ped = Convert.ToDateTime(dr["Horario_ped"]),
                                Cliente = (string)(dr["Nome_usu"]),
                                Valor = (double)(dr["Valor"]),
                                Situacao = (string)(dr["Situacao_ped"]),
                            });
                    }
                    return PedidoList;
                }
            }

            public void Finalizar(int Id)
            {
                string situacao = PedidoTipoConstant.Entregue;
                using (var conexao = new MySqlConnection(_conexaoMySQl))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("update pedido set Situacao_ped = @Situacao_ped where Id_ped=@Id", conexao);
                    cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                    cmd.Parameters.Add("@Situacao_ped", MySqlDbType.VarChar).Value = situacao;
                    cmd.ExecuteNonQuery();
                    conexao.Close();
                }
            }
            public void Reabrir(int Id)
            {
                string situacao = PedidoTipoConstant.Andamento;
                using (var conexao = new MySqlConnection(_conexaoMySQl))
                {
                    conexao.Open();
                    MySqlCommand cmd = new MySqlCommand("update pedido set Situacao_ped = @Situacao_ped where Id_ped=@Id", conexao);
                    cmd.Parameters.Add("@Id", MySqlDbType.VarChar).Value = Id;
                    cmd.Parameters.Add("@Situacao_ped", MySqlDbType.VarChar).Value = situacao;
                    cmd.ExecuteNonQuery();
                    conexao.Close();
                }
            }
        }
    }
