using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OzElectric_EmployeeManagement.Models;

namespace OzElectric_EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ForemenController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: Foremen
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName_desc" : "LastName";

            var foremen = from f in db.Foremen
                      select f;

            switch (sortOrder)
            {
                case "FirstName_desc":
                    foremen = foremen.OrderByDescending(f => f.FirstName);
                    break;
                case "LastName":
                    foremen = foremen.OrderBy(f => f.LastName);
                    break;
                case "LastName_desc":
                    foremen = foremen.OrderByDescending(f => f.LastName);
                    break;
                default:
                    foremen = foremen.OrderBy(f => f.FirstName);
                    break;
            }

            return View(await foremen.ToListAsync());
        }

        // GET: Foremen/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foreman foreman = await db.Foremen.FindAsync(id);
            if (foreman == null)
            {
                return HttpNotFound();
            }
            return View(foreman);
        }

        // GET: Foremen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foremen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ForemanID,FirstName,LastName,Cell")] Foreman foreman)
        {
            if (ModelState.IsValid)
            {
                db.Foremen.Add(foreman);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(foreman);
        }

        // GET: Foremen/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foreman foreman = await db.Foremen.FindAsync(id);
            if (foreman == null)
            {
                return HttpNotFound();
            }
            return View(foreman);
        }

        // POST: Foremen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ForemanID,FirstName,LastName,Cell")] Foreman foreman)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foreman).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(foreman);
        }

        // GET: Foremen/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Foreman foreman = await db.Foremen.FindAsync(id);
            if (foreman == null)
            {
                return HttpNotFound();
            }
            return View(foreman);
        }

        // POST: Foremen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Foreman foreman = await db.Foremen.FindAsync(id);
            db.Foremen.Remove(foreman);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
