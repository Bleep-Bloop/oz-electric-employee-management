using System;
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
using System.Diagnostics;
using System.Collections.Generic;

namespace OzElectric_EmployeeManagement.Controllers
{
    public class JobsController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: Jobs
        public async Task<ActionResult> Index(string sortOrder)
        {
            var jobs = from j in db.Jobs.Include(j => j.Foreman).Include(j => j.GenContractor).Include(j => j.PM).Include(j => j.Purchaser).Include(j => j.SiteSuper)
                       select j;

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


        //Grab users hours and export to excel (for now)
        public ActionResult getAllJobHours(string wantedJob)
        {
            Debug.WriteLine(wantedJob);
            Debug.WriteLine("ShoudaWrotethatjob");

            string strQuery = "select HourRecordID, DateTime, Hours, Employee_EmployeeID, Job_JobID, Comment " +
                              " from HourRecords where Job_JobID = " + wantedJob;
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
             "attachment;filename=" + wantedJob + "HourRecords.xls");
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

        }//END getAllJobHours()




        // GET: Jobs/Create
        [Authorize(Roles = "Admin, Accounting")]
        public ActionResult Create()
        {
            //populates multiselect list with foremen and adds it to the view model
            MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName");
            JobViewModel model = new JobViewModel { Foremen = foremenList };

            //populates dropdowns for foreign key fields
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name");
            ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName");
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name");
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name");

            return View(model);
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JobID,JobNumber,JobName,LocationName,Address,City,ProvinceOrState,GenContractorContact,GenContractor_GenContractorID,PM_PMID,Purchaser_PurchaserID,SiteSuper_SiteSuperID,Foremen,ForemanIDs")] JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                //string used for log file
                string foremenLogFile = "";
                //creates a new instance of job
                Job job = new Job
                {
                    JobID = model.JobID,
                    JobNumber = model.JobNumber,
                    JobName = model.JobName,
                    LocationName = model.LocationName,
                    Address = model.Address,
                    City = model.City,
                    ProvinceOrState = model.ProvinceOrState,
                    GenContractorContact = model.GenContractorContact,
                    GenContractor_GenContractorID = model.GenContractor_GenContractorID,
                    PM_PMID = model.PM_PMID,
                    Purchaser_PurchaserID = model.Purchaser_PurchaserID,
                    SiteSuper_SiteSuperID = model.SiteSuper_SiteSuperID
            };
                //if there are foremen selected, create new relationships between the new job and the foremen
                if (model.ForemanIDs != null)
                {
                    foreach (var ID in model.ForemanIDs)
                    {
                        var foreman = db.Foremen.Find(int.Parse(ID));
                        //adding the foreman to the log file
                        foremenLogFile = foremenLogFile + foreman.FirstName + " " + foreman.LastName;
                        try
                        {
                            job.Foremen.Add(foreman);
                        }
                        catch (Exception ex)
                        {
                            return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                        }
                    }
                }
                try
                {
                    db.Jobs.Add(job);
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                }
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " created: Job ID: " + job.JobID + " Job Number: " + job.JobNumber + " Job Name: " + job.JobName + " Job Location: " + job.LocationName + " Job Address: " + job.Address + " City: " + job.City + " Province/State: " + job.ProvinceOrState + " Gen Contractor Contact: " + job.GenContractorContact + " Foreman Name: " + foremenLogFile + " Gen Contractor Id: " + job.GenContractor_GenContractorID + " PM_PMID: " + job.PM_PMID + " Purchaser ID: " + job.Purchaser_PurchaserID + " Site Super ID: " + job.SiteSuper_SiteSuperID, User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }
            //else something was wrong (like a field incorrectly filled out) return to create with errors
            else
            {
                //fills dropdowns/select list with data previously entered before attempting submit.
                MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName", model.ForemanIDs);
                model.Foremen = foremenList;
                ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", model.GenContractor_GenContractorID);
                ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName", model.PM_PMID);
                ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", model.Purchaser_PurchaserID);
                ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", model.SiteSuper_SiteSuperID);
                ModelState.AddModelError("", "Something went wrong, please look for errors below.");
                return View(model);
            }     
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<ActionResult> Edit(int? ID)
        {
            if (ID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = await db.Jobs.FindAsync(ID);
            if (job == null)
            {
                return HttpNotFound();
            }
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " attempting to edit. Previous values: Job ID: " + job.JobID + " Job Number: " + job.JobNumber + " Job Name: " + job.JobName + " Job Location: " + job.LocationName + " Job Address: " + job.Address + " City: " + job.City + " Province/State: " + job.ProvinceOrState + " Gen Contractor Contact: " + job.GenContractorContact + " Gen Contractor Id: " + job.GenContractor_GenContractorID + " PM_PMID: " + job.PM_PMID + " Purchaser ID: " + job.Purchaser_PurchaserID + " Site Super ID: " + job.SiteSuper_SiteSuperID, User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));
            //instantiate view model
            JobViewModel model = new JobViewModel
            {
                JobID = job.JobID,
                JobNumber = job.JobNumber,
                JobName = job.JobName,
                LocationName = job.LocationName,
                Address = job.Address,
                City = job.City,
                ProvinceOrState = job.ProvinceOrState,
                GenContractorContact = job.GenContractorContact,
                GenContractor_GenContractorID = job.GenContractor_GenContractorID,
                PM_PMID = job.PM_PMID,
                Purchaser_PurchaserID = job.Purchaser_PurchaserID,
                SiteSuper_SiteSuperID = job.SiteSuper_SiteSuperID
            };
            //retrieve list of foremen related to job
            var jobForemen = db.Foremen.Where(i => i.Jobs.Any(j => j.JobID.Equals(job.JobID))).ToList();
            if(jobForemen != null)
            {
                //init array to number of foremen related to job
                string[] jobFormenIDs = new string[jobForemen.Count];
                //set value of jobForemen.Count so the for loop doesn't need to work it out every iteration
                int length = jobForemen.Count;
                //loop through each foremen, store each ID in array
                for (int i = 0; i < length; i++)
                {
                    jobFormenIDs[i] = jobForemen[i].ForemanID.ToString();
                }
                //instantiate MultiSelectList, selecting jobForemenIDs array
                MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName", jobFormenIDs);
                //populates dropdowns for foreign key fields
                ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", model.GenContractor_GenContractorID);
                ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName", model.PM_PMID);
                ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", model.Purchaser_PurchaserID);
                ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", model.SiteSuper_SiteSuperID);
                //add foremenList to the Foremen property of the view model
                model.Foremen = foremenList;
                //return viewmodel
                return View(model);
            }
            else
            {
                //else instantiate MultiSelectList without any preselected values
                MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName");
                //populates dropdowns for foreign key fields
                ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name");
                ViewBag.PM_PMID = new SelectList(db.PMs, "PMID", "FullName");
                ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name");
                ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name");
                //add foremenList to the Foremen property of the view model
                model.Foremen = foremenList;
                //return viewmodel
                return View(model);
            }
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Accounting")]
        public async Task<ActionResult> Edit([Bind(Include = "JobID,JobNumber,JobName,LocationName,Address,City,ProvinceOrState,GenContractorContact,GenContractor_GenContractorID,PM_PMID,Purchaser_PurchaserID,SiteSuper_SiteSuperID,Foremen,ForemanIDs")] JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                Job job = db.Jobs.Find((model.JobID));
                if (job == null)
                {
                    return HttpNotFound();
                }

                //change Job information per viewmodel
                job.JobNumber = model.JobNumber;
                job.JobName = model.JobName;
                job.LocationName = model.LocationName;
                job.Address = model.Address;
                job.City = model.City;
                job.ProvinceOrState = model.ProvinceOrState;
                job.GenContractorContact = model.GenContractorContact;
                job.GenContractor_GenContractorID = model.GenContractor_GenContractorID;
                job.PM_PMID = model.PM_PMID;
                job.Purchaser_PurchaserID = model.Purchaser_PurchaserID;
                job.SiteSuper_SiteSuperID = model.SiteSuper_SiteSuperID;

                //check if any foremen were selected by user in the form
                if (model.ForemanIDs.Count > 0)
                {
                    //instantiate list to store each of the foremen in the viewmodel for later comparison
                    List<Foreman> viewModelForemen = new List<Foreman>();
                    //loop through each foreman ID
                    foreach (var ID in model.ForemanIDs)
                    {
                        //Retrieve forman from DB
                        var foreman = db.Foremen.Find(int.Parse(ID));
                        if (foreman != null)
                        {
                            //try to add foreman to tracking list of viewmodelforemen and job foremen
                            try
                            {
                                job.Foremen.Add(foreman);
                                viewModelForemen.Add(foreman);
                            }
                            catch(Exception ex)
                            {
                                return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                            }
                        }
                    }
                    //create list of foremen from DB
                    var allForemen = db.Foremen.ToList();
                    //exculude viewModelForemen from allForemen list, this creates list of foremen that need to be removed from the job.
                    var foremenToRemove = allForemen.Except(viewModelForemen);
                    //loop through foremen to remove, and remove them
                    foreach (var foreman in foremenToRemove)
                    {
                        try
                        {
                            //remove foreman from job
                            job.Foremen.Remove(foreman);
                        }
                        catch(Exception ex)
                        {
                            return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                        }
                    }
                }
                //save changes to the DB
                try
                {
                    db.Entry(job).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                }
                //if successful, redirect to job details page
                return RedirectToAction("Details", new { id = job.JobID });
            }
            return View(model);
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
            if (job == null)
            {
                return HttpNotFound();
            }
            //istantiate new JobViewModel
            JobViewModel model = new JobViewModel();
            model.JobNumber = job.JobNumber;
            model.JobName = job.JobName;
            model.LocationName = job.LocationName;
            model.Address = job.Address;
            model.City = job.City;
            model.ProvinceOrState = job.ProvinceOrState;
            model.GenContractorContact = job.GenContractorContact;
            model.GenContractor_GenContractorID = job.GenContractor_GenContractorID;
            model.PM_PMID = job.PM_PMID;
            model.Purchaser_PurchaserID = job.Purchaser_PurchaserID;
            model.SiteSuper_SiteSuperID = job.SiteSuper_SiteSuperID;

            //retrieve list of foremen related to job, so they can be removed prior to deleting job
            var jobForemen = db.Foremen.Where(i => i.Jobs.Any(j => j.JobID.Equals(job.JobID))).ToList();
            if (jobForemen != null)
            {
                //loop through foremen and remove them
                foreach (var foreman in jobForemen)
                {
                    try
                    {
                        //remove foreman from job
                        job.Foremen.Remove(foreman);
                    }
                    catch (Exception ex)
                    {
                        return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                    }
                }
                //save changes to the DB
                try
                {
                    db.Entry(job).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                }
            }
            //finally, remove the job from the db
            db.Jobs.Remove(job);
            await db.SaveChangesAsync();
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " deleted " + job.JobName + " " + job.JobID, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

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

            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the jobs table to .csv", User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));

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

            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the jobs table to excel", User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));

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
