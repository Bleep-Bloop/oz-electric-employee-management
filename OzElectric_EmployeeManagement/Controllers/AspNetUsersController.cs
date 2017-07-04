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

//added for logging
using log4net;

namespace OzElectric_EmployeeManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AspNetUsersController : Controller
    {

        public ILog logger = log4net.LogManager.GetLogger(typeof(AspNetUsersController));
        
     

        private ManagementContext db = new ManagementContext();

        // GET: AspNetUsers
        public async Task<ActionResult> Index()
        {
            return View(await db.AspNetUsers.ToListAsync());
        }

        // GET: AspNetUsers
        public async Task<ActionResult> Customize()
        {
            return View(await db.AspNetUsers.ToListAsync());
        }

        // GET: AspNetUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            return View(aspNetUser);
        }
        
        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                await db.SaveChangesAsync();
                //Log user being created (with username and Email)
                logger.Info(User.Identity.Name + " created " + aspNetUser.UserName + " " + aspNetUser.Email + " " );
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " created " + aspNetUser.UserName + " " + aspNetUser.Email + " ", User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));

                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            //Grabbing and the logging the pre-change edit values
            logger.Info(User.Identity.Name + "Attempting to edit. Previous Values: " + "ID: " + aspNetUser.Id + "Email: " + aspNetUser.Email + "Email Confirmed: " + aspNetUser.EmailConfirmed + "Security Stamp: " + aspNetUser.SecurityStamp + "Phone Number: " + aspNetUser.PhoneNumber + "Phone Number Confirmed: " + aspNetUser.PhoneNumberConfirmed + "Two Factor Enabled: " + aspNetUser.TwoFactorEnabled + "Lockout End Date: " + aspNetUser.LockoutEndDateUtc + "Lockout Enabled: " + aspNetUser.LockoutEnabled + "Access Failed Count: " + aspNetUser.AccessFailedCount + "Username: " + aspNetUser.UserName);
            AccountController.dynamicLogRecord(User.Identity.Name + "Attempting to edit. Previous Values: " + "ID: " + aspNetUser.Id + "Email: " + aspNetUser.Email + "Email Confirmed: " + aspNetUser.EmailConfirmed + "Security Stamp: " + aspNetUser.SecurityStamp + "Phone Number: " + aspNetUser.PhoneNumber + "Phone Number Confirmed: " + aspNetUser.PhoneNumberConfirmed + "Two Factor Enabled: " + aspNetUser.TwoFactorEnabled + "Lockout End Date: " + aspNetUser.LockoutEndDateUtc + "Lockout Enabled: " + aspNetUser.LockoutEnabled + "Access Failed Count: " + aspNetUser.AccessFailedCount + "Username: " + aspNetUser.UserName, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));



            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //Grabbing and logging the post-change edit values //DOUBLE CHECK WHAT CAN BE EDITED
                logger.Info(User.Identity.Name + "Finished editing. Values: " + "ID: " + aspNetUser.Id + "Email: " + aspNetUser.Email + "Email Confirmed: " + aspNetUser.EmailConfirmed + "Security Stamp: " + aspNetUser.SecurityStamp + "Phone Number: " + aspNetUser.PhoneNumber + "Phone Number Confirmed: " + aspNetUser.PhoneNumberConfirmed + "Two Factor Enabled: " + aspNetUser.TwoFactorEnabled + "Lockout End Date: " + aspNetUser.LockoutEndDateUtc + "Lockout Enabled: " + aspNetUser.LockoutEnabled + "Access Failed Count: " + aspNetUser.AccessFailedCount + "Username: " + aspNetUser.UserName);
                AccountController.dynamicLogRecord(User.Identity.Name + "Finished editing. Values: " + "ID: " + aspNetUser.Id + "Email: " + aspNetUser.Email + "Email Confirmed: " + aspNetUser.EmailConfirmed + "Security Stamp: " + aspNetUser.SecurityStamp + "Phone Number: " + aspNetUser.PhoneNumber + "Phone Number Confirmed: " + aspNetUser.PhoneNumberConfirmed + "Two Factor Enabled: " + aspNetUser.TwoFactorEnabled + "Lockout End Date: " + aspNetUser.LockoutEndDateUtc + "Lockout Enabled: " + aspNetUser.LockoutEnabled + "Access Failed Count: " + aspNetUser.AccessFailedCount + "Username: " + aspNetUser.UserName, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));



                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }

            return View(aspNetUser);
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

        //Confirm query with Kevin
        [Authorize(Roles = "Admin")]
        public ActionResult ExportToCSV(object sender, EventArgs e)
        {

            string strQuery = "select UserName, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount" +
                              " from AspNetUsers";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(cmd);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=UsersTable.csv");
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

            //Log user who exported to csv
            logger.Info(User.Identity.Name + " exported the employee table to csv ");
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the employee table to .csv ", User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

            return View();
        }

        //Confirm query with Kevin
        [Authorize(Roles = "Admin")]
        public ActionResult ExportToExcel(object sender, EventArgs e)
        {
            string strQuery = "select UserName, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount" +
                              " from AspNetUsers";
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
             "attachment;filename=UsersTable.xls");
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

            //Log user who exported to excel
            logger.Info(User.Identity.Name + " exported the employee table to excel ");
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the employee table to excel ", User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));



            return View();

        }


        
        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
            
            //Logging user deleting and the victim
            logger.Info(User.Identity.Name + " deleted " + aspNetUser.UserName);
            AccountController.dynamicLogRecord(User.Identity.Name + " deleted " + aspNetUser.UserName, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));



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
