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
    public class GenContractorsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: GenContractors
        public async Task<ActionResult> Index()
        {
            return View(await db.GenContractors.ToListAsync());
        }

        // GET: GenContractors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenContractor genContractor = await db.GenContractors.FindAsync(id);
            if (genContractor == null)
            {
                return HttpNotFound();
            }
            return View(genContractor);
        }

        // GET: GenContractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GenContractors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "GenContractorID,Name")] GenContractor genContractor)
        {
            if (ModelState.IsValid)
            {
                db.GenContractors.Add(genContractor);
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " created: " + " Gen Contractor ID: " + genContractor.GenContractorID + " Gen Contractor Name: " + genContractor.Name , User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }

            return View(genContractor);
        }

        // GET: GenContractors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenContractor genContractor = await db.GenContractors.FindAsync(id);
            if (genContractor == null)
            {
                return HttpNotFound();
            }
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " is attempting to edit. Previous values: " + " Gen Contractor ID: " + genContractor.GenContractorID + " Gen Contractor Name: " + genContractor.Name, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
            return View(genContractor);
        }

        // POST: GenContractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "GenContractorID,Name")] GenContractor genContractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genContractor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " finished editing. New values: " + " Gen Contractor ID: " + genContractor.GenContractorID + " Gen Contractor Name: " + genContractor.Name, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

                return RedirectToAction("Index");
            }
            return View(genContractor);
        }

        // GET: GenContractors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GenContractor genContractor = await db.GenContractors.FindAsync(id);
            if (genContractor == null)
            {
                return HttpNotFound();
            }
            return View(genContractor);
        }

        // POST: GenContractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                GenContractor genContractor = await db.GenContractors.FindAsync(id);
                db.GenContractors.Remove(genContractor);
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " deleted " + genContractor.Name + " " + genContractor.GenContractorID, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

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
