using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ExpTrackerAPI.Models;

namespace ExpTrackerAPI.Controllers
{
    public class expensesController : ApiController
    {
        private ExpenseTrackerEntities db = new ExpenseTrackerEntities();

        // GET: api/expenses
        /*public IQueryable<expense> Getexpenses()
        {
            return db.expenses;
        }*/

        public IHttpActionResult GetExpenses()
        {
            var expenses = db.expenses.Join(db.categories, e => e.categoryId, c => c.categoryId, (e, c) => new { e, c })
                .Select(ec => new categoryName()
                {
                    expenseId = ec.e.expenseId,
                    CategoryName = ec.c.categoryName,
                    expenseTitle = ec.e.expenseTitle,
                    expenseDesription = ec.e.expenseDesription,
                    date = ec.e.date,
                    expenseAmount = ec.e.expenseAmount
                }).ToList();
            return Ok(expenses);
        }

        // GET: api/expenses/5
        [ResponseType(typeof(expense))]
        public IHttpActionResult Getexpense(int id)
        {
            expense expense = db.expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        // PUT: api/expenses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putexpense(int id, expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expense.expenseId)
            {
                return BadRequest();
            }
            expense.date = DateTime.Now;
            if(expense.expenseTitle == "")
            {
                expense.expenseTitle = "None";
            }
            db.Entry(expense).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!expenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/expenses
        [ResponseType(typeof(expense))]
        public IHttpActionResult Postexpense(expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            expense.date = DateTime.Now;
            
            db.expenses.Add(expense);
            
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expense.expenseId }, expense);
        }

        // DELETE: api/expenses/5
        [ResponseType(typeof(expense))]
        public IHttpActionResult Deleteexpense(int id)
        {
            expense expense = db.expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }
            var obj = db.categories.FirstOrDefault(x => x.categoryId == expense.categoryId);
            obj.categoryExpense = obj.categoryExpense - expense.expenseAmount;
            db.expenses.Remove(expense);
            db.SaveChanges();

            return Ok(expense);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool expenseExists(int id)
        {
            return db.expenses.Count(e => e.expenseId == id) > 0;
        }

        [System.Web.Http.Route("api/expenses/categoryNameList")]
        public IHttpActionResult GetcategoryNameList()
        {
            var categoryNameList = db.categories.Select(e => e.categoryName).ToList();
            return Ok(categoryNameList);
        }

        [Route("api/expenses/expenseAmount/{id}")]
        public IHttpActionResult GetexpenseAmount(int id)
        {
            var expenseAmount = db.expenses.Where(x => x.expenseId == id).Select(e => e.expenseAmount).ToList();
            return Ok(expenseAmount);
        }
        //CATEGORY AMOUNT: 
        [Route("api/expenses/totalExpenseByCategory")]
        public decimal PostTotalCategoryAmount(expense expense)
        {
            var totCatExp = db.expenses.Where(e => e.categoryId == expense.categoryId).Sum(e => e.expenseAmount);
            return totCatExp;
        }

        //GET EXPENSE BY CATEGORY: 
        [Route("api/expenses/ExpenseByCategory/{id}")]
        public IHttpActionResult GetExpenseByCategory(int id)
        {
            var expenses = db.expenses.Join(db.categories, e => e.categoryId, c => c.categoryId, (e, c) => new { e, c })
                .Where(ec => ec.e.categoryId == id)
                .Select(ec => new categoryName()
                {
                    expenseId = ec.e.expenseId,
                    CategoryName = ec.c.categoryName,
                    expenseTitle = ec.e.expenseTitle,
                    expenseDesription = ec.e.expenseDesription,
                    date = ec.e.date,
                    expenseAmount = ec.e.expenseAmount
                }).ToList();
            return Ok(expenses);
        }
    }
}