namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMsToJobs
    {
        [Key]
        public virtual int PMsToJobID { get; set; }
        public virtual int? PMID { get; set; }
        public virtual int? JobID { get; set; }

        public virtual PM PM { get; set; }
        public virtual Job Job { get; set; }
    }
}