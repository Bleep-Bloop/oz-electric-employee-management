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
            GenContractor genContractor = await db.GenContractors.FindAsync(id);
            db.GenContractors.Remove(genContractor);
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
