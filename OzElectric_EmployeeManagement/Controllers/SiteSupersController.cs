﻿using System;
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
    public class SiteSupersController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: SiteSupers
        public async Task<ActionResult> Index()
        {
            return View(await db.SiteSupers.ToListAsync());
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
            SiteSuper siteSuper = await db.SiteSupers.FindAsync(id);
            db.SiteSupers.Remove(siteSuper);
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
