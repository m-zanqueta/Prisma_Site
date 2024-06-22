using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class Pedido
    {
        [Display(Name = "Número do Pedido")]
        public int Id_pedido { get; set; }
        public int Id_usu { get; set; }
        [Display(Name = "Cliente")]
        public string? Cliente { get; set; }
        [Display(Name = "Horário do Pedido")]
        public DateTime Horario_ped {  get; set; }
        [Display(Name = "Valor do Pedido")]
        public double Valor { get; set; }
        [Display(Name = "Situação do Pedido")]
        public string? Situacao { get; set; }
    }
}
