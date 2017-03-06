namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Job
    {
        public int JobID { get; set; }

        public string LocationName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Province { get; set; }

        public string GenContractorContact { get; set; }

        public int? Foreman_ForemanID { get; set; }

        public int? GenContractor_GenContractorID { get; set; }

        public int? PM_PMID { get; set; }

        public int? Purchaser_PurchaserID { get; set; }

        public int? SiteSuper_SiteSuperID { get; set; }

        public virtual Foreman Foreman { get; set; }

        public virtual GenContractor GenContractor { get; set; }

        public virtual PM PM { get; set; }

        public virtual Purchaser Purchaser { get; set; }

        public virtual SiteSuper SiteSuper { get; set; }
    }
}
