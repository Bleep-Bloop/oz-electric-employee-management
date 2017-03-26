namespace OzElectric_EmployeeManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ManagementContext : DbContext
    {
        public ManagementContext()
            : base("name=ManagementContext")
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Foreman> Foremen { get; set; }
        public virtual DbSet<GenContractor> GenContractors { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<PM> PMs { get; set; }
        public virtual DbSet<Purchaser> Purchasers { get; set; }
        public virtual DbSet<SiteSuper> SiteSupers { get; set; }

        /*
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ManagementContext>(null);

            modelBuilder.Entity<Foreman>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.Foreman)
                .HasForeignKey(e => e.Foreman_ForemanID);

            modelBuilder.Entity<GenContractor>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.GenContractor)
                .HasForeignKey(e => e.GenContractor_GenContractorID);

            modelBuilder.Entity<PM>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.PM)
                .HasForeignKey(e => e.PM_PMID);

            modelBuilder.Entity<Purchaser>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.Purchaser)
                .HasForeignKey(e => e.Purchaser_PurchaserID);

            modelBuilder.Entity<SiteSuper>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.SiteSuper)
                .HasForeignKey(e => e.SiteSuper_SiteSuperID);
        }*/
    }
}
