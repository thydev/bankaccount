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
    public class BankController : Controller
    {
        private BankContext _context;
 
        public BankController(BankContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("/account/{Id}")]
        public IActionResult ShowAccount(int Id)
        {
            if (!IsUserLoggedIn()) return RedirectToAction("Login", "User");

            User ShowUser = _context.Users.Include(user => user.AccountTransactions).SingleOrDefault(r => r.UserId == Id);
            ViewBag.User = ShowUser;

            return View("ShowAccount");
        }

        [HttpPost]
        [Route("/Account/{UserId}/AddTransaction")]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddTranstation(int UserId, decimal Amount)
        {
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
 
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool IsUserLoggedIn()
        {
            if (HttpContext.Session.GetInt32("UserId") == null || HttpContext.Session.GetInt32("UserId") == 0 ) {
                return false;
            } else {
                return true;
            }
        }
    }
}
