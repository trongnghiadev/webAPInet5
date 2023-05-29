using Microsoft.EntityFrameworkCore;


namespace HocHanh6day.Data
{
    

    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        public DbSet<HangHoa> HangHoas { get; set; }
        public DbSet<Loai> Loais { get; set; }
    }
}