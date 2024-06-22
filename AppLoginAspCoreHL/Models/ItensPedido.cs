using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class ItensPedido
    {
        public int Id_item { get; set; }
        public int Id_liv {  get; set; }
        [Display(Name = "Livro")]
        public string? Titulo_liv { get; set; }
        [Display(Name = "Número do Pedido")]
        public int Id_pedido {  get; set; }
        [Display(Name = "Quantidade")]
        public int QtItens { get; set; }
        [Display(Name = "Valor")]
        public double VlTotal { get; set; }
        public string Imagem { get; set; }
    }
}
