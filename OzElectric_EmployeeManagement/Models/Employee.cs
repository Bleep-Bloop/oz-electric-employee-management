namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlClient;

    public partial class Employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            HourRecords = new HashSet<HourRecord>();
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        //For populating jobs drop down list in employee hour record query
        public void GetDropDownList()
        {

            //Pass your data base connection string here 
            //using (SqlConnection c = new SqlConnection(cString))
            String strConnString = "Data Source=patrickdatabase.database.windows.net;Initial Catalog=COMP2007DataBase_2017-05-30T01 -48Z;Integrated Security=False;User ID=patr9240;Password=OzzPassword123;Connect Timeout=15;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            SqlConnection con = new SqlConnection(strConnString);
            //Pass your SQL Query and above created SqlConnection object  "c"
            using (SqlCommand cmd = new SqlCommand("SELECT JobName FROM Jobs", con))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                        JobsDropDownList.Add(rdr["JobName"].ToString());
            
        }
                }
            }
        }//End GetDropDownList()

        //For populating jobs drop down list in employee hour record query
        public void GetDropDownListEmployee()
        {

            //Pass your data base connection string here 
            //using (SqlConnection c = new SqlConnection(cString))
            String strConnString = "Data Source=patrickdatabase.database.windows.net;Initial Catalog=COMP2007DataBase_2017-05-30T01 -48Z;Integrated Security=False;User ID=patr9240;Password=OzzPassword123;Connect Timeout=15;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


            SqlConnection con = new SqlConnection(strConnString);
            //Pass your SQL Query and above created SqlConnection object  "c"
            using (SqlCommand cmd = new SqlCommand("SELECT EmployeeID FROM Employees", con))
            {
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {

                       EmployeesDropDownList.Add(rdr["EmployeeName"].ToString());

                    }
                }
            }
        }//End GetDropDownListEmployee()




        public int EmployeeID { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Not a valid Employee Number.")]
        public string EmployeeNumber { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid Name.")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid Name.")]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{1,6})[ ]([a-zA-Z\s]{1,40})$", ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid City.")]
        public string City { get; set; }

        public string ProvinceOrState { get; set; }



        public List<string> JobsDropDownList = new List<string>();
        public List<string> EmployeesDropDownList = new List<string>();



        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string HomePhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string HomeCellPhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string WorkPhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string WorkCellPhone { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid Name.")]
        public string EmergencyContactName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string EmergencyContactPhone { get; set; }
        public virtual ICollection<HourRecord> HourRecords { get; set; }
    }
}
