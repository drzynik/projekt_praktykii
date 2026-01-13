using Microsoft.EntityFrameworkCore;

namespace praktyki_backend.Models
{
    public class dbcontext : DbContext
    {
        public dbcontext(DbContextOptions<dbcontext> options) : base(options)
        {
            // Włączenie FK przy każdym połączeniu
            Database.OpenConnection(); // otwiera połączenie
            Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;");
        }

        public DbSet<Cards> Cards { get; set; }
        public DbSet<Cardenablers> Cardenablers { get; set; }
        public DbSet<Cardweights> Cardweights { get; set; }
        public DbSet<Decisions> Decisions { get; set; }
        public DbSet<Decks> Decks { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        public DbSet<Gameevents> Gameevents { get; set; }
        public DbSet<Hardwares> Hardwares { get; set; }
        public DbSet<Processes> Processes { get; set; }
        public DbSet<Softwares> Softwares { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<GamemasterRequest> GamemasterRequests { get; set; }
    }
}
