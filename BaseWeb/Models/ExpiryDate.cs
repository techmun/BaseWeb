using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BaseWeb.Models
{
    public class ExpiryDate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(10)]
        public string CustCode { get; set; }

        public DateTime ExpiredDate { get; set; }

        [MaxLength(20)]
        public string DocType { get; set; } = "";

        public string Description { get; set; } = "";

        [MaxLength(25)]
        public string CreatedBy { get; set; } = "";

        public DateTime? CreatedDate { get; set; }

        [MaxLength(25)]
        public string EditedBy { get; set; } = "";
        public DateTime? EditedDate { get; set; }

        public Boolean active { get;set; } = true;
    }
}
