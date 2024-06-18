using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class Endereco
    {
        public int id { get; set; }
        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O CEP é obrigatório CEP")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Insira um CEP válido")]
        public string cep { get; set; }
        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O bairro é obrigatório")]
        public string bairro { get; set; }
        [Display(Name = "Rua")]
        [Required(ErrorMessage = "a rua é obrigatória")]
        public string rua { get; set; }
        [Display(Name = "Número")]
        [Required(ErrorMessage = "O número é obrigatório")]
        [Range(1,9999, ErrorMessage = "Insira um número residencial válido")]
        public Int32 logradouro { get; set; }
        [Display(Name = "Complemento")]
        public string? complemento { get; set; }

        public Estado estado { get; set; }
    }
}
