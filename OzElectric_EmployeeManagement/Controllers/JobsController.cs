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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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
        [Authorize(Roles = "Admin, Accounting")]
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
            MultiSelectList pmList = new MultiSelectList(db.PMs.ToList().OrderBy(i => i.FirstName), "PMID", "FullName");

            JobViewModel model = new JobViewModel { Foremen = foremenList, PMs = pmList };

            //populates dropdowns for foreign key fields
            ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name");
            ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name");
            ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name");

            return View(model);
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "JobID,JobNumber,JobName,LocationName,Address,City,ProvinceOrState,GenContractorContact,GenContractor_GenContractorID,Purchaser_PurchaserID,SiteSuper_SiteSuperID,Foremen,ForemanIDs,PMs,PMIDs")] JobViewModel model)
        {
            if (ModelState.IsValid)
            {
                //string used for log file
                string foremenLogFile = "";
                string pmsLogFile = "";

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
                //if there are pms selected, create new relationships between the new job and the pms
                if (model.PMIDs != null)
                {
                    foreach (var ID in model.PMIDs)
                    {
                        var pm = db.PMs.Find(int.Parse(ID));
                        //adding the PM to the log file
                        pmsLogFile = pmsLogFile + pm.FirstName + " " + pm.LastName;
                        try
                        {
                            job.PMs.Add(pm);
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
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " created: Job ID: " + job.JobID + " Job Number: " + job.JobNumber + " Job Name: " + job.JobName + " Job Location: " + job.LocationName + " Job Address: " + job.Address + " City: " + job.City + " Province/State: " + job.ProvinceOrState + " Gen Contractor Contact: " + job.GenContractorContact + " Foreman Name: " + foremenLogFile + " Gen Contractor Id: " + job.GenContractor_GenContractorID + " Project Managers: " + pmsLogFile + " Purchaser ID: " + job.Purchaser_PurchaserID + " Site Super ID: " + job.SiteSuper_SiteSuperID, User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }
            //else something was wrong (like a field incorrectly filled out) return to create with errors
            else
            {
                //fills dropdowns/select list with data previously entered before attempting submit.
                MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName", model.ForemanIDs);
                MultiSelectList pmsList = new MultiSelectList(db.PMs.ToList().OrderBy(i => i.FirstName), "PMID", "FullName", model.PMIDs);
                model.Foremen = foremenList;
                model.PMs = pmsList;

                ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", model.GenContractor_GenContractorID);
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
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " attempting to edit. Previous values: Job ID: " + job.JobID + " Job Number: " + job.JobNumber + " Job Name: " + job.JobName + " Job Location: " + job.LocationName + " Job Address: " + job.Address + " City: " + job.City + " Province/State: " + job.ProvinceOrState + " Gen Contractor Contact: " + job.GenContractorContact + " Gen Contractor Id: " + job.GenContractor_GenContractorID + " Purchaser ID: " + job.Purchaser_PurchaserID + " Site Super ID: " + job.SiteSuper_SiteSuperID, User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));
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
                Purchaser_PurchaserID = job.Purchaser_PurchaserID,
                SiteSuper_SiteSuperID = job.SiteSuper_SiteSuperID
            };
            //retrieve list of foremen and pms related to job
            var jobForemen = db.Foremen.Where(i => i.Jobs.Any(j => j.JobID.Equals(job.JobID))).ToList();
            var jobPMs = db.PMs.Where(i => i.Jobs.Any(j => j.JobID.Equals(job.JobID))).ToList();

            if (jobForemen != null || jobPMs != null)
            {
                if (jobForemen != null)
                {
                    //init array to number of foremen related to job
                    string[] jobFormenIDs = new string[jobForemen.Count];
                    //set value of jobForemen.Count so the for loop doesn't need to work it out every iteration
                    int length = jobForemen.Count;
                    //loop through each foreman, store each ID in array
                    for (int i = 0; i < length; i++)
                    {
                        jobFormenIDs[i] = jobForemen[i].ForemanID.ToString();
                    }
                    //instantiate MultiSelectList, selecting jobForemenIDs array
                    MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName", jobFormenIDs);
                    //populates dropdowns for foreign key fields
                    ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", model.GenContractor_GenContractorID);
                    ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", model.Purchaser_PurchaserID);
                    ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", model.SiteSuper_SiteSuperID);
                    //add foremenList to the Foremen property of the view model
                    model.Foremen = foremenList;
                }
                else
                {
                    //else instantiate MultiSelectList without any preselected values
                    MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName");
                    //add foremenList to the Foremen property of the view model
                    model.Foremen = foremenList;
                }

                if(jobPMs != null)
                {
                    //init array to number of PMs related to job
                    string[] jobPMsIDs = new string[jobPMs.Count];
                    //set value of jobPMs.Count so the for loop doesn't need to work it out every iteration
                    int length = jobPMs.Count;
                    //loop through each pm, store each ID in array
                    for (int i = 0; i < length; i++)
                    {
                        jobPMsIDs[i] = jobPMs[i].PMID.ToString();
                    }
                    //instantiate MultiSelectList, selecting jobPMsIDs array
                    MultiSelectList pmsList = new MultiSelectList(db.PMs.ToList().OrderBy(i => i.FirstName), "PMID", "FullName", jobPMsIDs);
                    //populates dropdowns for foreign key fields
                    ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name", model.GenContractor_GenContractorID);
                    ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name", model.Purchaser_PurchaserID);
                    ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name", model.SiteSuper_SiteSuperID);
                    //add pmsList to the PMs property of the view model
                    model.PMs = pmsList;
                }
                else
                {
                    //else instantiate MultiSelectList without any preselected values
                    MultiSelectList pmsList = new MultiSelectList(db.PMs.ToList().OrderBy(i => i.FirstName), "PMID", "FullName");
                    //add pmsList to the PMs property of the view model
                    model.PMs = pmsList;
                }
                //return viewmodel
                return View(model);
            }
            else
            {
                //else instantiate MultiSelectList without any preselected values
                MultiSelectList foremenList = new MultiSelectList(db.Foremen.ToList().OrderBy(i => i.FirstName), "ForemanID", "FullName");
                MultiSelectList pmsList = new MultiSelectList(db.PMs.ToList().OrderBy(i => i.FirstName), "PMID", "FullName");
                //add foremenList and pmsList to the view model
                model.Foremen = foremenList;
                model.PMs = pmsList;
                //populates dropdowns for foreign key fields
                ViewBag.GenContractor_GenContractorID = new SelectList(db.GenContractors, "GenContractorID", "Name");
                ViewBag.Purchaser_PurchaserID = new SelectList(db.Purchasers, "PurchaserID", "Name");
                ViewBag.SiteSuper_SiteSuperID = new SelectList(db.SiteSupers, "SiteSuperID", "Name");
                
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
        public async Task<ActionResult> Edit([Bind(Include = "JobID,JobNumber,JobName,LocationName,Address,City,ProvinceOrState,GenContractorContact,GenContractor_GenContractorID,Purchaser_PurchaserID,SiteSuper_SiteSuperID,Foremen,ForemanIDs,PMs,PMIDs")] JobViewModel model)
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
                    //loop through foremen and remove them
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
                //check if any pms were selected by user in the form
                if (model.PMIDs.Count > 0)
                {
                    //instantiate list to store each of the pms in the viewmodel for later comparison
                    List<PM> viewModelPMs = new List<PM>();
                    //loop through each pm ID
                    foreach (var ID in model.PMIDs)
                    {
                        //Retrieve pm from DB
                        var pm = db.PMs.Find(int.Parse(ID));
                        if (pm != null)
                        {
                            //try to add pm to tracking list of viewModelPMs and job pms
                            try
                            {
                                job.PMs.Add(pm);
                                viewModelPMs.Add(pm);
                            }
                            catch (Exception ex)
                            {
                                return View("Error", new HandleErrorInfo(ex, "Jobs", "Index"));
                            }
                        }
                    }
                    //create list of pms from DB
                    var allPMs = db.PMs.ToList();
                    //exculude viewModelPMs from allPMs list, this creates list of pms that need to be removed from the job.
                    var pmsToRemove = allPMs.Except(viewModelPMs);
                    //loop through pms and remove them
                    foreach (var pm in pmsToRemove)
                    {
                        try
                        {
                            //remove pm from job
                            job.PMs.Remove(pm);
                        }
                        catch (Exception ex)
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

            //retrieve list of pms related to job, so they can be removed prior to deleting job
            var jobPMs = db.PMs.Where(i => i.Jobs.Any(j => j.JobID.Equals(job.JobID))).ToList();
            if (jobPMs != null)
            {
                //loop through pms and remove them
                foreach (var pm in jobPMs)
                {
                    try
                    {
                        //remove pm from job
                        job.PMs.Remove(pm);
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
            string strQuery = "select JobNumber, JobName, LocationName, Address, City, ProvinceOrState, GenContractorContact, GenContractor_GenContractorID, Purchaser_PurchaserID, SiteSuper_SiteSuperID" +
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
            string strQuery = "select JobNumber, JobName, LocationName, Address, City, ProvinceOrState, GenContractorContact, GenContractor_GenContractorID, Purchaser_PurchaserID, SiteSuper_SiteSuperID" +
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





        //Opens the window for the invoice query search        
        public ActionResult JobHourRecordQuery(int wantedJobID)
        {

            //Populates job dropdown from jobs table
            ViewBag.Job_JobID = new SelectList(db.Jobs, "JobID", "JobName");
            ViewBag.Employee_EmployeeID = new SelectList(db.Employees, "EmployeeID", "EmployeeNumber");
            TempData["chosenJob"] = wantedJobID;

            Debug.WriteLine("CHOSEN JOB ID: " + wantedJobID);
            Debug.WriteLine("CHOSEN JOB ID to STRING: " + wantedJobID.ToString());
            return View();


        }


        //called when submitting query page
        public async Task<ActionResult> JobInvoice(int? wantedJobID, DateTime? startDate, DateTime? endDate)//, int Job_JobID)
        {

            string incomingJobID = TempData["chosenJob"].ToString();

            //string Job_JobID = TempData["chosenJob"].ToString();

            Debug.WriteLine("Incoming Job ID: " + incomingJobID);
            //Debug.WriteLine("JOB_JOBID = " + Job_JobID);

            //incase user backspaces
            TempData["chosenJob"] = incomingJobID;


            Debug.WriteLine("JOB NUMBER: " + incomingJobID);


            ApplicationDbContext context = new ApplicationDbContext();
            //used to see who is currently logged in
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            var hourTracker = from h in db.HourRecords.Include(h => h.Job).Include(h => h.Employee)
                              select h;


            //Fix this just leaving as reminder (35 job id of job all untill adding list items work)
            
                hourTracker = from h in db.HourRecords.Include(h => h.Job)
                              where h.Job_JobID.ToString() == incomingJobID  && (h.DateTime >= startDate && h.DateTime <= endDate) && h.Job_JobID.ToString() == incomingJobID
                              select h;
            

            return View(await hourTracker.ToListAsync());

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
