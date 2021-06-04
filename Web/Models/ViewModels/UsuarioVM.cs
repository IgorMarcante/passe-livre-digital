using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels
{
    public class UsuarioVM
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "A senha deve possuir no minimo 6 dígitos")]
        public string Senha { get; set; }
        [Required]
        [DataType(DataType.Password)]

        [Compare("Senha")]
        
        public string ConfirmSenha { get; set; }
    }
}