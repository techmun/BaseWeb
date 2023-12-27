using BaseWeb.Models;

namespace BaseWeb.ViewModels
{
    public class ProcListViewModel
    {
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public List<Services> services { get; set; }
    }
}
