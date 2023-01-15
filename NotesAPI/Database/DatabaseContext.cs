using Microsoft.EntityFrameworkCore;

namespace NotesAPI.Database
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Models.UserModel> Users { get; set; }
        public DbSet<Models.NotesModel> Notes { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }  
    }
}
