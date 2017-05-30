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

//added for export
using System.Web.UI;
using System.Text;
using System.IO;


using System.Data.SqlClient;
//using System.Text;
//using System.IO;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using iTextSharp.text.html;
//using iTextSharp.text.html.simpleparser;
using System.Web.UI.WebControls;

namespace OzElectric_EmployeeManagement.Controllers
{


    [Authorize(Roles ="Admin")]
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


            //Pass query to GetData() and it returns result as a datatable                  
        private DataTable GetData(SqlCommand cmd)
        {

                                      


            //Taken from Web.config will need to be changed when integrated in Ozz system
            String strConnString = "Data Source=patrickdatabase.database.windows.net;Initial Catalog=COMP2007DataBase_2017-05-30T01 -48Z;Integrated Security=False;User ID=patr9240;Password=OzzPassword123;Connect Timeout=15;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; 

            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(strConnString);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                con.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }


        public ActionResult ExportToWord()
        {
            
            //Get the data from database into datatable
            string strQuery = "select EmployeeNumber, FirstName, LastName, Address, City, ProvinceOrState, HomePhone, HomeCellPhone, WorkPhone, WorkCellPhone, EmergencyContactName, EmergencyContactPhone" +
                              " from Employees";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(cmd);

            //Create a dummy GridView
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=EmployeeTable.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return View();
            
        }

        public ActionResult ExportToCSV(object sender, EventArgs e)
        {

            string strQuery = "select EmployeeNumber, FirstName, LastName, Address, City, ProvinceOrState, HomePhone, HomeCellPhone, WorkPhone, WorkCellPhone, EmergencyContactName, EmergencyContactPhone" +
                              " from Employees";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(cmd);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=EmployeeTable.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";


            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < dt.Columns.Count; k++)
            {
                //add separator
                sb.Append(dt.Columns[k].ColumnName + ',');
            }
            //add new line
            sb.Append("\r\n");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    //add separator
                    sb.Append(dt.Rows[i][k].ToString().Replace(",", ";") + ',');
                }
                //append new line
                sb.Append("\r\n");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

            return View();
        }


 

        public ActionResult ExportToExcel(object sender, EventArgs e)
        {
            string strQuery = "select EmployeeNumber, FirstName, LastName, Address, City, ProvinceOrState, HomePhone, HomeCellPhone, WorkPhone, WorkCellPhone, EmergencyContactName, EmergencyContactPhone" +
                               " from Employees";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(cmd);

            //Create a dummy GridView
            GridView GridView1 = new GridView();
            GridView1.AllowPaging = false;
            GridView1.DataSource = dt;
            GridView1.DataBind();

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=EmployeeTable.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                //Apply text style to each Row
                GridView1.Rows[i].Attributes.Add("class", "textmode");
            }
            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();


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
