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
    public class homesController : ApiController
    {
        private ExpenseTrackerEntities db = new ExpenseTrackerEntities();

        // GET: api/homes
        public IQueryable<home> Gethomes()
        {
            return db.homes;
        }

        // GET: api/homes/5
        [ResponseType(typeof(home))]
        public IHttpActionResult Gethome(int id)
        {
            home home = db.homes.Find(id);
            if (home == null)
            {
                return NotFound();
            }

            return Ok(home);
        }

        // PUT: api/homes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puthome(int id, home home)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != home.id)
            {
                return BadRequest();
            }

            db.Entry(home).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!homeExists(id))
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

        // POST: api/homes
        [ResponseType(typeof(home))]
        public IHttpActionResult Posthome(home home)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.homes.Add(home);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = home.id }, home);
        }

        // DELETE: api/homes/5
        [ResponseType(typeof(home))]
        public IHttpActionResult Deletehome(int id)
        {
            home home = db.homes.Find(id);
            if (home == null)
            {
                return NotFound();
            }

            db.homes.Remove(home);
            db.SaveChanges();

            return Ok(home);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool homeExists(int id)
        {
            return db.homes.Count(e => e.id == id) > 0;
        }
    }
}