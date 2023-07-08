using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseWeb.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Login ID")]
        [Required(ErrorMessage = "Please enter your LoginId")]
        public string LoginId { get; set; } = string.Empty;

        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; } = string.Empty;
    }
}
