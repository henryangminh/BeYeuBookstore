using ERPWebApp.Data.EF.Configurations;
using ERPWebApp.Data.EF.Extensions;
using ERPWebApp.Data.Entities;
using ERPWebApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ERPWebApp.Data.EF
{
    public class ERPDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ERPDbContext(DbContextOptions options) : base(options)
        {

        }
        #region creare DbSet
        public virtual DbSet<ArAccountsReceivable> ArAccountsReceivable { get; set; }
        public virtual DbSet<ArAccountsReceivableAdjustments> ArAccountsReceivableAdjustments { get; set; }
        
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Function> Functions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
      

        //boq
        
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Identity Config

            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
                .HasKey(x => x.Id);
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
                .HasKey(x => new { x.RoleId, x.UserId });
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });

            modelBuilder.AddConfiguration(new FunctionConfiguration());

            #endregion Identity Config

            

          


            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.LastLogIn).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Street).HasMaxLength(200);
                entity.Property(e => e.Ward).HasMaxLength(100);
                entity.Property(e => e.District).HasMaxLength(100);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.Fax)
                    .HasMaxLength(25)
                    .IsUnicode(false);
                entity.Property(e => e.FullName).HasMaxLength(200);
                entity.Property(e => e.LastupdatedName).HasMaxLength(200);

                entity.Property(e => e.IdNumber)
                    .HasColumnName("IdNumber")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).HasMaxLength(255);

                entity.Property(e => e.Origin_FK).HasColumnName("Origin_FK");

                entity.Property(e => e.TaxIdnumber)
                    .HasColumnName("TaxIDNumber")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                

            });


        }
        /// <summary>
        /// Tự động thêm ngày tạo và ngày sửa đổi  (đối với các class kế thừa từ interface IdateTracking
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                var changedOrAddedItem = item.Entity as IDateTracking;
                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ERPDbContext>
    {
        public ERPDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ERPDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions
                    .ServerVersion(new Version(8, 0, 13), ServerType.MySql)
                    .CharSetBehavior(CharSetBehavior.AppendToAllColumns)
                    .AnsiCharSet(CharSet.Latin1)
                    .UnicodeCharSet(CharSet.Utf8mb4);
            });
            return new ERPDbContext(builder.Options);
        }
    }
}