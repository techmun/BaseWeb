using BaseWeb.Models;

namespace BaseWeb.ViewModels
{
    public class JobProcessViewModel
    {
        public List<Job> JobList{get;set;}
        public List<DocProcess> docList { get; set; }
    }

    public class Job
    {
        public string status { get; set; } = "";
        public JobProcess jobProcess { get; set; }
    }
}
