using LoginApp.Models;

namespace LoginApp.Models
{
    public class ViewModel
    {
        public List<dynamic>? TaskList { get; set; }

        public List<DevModel>? DevList { get; set; }

        public TaskModel TaskModel { get; set; }

    }
}