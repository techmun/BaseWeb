using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace BaseWeb.Models
{
    public class UserLogin
    {
        [Key]
        [MaxLength(25)]
        public string LoginId { get; set; } 
        public string Password { get; set; }

        [MaxLength(100)]
        public string UserName { get; set; }

        [MaxLength(25)]
        public string Role { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
        public int Phone { get; set; }
        public bool isActived { get; set; }
        [MaxLength(25)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get;set; }
        [MaxLength(25)]
        public string EditedBy { get; set; }    
        public DateTime EditedDate { get; set;}

    }
}
