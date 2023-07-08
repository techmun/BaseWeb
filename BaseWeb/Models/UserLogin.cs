using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace BaseWeb.Models
{
    public class UserLogin
    {
        [Key]
        public string LoginId { get; set; } 
        public string Password { get; set; } 
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get;set; }
        public string EditedBy { get; set; }    
        public string EditedDate { get; set;}

    }
}
