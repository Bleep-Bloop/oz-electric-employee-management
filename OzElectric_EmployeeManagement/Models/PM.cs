namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PM()
        {
            this.Jobs = new List<Job>();
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public int PMID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-]([0-9]{3})[-]([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        public string WorkPhone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
