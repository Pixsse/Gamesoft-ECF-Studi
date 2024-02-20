using System.ComponentModel.DataAnnotations;

namespace Gamesoft.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required, EmailAddress, Display(Name = "Registered email address")]
        
        public string Email { get; set; }
    }
}
