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
    public class JobsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: Jobs
        public async Task<ActionResult> Index()
        {
            var jobs = db.Jobs.Include(j => j.Foreman).Include(j => j.GenContractor).Include(j => j.PM).Include(j => j.Purchaser).Include(j => j.SiteSuper);
            return View(await jobs.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // GET: Jobs/Create
        public ActionResult Create()
        {
            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FullName");
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name");
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName");
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name");
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name");
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JobID,LocationName,Address,City,Province,GenContractorContact,Foreman_ForemanID,GenContractor_GenContractorID,PM_PMID,Purchaser_PurchaserID,SiteSuper_SiteSuperID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FirstName", job.Foreman_ForemanID);
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", job.GenContractor_GenContractorID);
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FirstName", job.PM_PMID);
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", job.Purchaser_PurchaserID);
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", job.SiteSuper_SiteSuperID);
            return View(job);
        }

        // GET: Jobs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FirstName", job.Foreman_ForemanID);
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", job.GenContractor_GenContractorID);
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FirstName", job.PM_PMID);
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", job.Purchaser_PurchaserID);
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", job.SiteSuper_SiteSuperID);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JobID,LocationName,Address,City,Province,GenContractorContact,Foreman_ForemanID,GenContractor_GenContractorID,PM_PMID,Purchaser_PurchaserID,SiteSuper_SiteSuperID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FirstName", job.Foreman_ForemanID);
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", job.GenContractor_GenContractorID);
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FirstName", job.PM_PMID);
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", job.Purchaser_PurchaserID);
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", job.SiteSuper_SiteSuperID);
            return View(job);
        }

        // GET: Jobs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Job job = await db.Jobs.FindAsync(id);
            db.Jobs.Remove(job);
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
