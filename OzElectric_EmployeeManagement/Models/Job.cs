namespace OzElectric_EmployeeManagement.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Job
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Job()
        {
            HourRecords = new HashSet<HourRecord>();
            this.Foremen = new List<Foreman>();
            this.PMs = new List<PM>();
        }

        public int JobID { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Not a valid Job Number.")]
        public string JobNumber { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid Job Name.")]
        public string JobName { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid Name.")]
        public string LocationName { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^([0-9]{1,6})[ ]([a-zA-Z\s]{1,40})$", ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid City.")]
        public string City { get; set; }

        public string ProvinceOrState { get; set; }

        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Not a valid Name.")]
        public string GenContractorContact { get; set; }

        public int? GenContractor_GenContractorID { get; set; }

        public int? Purchaser_PurchaserID { get; set; }

        public int? SiteSuper_SiteSuperID { get; set; }

        public virtual Foreman Foreman { get; set; }

        public virtual GenContractor GenContractor { get; set; }

        public virtual PM PM { get; set; }

        public virtual Purchaser Purchaser { get; set; }

        public virtual SiteSuper SiteSuper { get; set; }
        public virtual ICollection<Foreman> Foremen { get; set; }
        public virtual ICollection<PM> PMs { get; set; }
        public virtual ICollection<HourRecord> HourRecords { get; set; }
    }
}
