namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
