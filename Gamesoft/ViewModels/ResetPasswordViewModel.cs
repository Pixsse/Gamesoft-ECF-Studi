using System.ComponentModel.DataAnnotations;

namespace Gamesoft.ViewModels
{

    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{10,}$", ErrorMessage = "Le mot de passe doit contenir au moins une lettre majuscule, une lettre minuscule, un chiffre et un caractère spécial.")]
        public string NewPassword { get; set; }
    }
}
