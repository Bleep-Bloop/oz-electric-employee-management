using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace OzElectric_EmployeeManagement.Models
{
    public class JobViewModel
    {
        public JobViewModel()
        {
            Foremen = new MultiSelectList(new List<Foreman>());
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

        public int? PM_PMID { get; set; }

        public int? Purchaser_PurchaserID { get; set; }

        public int? SiteSuper_SiteSuperID { get; set; }
        public List<string> ForemanIDs { get; set; }

        [Display(Name = "Foremen")]
        public MultiSelectList Foremen { get; set; }
    }
}