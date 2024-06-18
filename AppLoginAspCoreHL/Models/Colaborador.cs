using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class Colaborador
    {
        [Display(Name = "Código", Description = "Código")]
        public int Id { get; set; }


        [Display(Name = "Nome completo", Description = "Nome e Sobrenome")]
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Email para login")]
        [EmailAddress(ErrorMessage = "O email não é válido")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        public string Email { get; set; }


        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O senha é obrigatório")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 a 10 caracteres")]
        public string Senha { get; set; }


        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O tipo é obrigatório")]
        public string Tipo { get; set; }
    }
}
