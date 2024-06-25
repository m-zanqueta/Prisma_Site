using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class Cliente
    {
        [Display(Name = "Código", Description = "Código.")]
        public int Id { get; set; }


        [Display(Name = "Nome completo", Description = "Nome e Sobrenome")]
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string Nome { get; set; }


        [Display(Name = "Nascimento")]
        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        public DateTime Nascimento { get; set;}


        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        public string CPF { get; set;}


        [Display(Name = "Celular")]
        [Required(ErrorMessage = "O celular é obrigatório")]
        [StringLength(15, MinimumLength = 14, ErrorMessage = "Insira um telefone válido")]
        public string Telefone { get; set;}


        [Display(Name = "Email de login")]
        [EmailAddress(ErrorMessage = "O email não é válido")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        public string Login { get; set;}


        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O senha é obrigatório")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 20 caracteres")]
        public string Senha { get; set;}

        public Endereco? Endereco { get; set;}
    }
}
