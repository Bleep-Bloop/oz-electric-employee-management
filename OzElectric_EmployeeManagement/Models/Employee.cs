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
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Employee Number Required!")]
        public string EmployeeNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string ProvinceOrState { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?([0-9]{3}))?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string HomePhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?([0-9]{3}))?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string HomeCellPhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?([0-9]{3}))?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string WorkPhone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?([0-9]{3}))?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string WorkCellPhone { get; set; }

        public string EmergencyContactName { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?([0-9]{3}))?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string EmergencyContactPhone { get; set; }
        public virtual ICollection<HourRecord> HourRecords { get; set; }
    }
}
