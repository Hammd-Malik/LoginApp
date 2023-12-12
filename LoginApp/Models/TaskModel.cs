using Oracle.ManagedDataAccess.Types;

namespace LoginApp.Models
{
    public class TaskModel
    {
        public int Id { get; set; }

        public string? TaskTitle { get; set; }

        public int AssignedTo { get; set; }

        public int AssignedBy { get; set; }

        public string? TaskDetails { get; set; }

        public DateTime AssignedDate { get; set; }
    }
}
