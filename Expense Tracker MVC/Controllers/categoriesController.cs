using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Xml.Linq;
using ExpTrackerAPI.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace ExpTrackerAPI.Controllers
{
    public class categoriesController : ApiController
    {
        private ExpenseTrackerEntities db = new ExpenseTrackerEntities();

        // GET: api/categories
        public IQueryable<category> Getcategories()
        {
            return db.categories;
        }

        [System.Web.Http.Route("api/categories/categoryLimit/{id}")]
        public IHttpActionResult GetCategoryLimit(int id)
        {
            var budget = db.categories.Where(x => x.categoryId == id).Select(u => u.categoryLimit).ToList();
            return Ok(budget);
        }

        [System.Web.Http.Route("api/categories/categoryExpense/{id}")]
        public IHttpActionResult GetCategoryExpense(int id)
        {
            var budget = db.categories.Where(x => x.categoryId == id).Select(u => u.categoryExpense).ToList();
            return Ok(budget);
        }

        // GET: api/categories/5
        [ResponseType(typeof(category))]
        public IHttpActionResult Getcategory(int id)
        {
            category category = db.categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcategory(int id, category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.categoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!categoryExists(id))
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

        // POST: api/categories
        [ResponseType(typeof(category))]
        public IHttpActionResult Postcategory(category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.categoryId }, category);
        }

        // DELETE: api/categories/5
        [ResponseType(typeof(category))]
        public IHttpActionResult Deletecategory(int id)
        {
            category category = db.categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool categoryExists(int id)
        {
            return db.categories.Count(e => e.categoryId == id) > 0;
        }

        
        [System.Web.Http.Route("api/categories/IsExist")]
        public bool PostIsExist(category category)
        {
            var categoryExist = db.categories.FirstOrDefault(x => x.categoryName == category.categoryName);
            if (categoryExist == null)
            {
                return false;
            }
            return true;
        }

        [System.Web.Http.Route("api/categories/IsAnother")]
        public bool PostIsAnother(category category)
        {
            var categoryExist = db.categories.FirstOrDefault(x => x.categoryName == category.categoryName && x.categoryId != category.categoryId);
            if (categoryExist == null)
            {
                return false;
            }
            return true;
        }

        //UPDATE CATEGORYEXPENSE
        [System.Web.Http.Route("api/categories/updateCategoryExpense")]
        public IHttpActionResult PutupdateCategoryExpense(expense expense)
        {
            var obj = db.categories.FirstOrDefault(x => x.categoryId == expense.categoryId);
            if (obj != null)
            {
                obj.categoryExpense = db.expenses.Where(x => x.categoryId == expense.categoryId).Sum(x => x.expenseAmount);
                db.SaveChanges();
            }
            return Ok(obj.categoryExpense);
        }

        
    }
}