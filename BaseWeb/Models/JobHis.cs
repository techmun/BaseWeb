using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseWeb.Models
{
    public class JobHis
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = new Guid();

        [MaxLength(10)]
        public string CustCode { get; set; }
        [MaxLength(15)]
        public string SerID { get; set; } = "";

        [MaxLength(100)]
        public string GovDept { get; set; }

        [MaxLength(100)]
        public string Services { get; set; }
        [MaxLength(1000)]
        public string GovURL { get; set; }

        [MaxLength(5)]
        public string ExpiryPeriod { get; set; } = "";

        [MaxLength(25)]
        public string JobEndBy { get; set; }
        public DateTime? JobEndDate { get; set; }
        [MaxLength(25)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [MaxLength(25)]
        public string EditedBy { get; set; }
        public DateTime? EditedDate { get; set; }

    }
}
