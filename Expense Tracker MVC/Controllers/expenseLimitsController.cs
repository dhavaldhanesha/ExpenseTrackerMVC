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
    public class expenseLimitsController : ApiController
    {
        private ExpenseTrackerEntities db = new ExpenseTrackerEntities();

        // GET: api/expenseLimits
        public IQueryable<ExpenseLimit> GetexpenseLimits()
        {
            return db.ExpenseLimits;
        }

        [Route("api/expenseLimit/totalExpenseLimit")]
        public IHttpActionResult GetTotalLimit()
        {
            var budget = db.ExpenseLimits.Select(u => u.totalExpenseLimit).ToList();
            return Ok(budget);
        }

        [Route("api/expenseLimit/actualExpense")]
        public IHttpActionResult GetTotalExpense()
        {
            var budget = db.ExpenseLimits.Select(u => u.actualExpense).ToList();
            return Ok(budget);
        }

        //UPDATE TOTALEXPENSE
        [Route("api/expenseLimits/updateTotalExpense/{id}")]
        public IHttpActionResult GetUpdateBudgetTotalExpense(int id)
        {
            var obj = db.ExpenseLimits.FirstOrDefault(x => x.id == id);
            obj.actualExpense = db.categories.Sum(c => c.categoryExpense);
            db.SaveChanges();
            return Ok(obj.totalExpenseLimit);
        }



        // GET: api/expenseLimits/5
        [ResponseType(typeof(ExpenseLimit))]
        public IHttpActionResult GetexpenseLimit(int id)
        {
            ExpenseLimit expenseLimit = db.ExpenseLimits.Find(id);
            if (expenseLimit == null)
            {
                return NotFound();
            }

            return Ok(expenseLimit);
        }

        // PUT: api/expenseLimits/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutexpenseLimit(int id, ExpenseLimit expenseLimit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expenseLimit.totalExpenseLimit)
            {
                return BadRequest();
            }

            db.Entry(expenseLimit).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!expenseLimitExists(id))
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

        // POST: api/expenseLimits
        [ResponseType(typeof(ExpenseLimit))]
        public IHttpActionResult PostexpenseLimit(ExpenseLimit expenseLimit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ExpenseLimit obj = db.ExpenseLimits.Find(expenseLimit.id);
            obj.totalExpenseLimit = expenseLimit.totalExpenseLimit;
            /*db.ExpenseLimits.Add(expenseLimit);*/

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (expenseLimitExists(expenseLimit.totalExpenseLimit))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = expenseLimit.totalExpenseLimit }, expenseLimit);
        }

        // DELETE: api/expenseLimits/5
        [ResponseType(typeof(ExpenseLimit))]
        public IHttpActionResult DeleteexpenseLimit(int id)
        {
            ExpenseLimit expenseLimit = db.ExpenseLimits.Find(id);
            if (expenseLimit == null)
            {
                return NotFound();
            }

            db.ExpenseLimits.Remove(expenseLimit);
            db.SaveChanges();

            return Ok(expenseLimit);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool expenseLimitExists(int id)
        {
            return db.ExpenseLimits.Count(e => e.totalExpenseLimit == id) > 0;
        }

        [System.Web.Http.Route("api/expenseLimits/actualExpense")]
        public IHttpActionResult GetactualExpense()
        {
            var expenseAmount = db.ExpenseLimits.Where(x => x.id == 1).Select(e => e.actualExpense).ToList();
            return Ok(expenseAmount);
        }

    }
}