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
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "Date_desc" : "";
            ViewBag.JobSortParm = sortOrder == "Job" ? "Job_desc" : "Job";
            ViewBag.HoursSortParm = sortOrder == "Hours" ? "Hours_desc" : "Hours";
            
            var hourTracker = from h in db.HourRecords.Include(h => h.DateTime).Include(h => h.Job).Include(h => h.Hours)
                       select h;

            switch (sortOrder)
            {
                case "Date_desc":
                    hourTracker = hourTracker.OrderByDescending(h => h.DateTime);
                    break;

                case "Job":
                    hourTracker = hourTracker.OrderBy(h => h.Job);
                    break;
                case "Job_desc":
                    hourTracker = hourTracker.OrderByDescending(h => h.Job);
                    break;

                case "Hours":
                    hourTracker = hourTracker.OrderBy(h => h.Hours);
                    break;
                case "Hours_desc":
                    hourTracker = hourTracker.OrderByDescending(h => h.Hours);
                    break;                            

                default:
                    hourTracker = hourTracker.OrderBy(h => h.DateTime);
                    break;
            }
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
            ViewBag.Job = new SelectList(db.Jobs, "JobID", "JobName");
            ViewBag.Employee = new SelectList(db.Employees, "EmployeeID", "FirstName");
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

            ViewBag.Job = new SelectList(db.Jobs, "JobID", "JobName", hourRecord.Job);
            ViewBag.Employee = new SelectList(db.Employees, "EmployeeID", "FirstName", hourRecord.Employee);
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
            ViewBag.Job = new SelectList(db.Jobs, "JobID", "JobName", hourRecord.Job);
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
            ViewBag.Job = new SelectList(db.Jobs, "JobID", "JobName", hourRecord.Job);
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
