using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EAP.Core.Data;

public partial class EmployeeAdvertisementPortalContext : DbContext
{
    public EmployeeAdvertisementPortalContext()
    {
    }

    public EmployeeAdvertisementPortalContext(DbContextOptions<EmployeeAdvertisementPortalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdvertisementCategoryTbl> AdvertisementCategoryTbls { get; set; }

    public virtual DbSet<AdvertisementDetailsTbl> AdvertisementDetailsTbls { get; set; }

    public virtual DbSet<EmployeeDetailsTbl> EmployeeDetailsTbls { get; set; }

    public virtual DbSet<UserLoginTbl> UserLoginTbls { get; set; }

    public virtual DbSet<UserRoleTbl> UserRoleTbls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-7AQJSKH\\MSSQLSERVER1;Initial Catalog=Employee_Advertisement_Portal;Trusted_Connection=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdvertisementCategoryTbl>(entity =>
        {
            entity.HasKey(e => e.AdvCategoryId);

            entity.ToTable("AdvertisementCategory_tbl");

            entity.Property(e => e.Category)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AdvertisementDetailsTbl>(entity =>
        {
            entity.HasKey(e => e.AdvId).HasName("PK_AdvertisementDetails_tbl_1");

            entity.ToTable("AdvertisementDetails_tbl");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Title)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.AdvCategory).WithMany(p => p.AdvertisementDetailsTbls)
                .HasForeignKey(d => d.AdvCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvertisementDetails_tbl_AdvertisementCategory_tbl");

            entity.HasOne(d => d.Emp).WithMany(p => p.AdvertisementDetailsTbls)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdvertisementDetails_tbl_EmployeeDetails_tbl");
        });

        modelBuilder.Entity<EmployeeDetailsTbl>(entity =>
        {
            entity.HasKey(e => e.EmpId);

            entity.ToTable("EmployeeDetails_tbl");

            entity.Property(e => e.Address)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("date");

            entity.HasOne(d => d.Role).WithMany(p => p.EmployeeDetailsTbls)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EmployeeDetails_tbl_UserRole_tbl");
        });

        modelBuilder.Entity<UserLoginTbl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserLogin_tbl_1");

            entity.ToTable("UserLogin_tbl");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("date");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserRoleTbl>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("UserRole_tbl");

            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
