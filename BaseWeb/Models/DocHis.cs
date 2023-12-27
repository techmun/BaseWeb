using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseWeb.Models
{
    public class DocHis
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid JobId { get; set; }
        [MaxLength(15)]
        public string SerID { get; set; } = "";
        public string Document { get; set; } = "";

        [MaxLength(3)]
        public string PreparedBy { get; set; } = "";

        [MaxLength(1)]
        public string Mandatory { get; set; } = "";

        [MaxLength(1)]
        public string UploadDoc { get; set; } = "";

        public int DocCount { get; set; } = 1;

        [MaxLength(1)]
        public string Step { get; set; } = "";
        [MaxLength(25)]
        public string UploadedBy { get; set; } = "";

        public DateTime? UploadedDate { get; set; }

        [MaxLength(25)]
        public string JobEndBy { get; set; }
        public DateTime? JobEndDate { get; set; }

    }
}
