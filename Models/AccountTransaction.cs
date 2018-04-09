using System;
using System.Linq;

namespace bankaccount.Models
{
    public class AccountTransaction : BaseEntity
    {
        public int AccountTransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        
    }
}