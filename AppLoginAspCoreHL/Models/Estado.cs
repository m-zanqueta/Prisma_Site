using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class Estado
    {
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O estado é obrigatório")]
        public string uf {  get; set; }

        public string? nome { get; set; }
    }
}
