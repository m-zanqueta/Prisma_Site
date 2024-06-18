using AppLoginAspCoreHL.Models;
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
                    " values(default, @Id_usu, @Horario_ped, 0)", conexao);

                cmd.Parameters.Add("@Id_usu", MySqlDbType.VarChar).Value = pedido.Id_usu;
                cmd.Parameters.Add("Horario_ped", MySqlDbType.VarChar).Value = pedido.Horario_ped.ToString("yyyy/MM/dd");
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }


        public void InputValor(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {
                double valor = 0;
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select VlTotal from itens_pedido where Id_ped = @Id_ped;", conexao);

                cmd.Parameters.Add("@Id_ped", MySqlDbType.VarChar).Value = Id;
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    valor += (double)(dr["VlTotal"]);
                }

                conexao.Open();
                MySqlCommand cmd2 = new MySqlCommand("update pedido set Valor=@Valor where id_ped=@id_ped;", conexao);
                cmd.Parameters.Add("@Valor", MySqlDbType.VarChar).Value = valor;

            }
        }

        public Pedido ObterPedido(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Pedido> ObterTodosPedidos()
        {
            throw new NotImplementedException();
        }
    }
}
