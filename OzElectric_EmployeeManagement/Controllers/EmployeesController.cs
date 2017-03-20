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
    public class EmployeesController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: Employees
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.EmployeeNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "EmployeeNumber_desc" : "";
            ViewBag.FirstNameSortParm = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            ViewBag.LastNameSortParm = sortOrder == "LastName" ? "LastName_desc" : "LastName";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "Address_desc" : "Address";
            ViewBag.CitySortParm = sortOrder == "City" ? "City_desc" : "City";
            ViewBag.ProvinceOrStateSortParm = sortOrder == "ProvinceOrState" ? "ProvinceOrState_desc" : "ProvinceOrState";
            var employees = from emp in db.Employees
                            select emp;


            switch (sortOrder)
            {
                case "EmployeeNumber_desc":
                    employees = employees.OrderByDescending(emp => emp.EmployeeNumber);
                    break;
                case "FirstName":
                    employees = employees.OrderBy(emp => emp.FirstName);
                    break;
                case "FirstName_desc":
                    employees = employees.OrderByDescending(emp => emp.FirstName);
                    break;
                case "LastName":
                    employees = employees.OrderBy(emp => emp.LastName);
                    break;
                case "LastName_desc":
                    employees = employees.OrderByDescending(emp => emp.LastName);
                    break;
                case "Address":
                    employees = employees.OrderBy(emp => emp.Address);
                    break;
                case "Address_desc":
                    employees = employees.OrderByDescending(emp => emp.Address);
                    break;
                case "City":
                    employees = employees.OrderBy(emp => emp.City);
                    break;
                case "City_desc":
                    employees = employees.OrderByDescending(emp => emp.City);
                    break;
                case "ProvinceOrState":
                    employees = employees.OrderBy(emp => emp.ProvinceOrState);
                    break;
                case "ProvinceOrState_desc":
                    employees = employees.OrderByDescending(emp => emp.ProvinceOrState);
                    break;
                default:
                    employees = employees.OrderBy(emp => emp.EmployeeNumber);
                    break;

            }
            
            return View(await employees.ToListAsync());
        }
             
        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,EmployeeNumber,FirstName,LastName,Address,City,ProvinceOrState,HomePhone,HomeCellPhone,WorkPhone,WorkCellPhone,EmergencyContactName,EmergencyContactPhone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeNumber,FirstName,LastName,Address,City,ProvinceOrState,HomePhone,HomeCellPhone,WorkPhone,WorkCellPhone,EmergencyContactName,EmergencyContactPhone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employees.FindAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Employee employee = await db.Employees.FindAsync(id);
            db.Employees.Remove(employee);
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
