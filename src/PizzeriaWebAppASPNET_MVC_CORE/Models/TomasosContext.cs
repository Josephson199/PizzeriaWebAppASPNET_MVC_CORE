using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public partial class TomasosContext : DbContext
    {
        public TomasosContext(DbContextOptions<TomasosContext> options)
            : base(options)
        { }
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Bestallning> Bestallning { get; set; }
        public virtual DbSet<BestallningMatratt> BestallningMatratt { get; set; }
        public virtual DbSet<Kund> Kund { get; set; }
        public virtual DbSet<Matratt> Matratt { get; set; }
        public virtual DbSet<MatrattProdukt> MatrattProdukt { get; set; }
        public virtual DbSet<MatrattTyp> MatrattTyp { get; set; }
        public virtual DbSet<Produkt> Produkt { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //    optionsBuilder.UseSqlServer(@"Server=LAPTOP-8HO4BK3G\SQLEXPRESS;Database=Tomasos;Trusted_Connection=True;");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex");

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserLogins_UserId");

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.ProviderKey).HasMaxLength(450);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK_AspNetUserRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IX_AspNetUserRoles_RoleId");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_AspNetUserRoles_UserId");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.RoleId).HasMaxLength(450);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.Property(e => e.LoginProvider).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(450);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).HasMaxLength(450);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<Bestallning>(entity =>
            {
                entity.Property(e => e.BestallningId).HasColumnName("BestallningID");

                entity.Property(e => e.BestallningDatum).HasColumnType("datetime");

                entity.Property(e => e.KundId).HasColumnName("KundID");

                entity.HasOne(d => d.Kund)
                    .WithMany(p => p.Bestallning)
                    .HasForeignKey(d => d.KundId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Bestallning_Kund");
            });

            modelBuilder.Entity<BestallningMatratt>(entity =>
            {
                entity.HasKey(e => new { e.MatrattId, e.BestallningId })
                    .HasName("PK_BestallningMatratt");

                entity.Property(e => e.MatrattId).HasColumnName("MatrattID");

                entity.Property(e => e.BestallningId).HasColumnName("BestallningID");

                entity.Property(e => e.Antal).HasDefaultValueSql("1");

                entity.HasOne(d => d.Bestallning)
                    .WithMany(p => p.BestallningMatratt)
                    .HasForeignKey(d => d.BestallningId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BestallningMatratt_Bestallning");

                entity.HasOne(d => d.Matratt)
                    .WithMany(p => p.BestallningMatratt)
                    .HasForeignKey(d => d.MatrattId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_BestallningMatratt_Matratt");
            });

            modelBuilder.Entity<Kund>(entity =>
            {
                entity.Property(e => e.KundId).HasColumnName("KundID");

                entity.Property(e => e.Email).HasColumnType("varchar(50)");

                entity.Property(e => e.Gatuadress)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Namn)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Postnr)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(e => e.Postort)
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Telefon).HasColumnType("varchar(50)");

                entity.Property(e => e.UserId).HasMaxLength(450);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Kund)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserId_Kund");

                entity.Property(e => e.Points).HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Matratt>(entity =>
            {
                entity.Property(e => e.MatrattId).HasColumnName("MatrattID");

                entity.Property(e => e.Beskrivning).HasColumnType("varchar(200)");

                entity.Property(e => e.MatrattNamn)
                    .IsRequired()
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.MatrattTypNavigation)
                    .WithMany(p => p.Matratt)
                    .HasForeignKey(d => d.MatrattTyp)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Matratt_MatrattTyp");
            });

            modelBuilder.Entity<MatrattProdukt>(entity =>
            {
                entity.HasKey(e => new { e.MatrattId, e.ProduktId })
                    .HasName("PK_MatrattProdukt");

                entity.Property(e => e.MatrattId).HasColumnName("MatrattID");

                entity.Property(e => e.ProduktId).HasColumnName("ProduktID");

                entity.HasOne(d => d.Matratt)
                    .WithMany(p => p.MatrattProdukt)
                    .HasForeignKey(d => d.MatrattId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MatrattProdukt_Matratt");

                entity.HasOne(d => d.Produkt)
                    .WithMany(p => p.MatrattProdukt)
                    .HasForeignKey(d => d.ProduktId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_MatrattProdukt_Produkt");
            });

            modelBuilder.Entity<MatrattTyp>(entity =>
            {
                entity.HasKey(e => e.MatrattTyp1)
                    .HasName("PK_MatrattTyp");

                entity.Property(e => e.MatrattTyp1).HasColumnName("MatrattTyp");

                entity.Property(e => e.Beskrivning)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Produkt>(entity =>
            {
                entity.Property(e => e.ProduktId).HasColumnName("ProduktID");

                entity.Property(e => e.ProduktNamn)
                    .IsRequired()
                    .HasColumnType("varchar(50)");
            });
        }
    }
}