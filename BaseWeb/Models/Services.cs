using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseWeb.Models
{
    public class Services
    {
        [Key]
        [MaxLength(15)]
        public string SerID { get; set; } = "";
        [MaxLength(100)]
        public string GovDept { get; set; } = "";
        [MaxLength(100)]
        public string SerName { get; set; } = "";
        [MaxLength(100)]
        public string GovURL { get; set; } = "";

        [MaxLength(5)]
        public string ExpiryPeriod { get; set; } = "";
        [MaxLength(25)]
        public string CreatedBy { get; set; } = "";

        public DateTime? CreatedDate { get; set; }

        [MaxLength(25)]
        public string EditedBy { get; set; } = "";
        public DateTime? EditedDate { get; set; }
    }
}
