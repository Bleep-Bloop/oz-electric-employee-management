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
    public class PurchasersController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: Purchasers
        public async Task<ActionResult> Index()
        {
            return View(await db.Purchasers.ToListAsync());
        }

        // GET: Purchasers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return HttpNotFound();
            }
            return View(purchaser);
        }

        // GET: Purchasers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Purchasers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PurchaserID,Name")] Purchaser purchaser)
        {
            if (ModelState.IsValid)
            {
                db.Purchasers.Add(purchaser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(purchaser);
        }

        // GET: Purchasers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return HttpNotFound();
            }
            return View(purchaser);
        }

        // POST: Purchasers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PurchaserID,Name")] Purchaser purchaser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(purchaser);
        }

        // GET: Purchasers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return HttpNotFound();
            }
            return View(purchaser);
        }

        // POST: Purchasers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            db.Purchasers.Remove(purchaser);
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
