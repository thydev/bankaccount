using System;
using System.Collections.Generic;
using System.Linq;

namespace bankaccount.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            AccountTransactions = new List<AccountTransaction>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<AccountTransaction> AccountTransactions { get; set; }

        public string FullName {
            get {
                return this.FirstName + " " + this.LastName;
            }
        }
        
        public decimal CurrentBalance {
            get {
                return this.AccountTransactions.Sum(r => r.Amount);
            }
        }
        
    }
}