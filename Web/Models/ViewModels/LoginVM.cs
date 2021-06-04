using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels
{
   public class LoginVM
    {
        [Required, MaxLength(30)]
        public string Usuario { get; set; }

        [Required, DataType(DataType.Password), MinLength(4), MaxLength(20)]
        public string Senha { get; set; }

        public string ReturnUrl {get;set;}

    }
}