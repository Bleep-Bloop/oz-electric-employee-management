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
    public class PMsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: PMs
        public async Task<ActionResult> Index(string sortOrder)
        {

            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewBag.CitySortParm = sortOrder == "City" ? "City_desc" : "City";
            ViewBag.ProvinceOrStateSortParm = sortOrder == "ProvinceOrState" ? "ProvinceOrState_desc" : "ProvinceOrState";

            var pms = from p in db.PMs
                       select p;

            switch (sortOrder)
            {
                case "FirstName_desc":
                    pms = pms.OrderByDescending(p => p.FirstName);
                    break;

                case "LastName":
                    pms = pms.OrderBy(p => p.LastName);
                    break;
                case "LastName_desc":
                    pms = pms.OrderByDescending(p => p.LastName);
                    break;

                case "Address":
                    pms = pms.OrderBy(p => p.Address);
                    break;
                case "Address_desc":
                    pms = pms.OrderByDescending(p => p.Address);
                    break;
                case "City":
                    pms = pms.OrderBy(p => p.City);
                    break;
                case "City_desc":
                    pms = pms.OrderByDescending(p => p.City);
                    break;
                case "ProvinceOrState":
                    pms = pms.OrderBy(p => p.Province);
                    break;
                case "ProvinceOrState_desc":
                    pms = pms.OrderByDescending(p => p.Province);
                    break;

                default:
                    pms = pms.OrderBy(p => p.FirstName);
                    break;
            }

            return View(await pms.ToListAsync());
        }

        // GET: PMs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PM pM = await db.PMs.FindAsync(id);
            if (pM == null)
            {
                return HttpNotFound();
            }
            return View(pM);
        }

        // GET: PMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PMs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PMID,FirstName,LastName,Address,City,Province,Phone,WorkPhone")] PM pM)
        {
            if (ModelState.IsValid)
            {
                db.PMs.Add(pM);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pM);
        }

        // GET: PMs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PM pM = await db.PMs.FindAsync(id);
            if (pM == null)
            {
                return HttpNotFound();
            }
            return View(pM);
        }

        // POST: PMs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PMID,FirstName,LastName,Address,City,Province,Phone,WorkPhone")] PM pM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pM).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pM);
        }

        // GET: PMs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PM pM = await db.PMs.FindAsync(id);
            if (pM == null)
            {
                return HttpNotFound();
            }
            return View(pM);
        }

        // POST: PMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PM pM = await db.PMs.FindAsync(id);
            db.PMs.Remove(pM);
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
