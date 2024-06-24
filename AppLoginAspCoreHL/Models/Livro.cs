using System.ComponentModel.DataAnnotations;

namespace AppLoginAspCoreHL.Models
{
    public class Livro
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Imagem")]
        public string? Imagem { get; set; }

        [Required(ErrorMessage = "A quantidade em estoque é obrigatória.")]
        [Display(Name = "Em estoque")]
        [Range(1, 9999, ErrorMessage = "Insira uma quantidade válida")]
        public int QuantidadeEstq { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O número de páginas é obrigatória.")]
        [Display(Name = "Número de páginas")]
        [Range(1, 9999, ErrorMessage = "Insira um número de páginas válido")]
        public int NumeroPags { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        [Display(Name = "Título")]
        public string Titulo { get; set;}

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Display(Name = "Preço")]
        [Range(1, 99999, ErrorMessage = "Insira um preço válido")]
        public double Preco { get; set; }

        [Required(ErrorMessage = "O autor é obrigatório.")]
        [Display(Name = "Autor")]
        public string Autor { get; set;}

        [Display(Name = "Situação")]
        public string? Situacao { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Display(Name = "Categoria")]
        public CategoriaLiv categoria { get; set; }
    }
}
