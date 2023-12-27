using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BaseWeb.Models
{
    public class CustProf
    {
        [Key]
        [MaxLength(10)]
        public string CustCode { get; set; } 

        [MaxLength(100)]
        public string CustName { get; set; }

        [MaxLength(100)]
        public string ContactPS { get; set; }

        [MaxLength(15)]
        public string ContactNo { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Add1 { get; set; } = "";

        [MaxLength(100)]
        public string Add2 { get; set; } = "";

        [MaxLength(100)]
        public string Add3 { get; set; } = "";

        [MaxLength(100)]
        public string Add4 { get; set; } = "";

        [MaxLength(5)]
        public int PostCode { get; set; }

        [MaxLength(100)]
        public string LicenseNo{ get; set; } = "";

        [MaxLength(255)]
        public string Remark { get; set; } = "";

        [MaxLength(25)]
        public string CreatedBy { get; set; } 

        public DateTime? CreatedDate { get; set; }

        [MaxLength(25)]
        public string EditedBy { get; set; } 
        public DateTime? EditedDate { get; set; }
    }
}
