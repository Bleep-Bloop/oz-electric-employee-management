using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace OzElectric_EmployeeManagement.Models
{
    public partial class ManagementContext : DbContext
    {
        public ManagementContext() : base("name=ManagementConnection")
        {

        }

        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Foreman> Foremans { get; set; }
        public virtual DbSet<GenContractor> GenContractors { get; set; }
        public virtual DbSet<SiteSuper> SiteSupers { get; set; }
        public virtual DbSet<PM> PMs { get; set; }
        public virtual DbSet<Purchaser> Purchasers { get; set; }
    }
}