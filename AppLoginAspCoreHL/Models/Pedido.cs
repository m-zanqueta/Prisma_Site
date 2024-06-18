namespace AppLoginAspCoreHL.Models
{
    public class Pedido
    {
        public int Id_pedido { get; set; }
        public int Id_usu { get; set; }
        public DateTime Horario_ped {  get; set; }
        public double Valor { get; set; }
    }
}
