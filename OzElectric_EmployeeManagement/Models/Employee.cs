namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        public int EmployeeID { get; set; }

        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ProvinceOrState { get; set; }

        public string HomePhone { get; set; }

        public string HomeCellPhone { get; set; }

        public string WorkPhone { get; set; }

        public string WorkCellPhone { get; set; }

        public string EmergencyContactName { get; set; }

        public string EmergencyContactPhone { get; set; }
    }
}
