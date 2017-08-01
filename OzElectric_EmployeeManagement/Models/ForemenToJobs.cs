namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ForemenToJobs
    {
        [Key]
        public virtual int ForemenToJobID { get; set; }
        public virtual int? ForemanID { get; set; }
        public virtual int? JobID { get; set; }

        public virtual Foreman Foreman { get; set; }
        public virtual Job Job { get; set; }
    }
}