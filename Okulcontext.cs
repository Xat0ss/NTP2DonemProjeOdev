using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace denemeodev
{
    public class OkulContext : DbContext
    {
        public DbSet<Ogrenci> Ogrenciler { get; set; }
        public DbSet<Sinif> Siniflar { get; set; }
        public DbSet<Ders> Dersler { get; set; }
        public DbSet<OgrenciDers> OgrenciDersleri { get; set; }

       protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-GI6P2JA\\SQLEXPRESS;Database=OkulTaskDB;Trusted_Connection=True; TrustServerCertificate=True");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Ogrenci -> Sinif İlişkisi (Bir sınıf, birden fazla öğrenciye sahip olabilir)
            modelBuilder.Entity<Ogrenci>()
                .HasOne(o => o.Sinif)
                .WithMany(s => s.Ogrenciler)
                .HasForeignKey(o => o.SinifId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade silme engellendi

            // OgrenciDers -> Composite Key (Birleşik Anahtar)
            modelBuilder.Entity<OgrenciDers>()
                .HasKey(od => new { od.OgrenciId, od.DersId });

            // Ogrenci -> OgrenciDers İlişkisi
            modelBuilder.Entity<OgrenciDers>()
                .HasOne(od => od.Ogrenci)
                .WithMany(o => o.OgrenciDersleri)
                .HasForeignKey(od => od.OgrenciId);

            // Ders -> OgrenciDers İlişkisi
            modelBuilder.Entity<OgrenciDers>()
                .HasOne(od => od.Ders)
                .WithMany(d => d.OgrenciDersleri)
                .HasForeignKey(od => od.DersId);

            // Seed Data (Varsayılan Sınıflar)
            modelBuilder.Entity<Sinif>().HasData(
                new Sinif { SinifId = 1, Adi = "1. Sınıf", Kapasite = 10 },
                new Sinif { SinifId = 2, Adi = "2. Sınıf", Kapasite = 10 },
                new Sinif { SinifId = 3, Adi = "3. Sınıf", Kapasite = 10 }
            );

            // Ders Tablosu Varsayılan Verileri
            modelBuilder.Entity<Ders>().HasData(
                new Ders { DersId = 1, Baslik = "İnternet Programcılığı" },
                new Ders { DersId = 2, Baslik = "Görsel Programlama" },
                new Ders { DersId = 3, Baslik = "Nesne Programlama" },
                new Ders { DersId = 4, Baslik = "İş Sağlığı ve Güvenliği" },
                new Ders { DersId = 5, Baslik = "Veri Tabanı" },
                new Ders { DersId = 6, Baslik = "Yapay Zeka" },
                new Ders { DersId = 7, Baslik = "Adli Bilişim" },
                new Ders { DersId = 8, Baslik = "Zaman Yönetimi" },
                new Ders { DersId = 9, Baslik = "ERP" },
                new Ders { DersId = 10, Baslik = "Donanım" }
            );
        }
    }
}
