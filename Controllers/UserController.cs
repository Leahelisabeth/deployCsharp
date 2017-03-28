using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dojoPrep.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace dojoPrep.Controllers
{
    public class UserController : Controller
    {
        private dojoPrepContext _context;

        public UserController(dojoPrepContext context)
        {
            _context = context;
        }
        // GET: /Home///////////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.NewError = "";
            ViewBag.NextError = "";
            ViewBag.errors = new List<object>();
            return View();
        }
        // [HttpGet]
        // [Route("Register")]
        // public IActionResult GetResgister()
        // {
        //     ViewBag.NewError = "";
        //     ViewBag.errors = new List<object>();
        //     return View("Register");
        // }
// ///////////////////////////////////////////////Register////////////////////////////////////////////////////////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //check if a user with this email already exists
                User new_user = _context.User.SingleOrDefault(user1 => user1.Email == model.Email);
                if (new_user == null)
                {
                    //if they dont exist make a new one
                    
                    User NewUser = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        Password = model.Password
                    };
                    _context.Add(NewUser);
                    _context.SaveChanges();
                    //find the user we just registered in database and put them in session
                    User curuser = _context.User.SingleOrDefault(user2 => user2.Email == NewUser.Email);
                    HttpContext.Session.SetInt32("users_id", curuser.UserId);
                    HttpContext.Session.SetString("UserName", curuser.FirstName);

                    return RedirectToAction("dashboard", "Dash");
                }
                //the user wasnt null
                ViewBag.NextError = "A User with this email already exists, please login above";
                ViewBag.errors = new List<string>();
                ViewBag.NewError = "";
                return View("Index");
            }
            //the modelstate wasnt valid
            ViewBag.NewError = "";
            ViewBag.NextError = "";
            ViewBag.errors = new List<string>();
            return View("Index");
        }
// ///////////////////////////////////////////Login //////////////////////////////////////////////////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Login")]
        public IActionResult Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                if (Email == null || Password == null)
                {
                    ViewBag.NewError = "Please type in both an Email and Password";
                    ViewBag.errors = new List<object>();
                    ViewBag.NextError = "";
                    return View("Index");
                }
                User cur_user = _context.User.SingleOrDefault(user1 => user1.Email.ToString() == Email);
                if (cur_user != null)
                {
                    if ((string)cur_user.Password == Password)
                    {
                        HttpContext.Session.SetInt32("users_id", cur_user.UserId);
                        HttpContext.Session.SetString("UserName", cur_user.FirstName);
                        return RedirectToAction("dashboard", "Dash");
                    }
                    ViewBag.NewError = "Password is incorrect!";
                    ViewBag.errors = new List<String>();
                    ViewBag.NextError = "";
                    return View("Index");
                }

                ViewBag.errors = new List<object>(); 
                ViewBag.NextError = "";
                ViewBag.NewError = "We could not find your email please register below";
                return View("Index");
            }
            ViewBag.errors = ModelState.Values;
            ViewBag.NewError = "";
            ViewBag.NextError = "";
            return View("Index");
        }
// /////////////////////////////////////////////////Logout ////////////////////////////////////////////////////////////
        [HttpPostAttribute]
        [RouteAttribute("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }
    }
}
