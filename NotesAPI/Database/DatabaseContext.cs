using Microsoft.EntityFrameworkCore;

namespace NotesAPI.Database
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Models.UserModel> Users { get; set; }
        public DbSet<Models.NotesModel> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { 
            modelBuilder.Entity<Models.UserModel>().HasIndex(b => b.Email).IsUnique();
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }  
    }
}
