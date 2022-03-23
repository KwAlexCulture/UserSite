using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BOL;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL
{
    public class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext(string ConnectionStrings)
             : base(ConnectionStrings)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

        #region Tables      
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentInfo> StudentInfoes { get; set; }
        public DbSet<College> Colleges { get; set; } 
        public DbSet<University> Universities { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<AnnualAllowancesExchange> AnnualAllowancesExchanges { get; set; }
        public DbSet<TicketExchange> TicketExchanges { get; set; }
        public DbSet<ClinicalAllowance> ClinicalAllowances { get; set; }
        public DbSet<CertifiedRecruitment> CertifiedRecruitments { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<IdentityUser> IdentityUsers { get; set; }
        #endregion

        #region Views  
        public DbSet<StudentView> StudentViews { get; set; }
        #endregion

        #region Model Builder
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region  Many-to-Many 

            #endregion

            #region Mapping Tables with odd names
            //builder.Entity<IdentityUser>().ToTable("Users");

            #endregion
        }
        #endregion
    }
}
