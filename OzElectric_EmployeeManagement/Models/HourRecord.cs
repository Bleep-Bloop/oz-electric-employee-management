namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    public partial class HourRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HourRecordID { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }

        [DataType(DataType.Text)]
        public string Comment { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9'.']{1,5})$", ErrorMessage = "Not a valid Hour Entry.")]
        public virtual double Hours { get; set; }

        [Display(Name = "Employee")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int? Employee_EmployeeID { get; set; }

        public virtual int? Job_JobID { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Job Job { get; set; }

        



    }
}
