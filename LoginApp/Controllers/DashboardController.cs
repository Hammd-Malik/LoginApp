using LoginApp.Interfaces;
using LoginApp.Models;
using Microsoft.AspNetCore.Mvc;

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
            /*if(_context.HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("UserRegister", "UserAuth");
            }*/
            List<DevModel> devList = _db.GetDevList();
            return View(devList);

        }
    }
}
