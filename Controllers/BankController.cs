using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bankaccount.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace bankaccount.Controllers
{
    public class BankController : Controller
    {
        private BankContext _context;
 
        public BankController(BankContext context)
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
                return RedirectToAction("ShowAccount", new {Id = newUser.UserId});
            }
            return View("Index",item);
        }

        [HttpGet]
        [Route("/account/{Id}")]
        public IActionResult ShowAccount(int Id)
        {
            User ShowUser = _context.Users.Include(user => user.AccountTransactions).SingleOrDefault(r => r.UserId == Id);
            
            ViewBag.User = ShowUser;

            return View("ShowAccount");
        }

        [HttpPost]
        [Route("/Account/{UserId}/AddTransaction")]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddTranstation(int UserId, decimal Amount)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(Amount);
            System.Console.WriteLine();
            if(Amount ==0) return RedirectToAction("ShowAccount", new  {Id = UserId });

            User ShowUser = _context.Users.Include(user => user.AccountTransactions).SingleOrDefault(r => r.UserId == UserId);
            // decimal CurrentBalance = ShowUser.AccountTransactions.Sum(r => r.Amount);

            if( ShowUser.CurrentBalance + Amount < 0) {
                ViewBag.ErrorMessage = $"You can not withdraw more than the current amount $ {ShowUser.CurrentBalance}";
                ViewBag.User = ShowUser;
                return View("ShowAccount"); 
            } else {
                ShowUser.AccountTransactions.Add(new AccountTransaction {
                    Amount = Amount,
                    TransactionDate = DateTime.Now
                });
                _context.SaveChanges();
            }

            ViewBag.User = ShowUser;
            return RedirectToAction("ShowAccount", new  {Id = UserId });
        }
        // public IActionResult About()
        // {
        //     ViewData["Message"] = "Your application description page.";

        //     return View();
        // }

        // public IActionResult Contact()
        // {
        //     ViewData["Message"] = "Your contact page.";

        //     return View();
        // }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
