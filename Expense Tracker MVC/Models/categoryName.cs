using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpTrackerAPI.Models
{
    public class categoryName
    {
        public int expenseId { get; set; }
        public string expenseTitle { get; set; }
        public string expenseDesription { get; set; }
        public int categoryId { get; set; }
        public string CategoryName { get; set; }
        public int expenseAmount { get; set; }
        public System.DateTime date { get; set; }
    }
}