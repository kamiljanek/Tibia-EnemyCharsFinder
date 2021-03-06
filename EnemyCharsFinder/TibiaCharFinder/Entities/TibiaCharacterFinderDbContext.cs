using Microsoft.EntityFrameworkCore;

namespace TibiaCharFinderAPI.Entities
{
    public class TibiaCharacterFinderDbContext : DbContext
    {
        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=TibiaCharFinderDb; Trusted_Connection=True";

        public DbSet<World> Worlds { get; set; }
        public DbSet<WorldScan> WorldScans { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<WorldCorrelation> WorldCorrelations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<World>(w =>
                {
                    w.Property(wo => wo.Name).IsRequired();
                    w.Property(wo => wo.Url).IsRequired();
                    w.Property(wo => wo.IsAvailable).IsRequired();
                    w.HasMany(ws => ws.WorldScans)
                        .WithOne(ws => ws.World)
                        .HasForeignKey(ws => ws.WorldId);
                });

            modelBuilder.Entity<WorldScan>(ws =>
            {
                ws.Property(w => w.CharactersOnline).IsRequired();
            });
            modelBuilder.Entity<WorldCorrelation>()
                .HasOne(m => m.LoginCharacter)
                .WithMany(t => t.LoginWorldCorrelations)
                .HasForeignKey(m => m.LoginCharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<WorldCorrelation>()
                .HasOne(m => m.LogoutCharacter)
                .WithMany(t => t.LogoutWorldCorrelations)
                .HasForeignKey(m => m.LogoutCharacterId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
