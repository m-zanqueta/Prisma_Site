using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class CategoriaLiv
    {
        [Display(Name = "Categoria")]
        public int id { get; set; }

        [Display(Name = "Nome")]
        public string? nome { get; set; }
    }
}
