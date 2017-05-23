namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HourRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HourRecordID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateTime { get; set; }
        
        public double Hours { get; set; }

        public int Employee { get; set; }

        public int Job { get; set; }
    }
}
