using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Web.UI;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data;

namespace OzElectric_EmployeeManagement.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }//End ActionResult Index()



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
        }//End GetData()


        public DataTable callThing(SqlCommand cmd)
        {

            DataTable dt = GetData(cmd);
            return null; //careful

        }

        public string GetAllEmployeeHours()
        {

            //get a variable for which name to look for

            string strQuery = "select  Hours" +
                             " from HourRecords where Comment = 'null' ";

           // string queryResult;

            SqlCommand cmd = new SqlCommand(strQuery);
            // DataTable dt = GetData(cmd);

            DataTable dt = callThing(cmd);


            Console.WriteLine("under");
            Console.WriteLine("under");
            Console.WriteLine("under");
            Console.WriteLine(cmd);
            Console.WriteLine("above");
            Console.WriteLine("above");
            Console.WriteLine("above");
            return cmd.ToString();
        }






    }//End Public Class InvoiceController
}//End Namespace OzElectric_EmployeeManagement.Controllers