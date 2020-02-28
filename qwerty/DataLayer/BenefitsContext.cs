using DataLayer.Entities;
using DataLayer.Entities.EGISSO;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class BenefitsContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleUserLink> RoleUserLink { get; set; }
        public DbSet<PersonInfo> PersonInfos { get; set; }
        public DbSet<WorkInfo> WorkInfos { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<PostInfoLog> PostInfoLogs { get; set; }
        public DbSet<GetInfoLog> GetInfoLogs { get; set; }
        public DbSet<PersonBankCard> PersonBankCards { get; set; }
        public DbSet<ExtraPayVariant> ExtraPayVariants { get; set; }
        public DbSet<ExtraPay> ExtraPays { get; set; }
        public DbSet<DsPerc> DsPercs { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<GosPensionUpdate> GosPensionUpdates { get; set; }
        public DbSet<Reestr> Reestrs { get; set; }
        public DbSet<ReestrElement> ReestrElements { get; set; }
        public DbSet<MinExtraPay> MinExtraPays { get; set; }
        public DbSet<PeriodType> PeriodTypes { get; set; }
        public DbSet<MeasureUnit> MeasureUnits { get; set; }
        public DbSet<ProvisionForm> ProvisionForms { get; set; }
        public DbSet<KpCode> KpCodes { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<KpCodeLink> KpCodeLinks { get; set; }
        public DbSet<EgissoFact> EgissoFacts { get; set; }
        public DbSet<RecountDebt> RecountDebts { get; set; }
        public DbSet<EgissoEjectionHistory> EgissoEjectionHistories { get; set; }
        public DbSet<PfrSnilsRequest> PfrSnilsRequests { get; set; }

        public DbSet<ZagsStopAct> ZagsStopActs { get; set; }



        public BenefitsContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleUserLink>()
                .HasKey(c => new { c.RoleId, c.UserId });

            modelBuilder.Entity<EgissoFact>()
                .HasKey(f => f.Id);
        }
    }
}
