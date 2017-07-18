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
    [Authorize(Roles = "Admin")]
    public class PurchasersController : Controller
    {
        private ManagementContext db = new ManagementContext();

        // GET: Purchasers
        public async Task<ActionResult> Index(string sortOrder)
        {
            ViewBag.PurchaserSortParm = String.IsNullOrEmpty(sortOrder) ? "Purchaser_desc" : "";

            var purchaser = from p in db.Purchasers
                            select p;

            switch (sortOrder)
            {
                case "Purchaser_desc":
                    purchaser = purchaser.OrderByDescending(p => p.Name);
                    break;
                default:
                    purchaser = purchaser.OrderBy(p => p.Name);
                    break;

            }
            return View(await purchaser.ToListAsync());
        }

        // GET: Purchasers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return HttpNotFound();
            }
            return View(purchaser);
        }

        // GET: Purchasers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Purchasers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PurchaserID,Name")] Purchaser purchaser)
        {
            if (ModelState.IsValid)
            {
                db.Purchasers.Add(purchaser);
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " created: " + " Purchaser ID: " + purchaser.PurchaserID + " Name " + purchaser.Name, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

                return RedirectToAction("Index");
            }

            return View(purchaser);
        }

        // GET: Purchasers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return HttpNotFound();
            }
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " is attempting to edit. Previous values: " + " Purchaser ID: " + purchaser.PurchaserID + " Name " + purchaser.Name, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

            return View(purchaser);
        }

        // POST: Purchasers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PurchaserID,Name")] Purchaser purchaser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchaser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " finished editing. New values: " + " Purchaser ID: " + purchaser.PurchaserID + " Name " + purchaser.Name, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

                return RedirectToAction("Index");
            }
            return View(purchaser);
        }

        // GET: Purchasers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchaser purchaser = await db.Purchasers.FindAsync(id);
            if (purchaser == null)
            {
                return HttpNotFound();
            }
            return View(purchaser);
        }

        // POST: Purchasers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Purchaser purchaser = await db.Purchasers.FindAsync(id);
                db.Purchasers.Remove(purchaser);
                await db.SaveChangesAsync();
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " deleted " + purchaser.Name + " " + purchaser.PurchaserID, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Response.Write("<script language='javascript'>alert(" + e.Message + ")</script>");
                AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " encountered error when attempting delete " + " " + e, User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
        public ActionResult ExportToWord()
        {

            //Get the data from database into datatable
            string strQuery = "select Name, PurchaserID" +
                              " from Purchasers";
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
                "attachment;filename=PurchaserTable.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            GridView1.RenderControl(hw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            //Log user who exported
            //Temp Shutdown logger.Info(User.Identity.Name + " exported the employee table to word ");
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the purchasers table to word", User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));

            return View();

        }

        [Authorize(Roles = "Admin")]
        public ActionResult ExportToCSV(object sender, EventArgs e)
        {

            string strQuery = "select Name, PurchaserID" +
                              " from Purchasers";
            SqlCommand cmd = new SqlCommand(strQuery);
            DataTable dt = GetData(cmd);

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
                "attachment;filename=PurchaserTable.csv");
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

            //logging user who exported to csv to employee table log
            //Temp Shutdown logger.Info(User.Identity.Name + " exported the employee table to .csv");

            //logging user who exported to csv to their log
            //AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the employee table to .csv", User.Identity.Name.ToString());

            //AccountController.setDynamicLog(User.Identity.Name.ToString())
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the purchasers table to .csv ", User.Identity.Name.ToString(), AccountController.setDynamicLog(User.Identity.Name));
            return View();
        }



        [Authorize(Roles = "Admin")]
        public ActionResult ExportToExcel(object sender, EventArgs e)
        {
            string strQuery = "select Name, PurchaserID" +
                               " from Purchasers";
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
             "attachment;filename=PurchaserTable.xls");
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

            //logging user who exported to excel
            //Temp Shutdown logger.Info(User.Identity.Name + " exported the employee table to excel");
            AccountController.dynamicLogRecord(User.Identity.Name.ToString() + " exported the purchasers table to excel", User.Identity.Name, AccountController.setDynamicLog(User.Identity.Name));

            return View();


        }







    }
}
