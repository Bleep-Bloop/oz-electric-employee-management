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
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.JobNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "JobNumber_desc" : "";
            ViewBag.JobNameSortParm = sortOrder == "JobName" ? "JobName_desc" : "JobName";
            ViewBag.ForemanSortParm = sortOrder == "Foreman" ? "Foreman_desc" : "Foreman";
            ViewBag.GenContractorSortParm = sortOrder == "GenContractor" ? "GenContractor_desc" : "GenContractor";
            ViewBag.PMSortParm = sortOrder == "PM" ? "PM_desc" : "PM";
            ViewBag.PurchaserSortParm = sortOrder == "Purchaser" ? "Purchaser_desc" : "Purchaser";
            ViewBag.SiteSuperSortParm = sortOrder == "SiteSuper" ? "SiteSuper_desc" : "SiteSuper";
            ViewBag.LocationNameSortParm = sortOrder == "LocationName" ? "LocationName_desc" : "LocationName";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewBag.CitySortParm = sortOrder == "City" ? "City_desc" : "City";
            ViewBag.ProvinceOrStateSortParm = sortOrder == "ProvinceOrState" ? "ProvinceOrState_desc" : "ProvinceOrState";
            ViewBag.GenContractorContactSortParm = sortOrder == "GenContractorContact" ? "GenContractorContact_desc" : "GenContractorContact";

            var jobs = from j in db.Jobs.Include(j => j.Foreman).Include(j => j.GenContractor).Include(j => j.PM).Include(j => j.Purchaser).Include(j => j.SiteSuper)
                       select j;
            
            switch (sortOrder)
            {
                case "JobNumber_desc":
                    jobs = jobs.OrderByDescending(j => j.JobNumber);
                    break;

                case "JobName":
                    jobs = jobs.OrderBy(j => j.JobName);
                    break;
                case "JobName_desc":
                    jobs = jobs.OrderByDescending(j => j.JobName);
                    break;

                case "Foreman":
                    jobs = jobs.OrderBy(j => j.Foreman.FirstName);
                    break;
                case "Foreman_desc":
                    jobs = jobs.OrderByDescending(j => j.Foreman.FirstName);
                    break;

                case "GenContractor":
                    jobs = jobs.OrderBy(j => j.GenContractor.Name);
                    break;
                case "GenContractor_desc":
                    jobs = jobs.OrderByDescending(j => j.GenContractor.Name);
                    break;

                case "PM":
                    jobs = jobs.OrderBy(j => j.PM.FirstName);
                    break;
                case "PM_desc":
                    jobs = jobs.OrderByDescending(j => j.PM.FirstName);
                    break;

                case "Purchaser":
                    jobs = jobs.OrderBy(j => j.Purchaser.Name);
                    break;
                case "Purchaser_desc":
                    jobs = jobs.OrderByDescending(j => j.Purchaser.Name);
                    break;

                case "SiteSuper":
                    jobs = jobs.OrderBy(j => j.SiteSuper.Name);
                    break;
                case "SiteSuper_desc":
                    jobs = jobs.OrderByDescending(j => j.SiteSuper.Name);
                    break;

                case "LocationName":
                    jobs = jobs.OrderBy(j => j.LocationName);
                    break;
                case "LocationName_desc":
                    jobs = jobs.OrderByDescending(j => j.LocationName);
                    break;

                case "Address":
                    jobs = jobs.OrderBy(j => j.Address);
                    break;
                case "Address_desc":
                    jobs = jobs.OrderByDescending(j => j.Address);
                    break;

                case "City":
                    jobs = jobs.OrderBy(j => j.City);
                    break;
                case "City_desc":
                    jobs = jobs.OrderByDescending(j => j.City);
                    break;

                case "ProvinceOrState":
                    jobs = jobs.OrderBy(j => j.ProvinceOrState);
                    break;
                case "ProvinceOrState_desc":
                    jobs = jobs.OrderByDescending(j => j.ProvinceOrState);
                    break;

                case "GenContractorContact":
                    jobs = jobs.OrderBy(j => j.GenContractorContact);
                    break;
                case "GenContractorContact_desc":
                    jobs = jobs.OrderByDescending(j => j.GenContractorContact);
                    break;

                default:
                    jobs = jobs.OrderBy(j => j.JobNumber);
                    break;
            }
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
        public async Task<ActionResult> Create([Bind(Include = "JobID,JobNumber,JobName,LocationName,Address,City,ProvinceOrState,GenContractorContact,Foreman_ForemanID,GenContractor_GenContractorID,PM_PMID,Purchaser_PurchaserID,SiteSuper_SiteSuperID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Jobs.Add(job);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FullName", job.Foreman_ForemanID);
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", job.GenContractor_GenContractorID);
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName", job.PM_PMID);
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
            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FullName", job.Foreman_ForemanID);
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", job.GenContractor_GenContractorID);
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName", job.PM_PMID);
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", job.Purchaser_PurchaserID);
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", job.SiteSuper_SiteSuperID);
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "JobID,JobNumber,JobName,LocationName,Address,City,ProvinceOrState,GenContractorContact,Foreman_ForemanID,GenContractor_GenContractorID,PM_PMID,Purchaser_PurchaserID,SiteSuper_SiteSuperID")] Job job)
        {
            if (ModelState.IsValid)
            {
                db.Entry(job).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Foreman_ForemanID = new SelectList(db.Foremen, "ForemanID", "FullName", job.Foreman_ForemanID);
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", job.GenContractor_GenContractorID);
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName", job.PM_PMID);
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
