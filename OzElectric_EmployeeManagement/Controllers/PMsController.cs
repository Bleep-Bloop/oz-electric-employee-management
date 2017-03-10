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
    public class PMsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: PMs
        public async Task<ActionResult> Index()
        {
            return View(await db.PMs.ToListAsync());
        }

        // GET: PMs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PM pM = await db.PMs.FindAsync(id);
            if (pM == null)
            {
                return HttpNotFound();
            }
            return View(pM);
        }

        // GET: PMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PMID,FirstName,LastName,Address,City,Province,Phone,WorkPhone")] PM pM)
        {
            if (ModelState.IsValid)
            {
                db.PMs.Add(pM);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pM);
        }

        // GET: PMs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PM pM = await db.PMs.FindAsync(id);
            if (pM == null)
            {
                return HttpNotFound();
            }
            return View(pM);
        }

        // POST: PMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PMID,FirstName,LastName,Address,City,Province,Phone,WorkPhone")] PM pM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pM).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pM);
        }

        // GET: PMs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PM pM = await db.PMs.FindAsync(id);
            if (pM == null)
            {
                return HttpNotFound();
            }
            return View(pM);
        }

        // POST: PMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PM pM = await db.PMs.FindAsync(id);
            db.PMs.Remove(pM);
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
