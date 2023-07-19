using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BaseWeb.Models;
using System.Collections.Generic;


/********************************
 * used in Login/Index
*******************************/
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

        public string errMsg { get;set; } = string.Empty;

    }

}
