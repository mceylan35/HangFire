using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HangFire.API.Context
{
    public class OtelContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=OtelRezervasyon; Trusted_Connection=true");
        }
        public DbSet<Oda> Oda { get; set; }
        public DbSet<Otel> Otel { get; set; }
        public DbSet<Kullanici> Kullanici { get; set; }
        public DbSet<Rezervasyon> Rezervasyon { get; set; }
        public DbSet<Role> Role { get; set; }
    }
    public class Kullanici : IEntity
    {
        public Kullanici()
        {
            Rezervasyon = new HashSet<Rezervasyon>();
        }

        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Mail { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Rezervasyon> Rezervasyon { get; set; }
    }
    public class Oda : IEntity
    {

        public Oda()
        {
            Rezervasyon = new HashSet<Rezervasyon>();
        }

        public int Id { get; set; }
        public int OdaNo { get; set; }
        public bool OdaDurumu { get; set; }
        public decimal Ucret { get; set; }
        public int KisiSayisi { get; set; }
        public string Aciklama { get; set; }

        public string Resim { get; set; }
        public int OtelId { get; set; }
        public virtual Otel Otel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rezervasyon> Rezervasyon { get; set; }
    }
    public class Otel : IEntity
    {
        public Otel()
        {
            Odalar = new List<Oda>();
        }
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Adres { get; set; }
        public int Puan { get; set; }
        public string Resim { get; set; }
        public virtual List<Oda> Odalar { get; set; }
    }
    public class RegisterUser
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string KullaniciAdi { get; set; }
    }
    public class Rezervasyon : IEntity
    {
        public int Id { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime CikisTarihi { get; set; }
        public decimal Fiyat { get; set; }
        public decimal ToplamFiyat { get; set; }
        public DateTime RezervasyonTarihi { get; set; }
        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public int OdaId { get; set; }
        public virtual Oda Oda { get; set; }
    }
    public class Role : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Role()
        {
            Kullanici = new HashSet<Kullanici>();
        }

        public int Id { get; set; }
        public string RoleAdi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kullanici> Kullanici { get; set; }
    }

    public interface IEntity
    {

    }
}
