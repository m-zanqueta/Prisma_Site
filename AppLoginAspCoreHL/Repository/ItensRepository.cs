using AppLoginAspCoreHL.Models;
using AppLoginAspCoreHL.Repository.Contract;
using MySql.Data.MySqlClient;
using System.Data;

namespace AppLoginAspCoreHL.Repository
{
    public class ItensRepository : IItemRepository
    {
        private readonly string _conexaoMySQl;
        public ItensRepository(IConfiguration conf)
        {
            _conexaoMySQl = conf.GetConnectionString("ConexaoMySQL");
        }
        public void Atualizar(ItensPedido itensPedido)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(ItensPedido itensPedido)
        {

            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {

                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into itens_pedido" +
                    " values(default, @Id_ped, @Id_Liv, @QtItens, @VlTotal)", conexao);

                cmd.Parameters.Add("@Id_ped", MySqlDbType.VarChar).Value = itensPedido.Id_pedido;
                cmd.Parameters.Add("@Id_liv", MySqlDbType.VarChar).Value = itensPedido.Id_liv;
                cmd.Parameters.Add("@QtItens", MySqlDbType.VarChar).Value = itensPedido.QtItens;
                cmd.Parameters.Add("@VlTotal", MySqlDbType.VarChar).Value = itensPedido.VlTotal.ToString().Replace(".", "").Replace(",", "."); 
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            throw new NotImplementedException();
        }

        public ItensPedido ObterItens(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ItensPedido> ObterTodosItensPedido(int idPed, int idUsu)
        {
            List<ItensPedido> ItensList = new List<ItensPedido>();
            using (var conexao = new MySqlConnection(_conexaoMySQl))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select Titulo_liv, QtItens, ROUND(VlTotal, 1) AS VlTotal, t1.Id_ped, t3.Image_liv " + 
                " from itens_pedido as t1 inner join pedido as t2 on t1.Id_ped = t2.Id_ped inner join livros as t3 on t1.Id_liv = t3.Id_liv " + 
                " inner join usuario as t4 on t2.Id_usu = t4.Id_usu where t2.Id_usu = @Id_usu and t2.Id_ped = @Id_ped;", conexao);
                cmd.Parameters.Add("@Id_ped", MySqlDbType.VarChar).Value = idPed;
                cmd.Parameters.Add("@Id_usu", MySqlDbType.VarChar).Value = idUsu;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    ItensList.Add(
                        new ItensPedido
                        {
                            Titulo_liv = (string)(dr["Titulo_liv"]),
                            QtItens = (Int32)(dr["QtItens"]),
                            VlTotal = (double)(dr["VlTotal"]),
                            Id_pedido = (Int32)(dr["Id_ped"]),
                            Imagem = (string)(dr["Image_liv"])
                        });
                }
                return ItensList;
            }
        }
    }
}
