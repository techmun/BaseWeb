using System.ComponentModel.DataAnnotations;

namespace BaseWeb.Models
{
    public class WebSetup
    {
        [Key]
        public string name { get; set; }
        public string value { get; set; }
    }
}
