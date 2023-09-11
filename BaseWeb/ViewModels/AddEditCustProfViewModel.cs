using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BaseWeb.ViewModels
{
    public class AddEditCustProfViewModel
    {
        [DisplayName("Customer Code")]
        [Required(ErrorMessage = "Please enter customer code")]
        [MaxLength(10)]
        public string CustCode { get; set; }

        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Please enter custermer name")]
        [MaxLength(100)]
        public string CustName { get; set; }

        [DisplayName("Contact Person")]
        [Required(ErrorMessage = "Please enter contact person")]
        [MaxLength(100)]
        [RegularExpression("^[a-zA-Z @]*$", ErrorMessage = "Only accept alphabet, '@' and space")]
        public string ContactPS { get; set; }

        [DisplayName("Contact No.")]
        [Required(ErrorMessage = "Please enter contact no.")]
        [MaxLength(15)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only accept numeric (0-9)")]
        public string ContactNo { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Please enter email address")]
        [MaxLength(100)]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "invalid email address")]
        public string Email { get; set; }

        [DisplayName("License No.")]
        [Required(ErrorMessage = "Please enter license no.")]
        [MaxLength(100)]
        public string LicenseNo { get; set; }
        [DisplayName("Address 1.")]
        [Required(ErrorMessage = "Please enter Address 1.")]
        [MaxLength(100)]
        public string Add1 { get; set; }

        [DisplayName("Address 2")]
        [Required(ErrorMessage = "Please enter Address 2.")]
        [MaxLength(100)]
        public string Add2 { get; set; }

        [DisplayName("Address 3")]
        [MaxLength(100)]
        public string? Add3 { get; set; }

        [DisplayName("Address 4")]
        [MaxLength(100)]
        public string? Add4 { get; set; }

        [DisplayName("Postcode")]
        [Required(ErrorMessage = "Please enter Postcode.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only accept numeric (0-9)")]
        [MaxLength(5)]
        public int PostCode { get; set; }
    }
}
