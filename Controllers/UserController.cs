using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bankaccount.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace bankaccount.Controllers
{
    public class UserController : Controller
    {
        private BankContext _context;
 
        public UserController(BankContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(UserViewModel item)
        {
            // As soon as the model is submitted TryValidateModel() is run for us, ModelState is already set
            if(ModelState.IsValid)
            {
                // Handle Success Case
                User newUser = new User {
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    Password = item.Password,
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                // _context.Add(NewPerson);
                // OR _context.Users.Add(NewPerson);
                _context.Users.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("ShowAccount", "Bank", new {Id = newUser.UserId});
            }
            return View("Index", item);
        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("VerifyLogin")]
        public IActionResult VerifyLogin(string Email, string Password)
        {
            string PasswordToCheck = Password;
            // Attempt to retrieve a user from your database based on the Email submitted
            var user = _context.Users.SingleOrDefault(r => r.Email == Email);
            if(user != null && PasswordToCheck != null)
            {
                var Hasher = new PasswordHasher<User>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(user, user.Password, PasswordToCheck))
                {
                    //Handle success
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToAction("ShowAccount", "Bank", new {Id = user.UserId});
                }
            }

            //Handle failure
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
