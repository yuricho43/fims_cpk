using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FimsCPK.Models;

public partial class FimsDbContext : DbContext
{
    public FimsDbContext()
    {
    }

    public FimsDbContext(DbContextOptions<FimsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<Titem> Titems { get; set; }

    public virtual DbSet<Tsheet> Tsheets { get; set; }

    public virtual DbSet<TspecItem> TspecItems { get; set; }

    public virtual DbSet<TspecModel> TspecModels { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Get ConnectionString from appsettings.json
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnectionDevelop"));
    }
    /*
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    => optionsBuilder.UseSqlServer("Data Source=192.168.24.249;Initial Catalog=FimsDb;TrustServerCertificate=True;User Id=sa;Password=Fst23841!");
    */

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<Titem>(entity =>
        {
            entity.ToTable("TItems");

            entity.HasIndex(e => e.TsheetId, "IX_TItems_TSheetId");

            entity.Property(e => e.TsheetId).HasColumnName("TSheetId");

            entity.HasOne(d => d.Tsheet).WithMany(p => p.Titems).HasForeignKey(d => d.TsheetId);
        });

        modelBuilder.Entity<Tsheet>(entity =>
        {
            entity.ToTable("TSheets");

            entity.Property(e => e.ClosingEndDateTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
            entity.Property(e => e.ClosingStartDateTime).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");
        });

        modelBuilder.Entity<TspecItem>(entity =>
        {
            entity.ToTable("TSpecItems");

            entity.Property(e => e.TspecModelId).HasColumnName("TSpecModelId");

            entity.HasOne(d => d.TspecModel).WithMany(p => p.TspecItems).HasForeignKey(d => d.TspecModelId);
        });

        modelBuilder.Entity<TspecModel>(entity =>
        {
            entity.ToTable("TSpecModels");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
