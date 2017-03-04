namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("Jobs")]
    public partial class GenContractor
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        //public Job()
        //{
        //    Enrollments = new HashSet<Enrollment>();
        //}

            /*
             INFORMATION

             */

        [Key]
        public int GenContractorID { get; set; }

        //[Required]
        //[StringLength(100)]
        public string Name { get; set; }

        public Array Contacts { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
