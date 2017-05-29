namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HourRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HourRecordID { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

        public virtual double Hours { get; set; }
        public virtual int? Employee_EmployeeID { get; set; }
        public virtual int? Job_JobID { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Job Job { get; set; }
    }
}
