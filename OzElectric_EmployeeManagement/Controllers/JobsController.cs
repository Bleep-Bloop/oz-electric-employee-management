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
using System.Web.UI.WebControls;

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
        [Authorize(Roles = "Admin, Accounting")]
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
        [Authorize(Roles = "Admin, Accounting")]
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
        [Authorize(Roles = "Admin, Accounting")]
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
        [Authorize(Roles = "Admin, Accounting")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult ExportToCSV(object sender, EventArgs e)
        {

            string strQuery = "select JobNumber, JobName, LocationName, Address, City, ProvinceOrState, GenContractorContact, Foreman_ForemanID, GenContractor_GenContractorID, PM_PMID, Purchaser_PurchaserID, SiteSuper_SiteSuperID" +
                              " from Jobs";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(cmd);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=JobsTable.csv");
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

        [Authorize(Roles = "Admin")]
        public ActionResult ExportToExcel(object sender, EventArgs e)
        {
            string strQuery = "select JobNumber, JobName, LocationName, Address, City, ProvinceOrState, GenContractorContact, Foreman_ForemanID, GenContractor_GenContractorID, PM_PMID, Purchaser_PurchaserID, SiteSuper_SiteSuperID" +
                  " from Jobs";
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
             "attachment;filename=JobsTable.xls");
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
