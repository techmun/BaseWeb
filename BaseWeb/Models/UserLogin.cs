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

        [DefaultValue(false)]
        public Boolean isAdmin { get; set; }

        public bool isActived { get; set; }

        [MaxLength(25)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get;set; } = DateTime.Now;

        [MaxLength(25)]
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; } = DateTime.Now;

    }
}
