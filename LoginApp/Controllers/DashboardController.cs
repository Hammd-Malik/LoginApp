using Microsoft.AspNetCore.Mvc;

namespace LoginApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _context;

        public DashboardController(IHttpContextAccessor context)
        {
            _context = context;
        }
        public ActionResult UserDashboard()
        {
            if(_context.HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("UserRegister", "UserAuth");
            }
            return View();
        }
    }
}
