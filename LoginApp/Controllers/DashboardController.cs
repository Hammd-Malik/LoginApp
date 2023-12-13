using LoginApp.Interfaces;
using LoginApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Security.Policy;

namespace LoginApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _context;
        private readonly IApplicationDatabase _db;

        public DashboardController(IHttpContextAccessor context, IApplicationDatabase db)
        {
            _db = db;
            _context = context;
        }
        
        public ActionResult UserDashboard()
        {
            if (_context.HttpContext.Session.GetString("UserEmail") != null)
            {
                
                var TaskList = _db.GetTaskList();
                var DevList = _db.GetDevList();
                List<dynamic> TaskData = new List<dynamic>();

                foreach (TaskModel Task in TaskList)
                {
                    dynamic data = new ExpandoObject();
                    var dev = _db.GetDevById(Task.AssignedTo);
                    var user = _db.GetUserById(Task.AssignedBy);

                    data.id = Task.Id;
                    data.devId = dev?[0].Id;
                    data.devName = dev?[0].Name;
                    data.userName = user?[0].Name;
                    data.taskTitle = Task.TaskTitle;
                    data.taskDetails = Task.TaskDetails;
                    data.createdAt = Task.AssignedDate;
                    data.status = Task.Status;
                    TaskData.Add(data);
                }

                var viewModel = new ViewModel
                {
                    TaskList = TaskData,
                    DevList = DevList,
                    TaskModel = new TaskModel()

                };

                return View(viewModel);
                
            }
            return RedirectToAction("UserRegister", "UserAuth");
        }

        [ValidateAntiForgeryToken]
        public ActionResult UserAddTask(ViewModel model)
        {
            if (model.TaskModel != null && model.TaskModel.TaskTitle != null && model.TaskModel.TaskDetails != null && model.TaskModel.AssignedTo != 0)
            {
                var loginedUser = _db.CheckEmail(_context.HttpContext.Session.GetString("UserEmail"));
                model.TaskModel.AssignedBy = loginedUser[0].Id;
                _db.AddTask(model.TaskModel);
                TempData["SuccessMessage"] = "Task Added!!";
                return RedirectToAction("UserDashboard", "Dashboard");
            }
            TempData["ErrorMessage"] = "Some Error Occurred, Please try Again!!";
            return RedirectToAction("UserDashboard", "Dashboard");
        }

        public ActionResult DeleteTask(int id)
        {
            _db.DeleteTask(id);
            TempData["SuccessMessage"] = "Task Delete!!";
            return RedirectToAction("UserDashboard", "Dashboard");
        }

        public ActionResult DetailsTask(int id)
        {
            if (_context.HttpContext.Session.GetString("UserEmail") != null)
            {
                var TaskDetails = _db.TaskDetailsById(id);
                List<dynamic> TaskData = new List<dynamic>();

                foreach (TaskModel Task in TaskDetails)
                {
                    dynamic data = new ExpandoObject();
                    var dev = _db.GetDevById(Task.AssignedTo);
                    var user = _db.GetUserById(Task.AssignedBy);

                    data.id = Task.Id;
                    data.devName = dev?[0].Name;
                    data.userName = user?[0].Name;
                    data.taskTitle = Task.TaskTitle;
                    data.taskDetails = Task.TaskDetails;
                    data.createdAt = Task.AssignedDate;

                    TaskData.Add(data);
                }

                var viewModel = new ViewModel
                {
                    TaskList = TaskData,
                };
                return View(viewModel);
            }
            return RedirectToAction("UserRegister", "UserAuth");
               
        }

        public ActionResult TaskStatusUpdate(int id)
        {
            if (_context.HttpContext.Session.GetString("UserEmail") != null)
            {
                
                _db.TaskStatusUpdate(id);
                TempData["SuccessMessage"]  = "Task is Marked as Done";
                return RedirectToAction("UserDashboard", "Dashboard");
            }
            return RedirectToAction("UserRegister", "UserAuth");
        }


    }
}
