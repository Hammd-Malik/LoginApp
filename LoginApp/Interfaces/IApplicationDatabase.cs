using LoginApp.Models;
using System.ComponentModel.DataAnnotations;

namespace LoginApp.Interfaces
{
    public interface IApplicationDatabase
    {

        public List<UserModel> CheckEmail(String Email);

        public void RegisterUser(UserModel User);

        public void AddTask(TaskModel Task);

        public List<DevModel> GetDevList();

        public List<TaskModel> GetTaskList();

        public List<UserModel> GetUserById(int id);

        public List<DevModel> GetDevById(int id);

        public List<TaskModel> GetTaskById(int id);

        public void DeleteTask(int id);

        public List<TaskModel> TaskDetailsById(int id);

        public void TaskStatusUpdate(int id);
    }
}
