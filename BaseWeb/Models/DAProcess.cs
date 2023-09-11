using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseWeb.Models
{
    public class DAProcess
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = new Guid();
        public Guid CID { get; set; }
        public Guid SerID { get; set; }
        [MaxLength(100)]
        public string GovDept { get; set; }
        [MaxLength(100)]
        public string DeptCat { get; set; }
        [MaxLength(100)]
        public string Services { get; set; }

        [MaxLength(25)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(25)]
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; } = DateTime.Now;

    }
}
