using LoginApp.Interfaces;
using LoginApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace LoginApp.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly IApplicationDatabase _db;
        private readonly IHttpContextAccessor _context;

        public UserAuthController(IApplicationDatabase db, IHttpContextAccessor context)
        {
            _db = db;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserRegister()
        {
            string userEmail = HttpContext.Request.Cookies["UserInfo"];
            if (!string.IsNullOrEmpty(userEmail))
            {
                var dbUser = _db.CheckEmail(userEmail);
                _context.HttpContext.Session.SetString("UserEmail", dbUser.FirstOrDefault().Email);
                _context.HttpContext.Session.SetString("Username", dbUser.FirstOrDefault().Name);
                return RedirectToAction("UserDashboard", "Dashboard");

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRegister(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var dbUser = _db.CheckEmail(model.Email);
                if (!dbUser.Any())
                {
                    model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    _db.RegisterUser(model);
                    ViewBag.SuccessMessage = "Account Created Successfully.";

                    return View("UserRegister", model);
                }
                else
                {
                    ViewBag.ErrorMessage = "Email Already Exist.";
                }

            }
            return View("UserRegister", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(UserModel model)
        {

            if (model.Email != "" || model.Password != "")
            {
                var dbUser = _db.CheckEmail(model.Email);
                if (dbUser != null && dbUser.Any())
                {
                    var checkPassword = BCrypt.Net.BCrypt.Verify(model.Password, dbUser[0].Password);
                    if (checkPassword)
                    {
                        _context.HttpContext.Session.SetString("UserEmail", model.Email);
                        _context.HttpContext.Session.SetString("Username", dbUser[0]?.Name);
                        if (model.RememberMe)
                        {
                            CookieOptions options = new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(1)
                            };
                            Response.Cookies.Append("UserInfo",model.Email, options);
                        }
                        
                        return RedirectToAction("UserDashboard", "Dashboard");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Incorrect Password";
                        return View("UserRegister", model);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Incorrect Email";
                    return View("UserRegister", model);
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Email and Password cannot be Null";
                return View("UserRegister", model);
            }
            
        }

        public ActionResult UserLogout()
        {
            Response.Cookies.Delete("UserInfo");
            _context.HttpContext.Session.Remove("UserEmail");
            ViewBag.SuccessMessage = "You have been logout Successfully";
            return RedirectToAction("UserRegister", "UserAuth");
        }
    }
}
