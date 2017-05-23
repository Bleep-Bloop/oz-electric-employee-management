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
    public class HourRecordsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: HourRecords
        public async Task<ActionResult> Index()
        {
            return View(await db.HourRecords.ToListAsync());
        }

        // GET: HourRecords/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourRecord hourRecord = await db.HourRecords.FindAsync(id);
            if (hourRecord == null)
            {
                return HttpNotFound();
            }
            return View(hourRecord);
        }

        // GET: HourRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HourRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HourRecordID,DateTime,Hours,Employee,Job")] HourRecord hourRecord)
        {
            if (ModelState.IsValid)
            {
                db.HourRecords.Add(hourRecord);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hourRecord);
        }

        // GET: HourRecords/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourRecord hourRecord = await db.HourRecords.FindAsync(id);
            if (hourRecord == null)
            {
                return HttpNotFound();
            }
            return View(hourRecord);
        }

        // POST: HourRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HourRecordID,DateTime,Hours,Employee,Job")] HourRecord hourRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hourRecord).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hourRecord);
        }

        // GET: HourRecords/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourRecord hourRecord = await db.HourRecords.FindAsync(id);
            if (hourRecord == null)
            {
                return HttpNotFound();
            }
            return View(hourRecord);
        }

        // POST: HourRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HourRecord hourRecord = await db.HourRecords.FindAsync(id);
            db.HourRecords.Remove(hourRecord);
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
