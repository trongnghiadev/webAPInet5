using Microsoft.EntityFrameworkCore;


namespace HocHanh6day.Data
{
    

    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        public DbSet<HangHoa> HangHoas { get; set; }
        public DbSet<Loai> Loais { get; set; }

        public DbSet<DonHang> DonHangs { get; set; }

        public DbSet<DonHangChiTiet> DonHangChiTiets { get; set; }

        public DbSet<NguoiDung> nguoiDungs { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DonHang>(e =>
            {
                e.ToTable("DonHang");
                e.HasKey(dh => dh.MaDh);
                e.Property(dh => dh.NgayDat).HasDefaultValueSql("getutcdate()");
                e.Property(dh => dh.NguoiNhan).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<DonHangChiTiet>(e =>
            {
                e.ToTable("ChiTietDonHang");
                e.HasKey(e => new
                {
                    e.MaDh,
                    e.MaHh
                });

                e.HasOne(e => e.DonHang)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaDh)
                    .HasConstraintName("FK_DonHangChiTiet_DonHang");

                e.HasOne(e => e.HangHoa)
                    .WithMany(e => e.DonHangChiTiets)
                    .HasForeignKey(e => e.MaHh)
                    .HasConstraintName("FK_DonHangChiTiet_HangHoa");

            });

            modelBuilder.Entity<NguoiDung>(e =>
            {
                e.HasIndex(e => e.UserName).IsUnique();
                e.Property(e => e.HoTen).IsRequired().HasMaxLength(150);
                e.Property(e => e.Email).IsRequired().HasMaxLength(150);
            });
        }
    }
}