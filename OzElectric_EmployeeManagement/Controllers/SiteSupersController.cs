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
    public class SiteSupersController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: SiteSupers
        public async Task<ActionResult> Index(string sortOrder)
        {

            ViewBag.SiteSuperSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            var SiteSuper = from s in db.SiteSupers
                            select s;

            switch (sortOrder)
            {
                case "Name_desc":
                    SiteSuper = SiteSuper.OrderByDescending(s => s.Name);
                    break;
                default:
                    SiteSuper = SiteSuper.OrderBy(s => s.Name);
                    break;

            }
            return View(await SiteSuper.ToListAsync());
        }

        // GET: SiteSupers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteSuper siteSuper = await db.SiteSupers.FindAsync(id);
            if (siteSuper == null)
            {
                return HttpNotFound();
            }
            return View(siteSuper);
        }

        // GET: SiteSupers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteSupers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SiteSuperID,Name,Phone,Work")] SiteSuper siteSuper)
        {
            if (ModelState.IsValid)
            {
                db.SiteSupers.Add(siteSuper);
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " created: " + "Site Super ID: " + siteSuper.SiteSuperID + " Site Super Name: " + siteSuper.Name + " Site Super Phone: " + siteSuper.Phone + " Site Super Work: " + siteSuper.Work, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }

            return View(siteSuper);
        }

        // GET: SiteSupers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteSuper siteSuper = await db.SiteSupers.FindAsync(id);
            if (siteSuper == null)
            {
                return HttpNotFound();
            }
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " is attempting to edit. Previous values: " + "Site Super ID: " + siteSuper.SiteSuperID + " Site Super Name: " + siteSuper.Name + " Site Super Phone: " + siteSuper.Phone + " Site Super Work: " + siteSuper.Work, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
            return View(siteSuper);
        }

        // POST: SiteSupers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SiteSuperID,Name,Phone,Work")] SiteSuper siteSuper)
        {
            if (ModelState.IsValid)
            {
                db.Entry(siteSuper).State = EntityState.Modified;
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " finished editing. New values: " + "Site Super ID: " + siteSuper.SiteSuperID + " Site Super Name: " + siteSuper.Name + " Site Super Phone: " + siteSuper.Phone + " Site Super Work: " + siteSuper.Work, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }
            return View(siteSuper);
        }

        // GET: SiteSupers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteSuper siteSuper = await db.SiteSupers.FindAsync(id);
            if (siteSuper == null)
            {
                return HttpNotFound();
            }
            return View(siteSuper);
        }

        // POST: SiteSupers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                SiteSuper siteSuper = await db.SiteSupers.FindAsync(id);
                db.SiteSupers.Remove(siteSuper);
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " deleted " + siteSuper.Name + " " + siteSuper.SiteSuperID, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write("<script language='javascript'>alert(" + e.Message + ")</script>");
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " encountered error when attempting delete " + " " + e, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }
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
