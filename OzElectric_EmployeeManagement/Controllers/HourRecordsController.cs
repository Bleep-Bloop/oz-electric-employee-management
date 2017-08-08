using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OzElectric_EmployeeManagement.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OzElectric_EmployeeManagement.Controllers
{
    public class HourRecordsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: HourRecords
        public async Task<ActionResult> Index(string sortOrder)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            //used to see who is currently logged in
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            
            var hourTracker = from h in db.HourRecords.Include(h => h.Job).Include(h => h.Employee)
                              select h;
            
            if (UserManager.GetRoles(currentUser.Id).Contains("Guest"))
            {
                hourTracker = from h in db.HourRecords.Include(h => h.Job)
                              where h.Employee_EmployeeID == -1
                              select h;
            }
            else if (UserManager.GetRoles(currentUser.Id).Contains("Admin") || UserManager.GetRoles(currentUser.Id).Contains("Accounting") || UserManager.GetRoles(currentUser.Id).Contains("Manager"))
            {
            }
            else if (UserManager.GetRoles(currentUser.Id).Contains("Employee"))
            {
                hourTracker = from h in db.HourRecords.Include(h => h.Job)
                            where h.Employee_EmployeeID == currentUser.Employee_EmployeeID
                            select h;
            }
            else
            {
                hourTracker = from h in db.HourRecords.Include(h => h.Job)
                              where h.Employee_EmployeeID == -1
                              select h;
            }
            return View(await hourTracker.ToListAsync());
        }

        // GET: HourRecords/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HourRecord hourRecord = await db.HourRecords.FindAsync(id);
            var hourTracker = from h in db.HourRecords.Include(h => h.Employee).Include(h => h.Job)
                              where h.HourRecordID == id.Value
                              select h;
            if (hourRecord == null)
            {
                return HttpNotFound();
            }
            return View(hourRecord);
        }

        // GET: HourRecords/Create
        public ActionResult Create()
        {
            ViewBag.Job_JobID = new SelectList(db.Jobs, "JobID", "JobName");
            return View();
        }




        // POST: HourRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "HourRecordID,DateTime,Hours,Employee_EmployeeID,Job_JobID,Comment")] HourRecord hourRecord)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            //used to see who is currently logged in
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                hourRecord.Employee_EmployeeID = currentUser.Employee_EmployeeID;
                db.HourRecords.Add(hourRecord);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //Possibly adding again for admin use?
            //ViewBag.Employee_EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", hourRecord.Employee_EmployeeID);
            
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
            var hourTracker = from h in db.HourRecords.Include(h => h.Employee)
                              where h.HourRecordID == id.Value
                              select h;
            if (hourRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.Job_JobID = new SelectList(db.Jobs, "JobID", "JobName", hourRecord.Job_JobID);
            ViewBag.Employee_EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", hourRecord.Employee_EmployeeID);

            return View(hourRecord);
        }

        // POST: HourRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HourRecordID,DateTime,Hours,Employee_EmployeeID,Job_JobID,Comment")] HourRecord hourRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hourRecord).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Job_JobID = new SelectList(db.Jobs, "JobID", "JobName", hourRecord.Job_JobID);
            ViewBag.Employee_EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", hourRecord.Employee_EmployeeID);

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
