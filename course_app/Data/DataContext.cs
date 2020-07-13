using course_app.Models;
using Microsoft.EntityFrameworkCore;

namespace course_app.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


    }
}
