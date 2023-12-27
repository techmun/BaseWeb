using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseWeb.ViewModels
{
    public class ExpiryDateViewModel
    {
        [MaxLength(10)]
        public string CustCode { get; set; }

        [DisplayName("Company Name")]
        [MaxLength(100)]
        public string? CustName { get; set; }

        [DisplayName("Expiry Date")]
        [Required]
        public DateTime? ExpiredDate { get; set; } = null;

        [DisplayName("Type")]
        [MaxLength(5)]
        public string DocType { get; set; } = "";

        [DisplayName("Description")]
        public string Description { get; set; } = "";

    }
}
