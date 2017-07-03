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
    public class PhoneBooksController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: PhoneBooks
        public async Task<ActionResult> Index()
        {
            return View(await db.PhoneBooks.ToListAsync());
        }

        // GET: PhoneBooks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneBook phoneBook = await db.PhoneBooks.FindAsync(id);
            if (phoneBook == null)
            {
                return HttpNotFound();
            }
            return View(phoneBook);
        }

        // GET: PhoneBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PhoneBooks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ContactId,ContactName,WorkPhone,WorkCellPhone")] PhoneBook phoneBook)
        {
            if (ModelState.IsValid)
            {
                db.PhoneBooks.Add(phoneBook);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(phoneBook);
        }

        // GET: PhoneBooks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneBook phoneBook = await db.PhoneBooks.FindAsync(id);
            if (phoneBook == null)
            {
                return HttpNotFound();
            }
            return View(phoneBook);
        }

        // POST: PhoneBooks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ContactId,ContactName,WorkPhone,WorkCellPhone")] PhoneBook phoneBook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phoneBook).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(phoneBook);
        }

        // GET: PhoneBooks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhoneBook phoneBook = await db.PhoneBooks.FindAsync(id);
            if (phoneBook == null)
            {
                return HttpNotFound();
            }
            return View(phoneBook);
        }

        // POST: PhoneBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PhoneBook phoneBook = await db.PhoneBooks.FindAsync(id);
            db.PhoneBooks.Remove(phoneBook);
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
