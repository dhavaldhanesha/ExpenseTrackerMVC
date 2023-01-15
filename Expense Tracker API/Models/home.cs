using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExpTrackerAPI.Models
{
    public class home
    {
        public IEnumerable<category> Categories { get; set; }

        public IEnumerable<categoryName> Expenses { get; set; }

        public decimal Limit { get; set; }
        public decimal ExpenseAmt { get; set; }

        public int usage { get; set; }
        public int categoryId { get; set; }

        public string categoryName { get; set; }
        public int categoryExpense { get; set; }

        [Required(ErrorMessage = "Category Limit is Mendatory")]
        public int categoryLimit { get; set; }

        public int id { get; set; }
        public int totalExpenseLimit { get; set; }

        public int actualExpense { get; set; }

        public int expenseId { get; set; }
        public string expenseTitle { get; set; }
        public string expenseDesription { get; set; }
        public int expenseAmount { get; set; }
        public System.DateTime date { get; set; }
    }
}