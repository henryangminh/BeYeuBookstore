using BeYeuBookstore.Data.EF.Configurations;
using BeYeuBookstore.Data.EF.Extensions;
using BeYeuBookstore.Data.Entities;
using BeYeuBookstore.Data.Interfaces;
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

namespace BeYeuBookstore.Data.EF
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

        public virtual DbSet<AdvertiseContract> AdvertiseContracts { get; set; }
        public virtual DbSet<AdvertisementContent> AdvertisementContents { get; set; }
        public virtual DbSet<AdvertisementPosition> AdvertisementPositions { get; set; }
        public virtual DbSet<Advertiser> Advertisers { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCategory> BookCategorys { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Delivery> Deliverys { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public virtual DbSet<Merchant> Merchants { get; set; }
        public virtual DbSet<MerchantContract> MerchantContracts { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }
        public virtual DbSet<WebMaster> WebMasters { get; set; }
        public virtual DbSet<WebMasterType> WebMasterTypes { get; set; }


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


            modelBuilder.Entity<AdvertiseContract>(entity =>
            {
                entity.HasKey(e => e.KeyId);

            });

            modelBuilder.Entity<AdvertisementContent>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                entity.HasOne(d => d.AdvertiseContract)
                .WithOne(p => p.AdvertisementContentFKNavigation)
                .HasForeignKey<AdvertiseContract>(d => d.AdvertisementContentFK);

                entity.HasOne(d => d.AdvertiserFKNavigation)
              .WithMany(p => p.AdvertisementContents)
              .HasForeignKey(d => d.AdvertiserFK)
              .HasConstraintName("FK_dbo.AdvertisementContent.Advertiser_AdvertiserFK_FK");

                entity.HasOne(d => d.AdvertisementPositionFKNavigation)
              .WithMany(p => p.AdvertisementContents)
              .HasForeignKey(d => d.AdvertisementPositionFK)
              .HasConstraintName("FK_dbo.AdvertisementContent.AdvertisementPosition");

            });

            modelBuilder.Entity<AdvertisementPosition>(entity =>
            {
                entity.HasKey(e => e.KeyId);

               


            });

            modelBuilder.Entity<Advertiser>(entity =>
            {
                entity.HasKey(e => e.KeyId);

            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                entity.HasOne(d => d.BookCategoryFKNavigation)
              .WithMany(p => p.Books)
              .HasForeignKey(d => d.BookCategoryFK)
              .HasConstraintName("FK_dbo.Book.BookCategory_FK");

                entity.HasOne(d => d.MerchantFKNavigation)
              .WithMany(p => p.Books)
              .HasForeignKey(d => d.MerchantFK)
              .HasConstraintName("FK_dbo.Book.MerchantFK");

            });

            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.HasKey(e => e.KeyId);

            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                

            });

            modelBuilder.Entity<Delivery>(entity =>
            {
                entity.HasKey(e => e.KeyId);

            });


            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                entity.HasOne(d => d.CustomerFKNavigation)
              .WithMany(p => p.Invoices)
              .HasForeignKey(d => d.CustomerFK)
              .HasConstraintName("FK_dbo.Invoices.CustomerFK");

                entity.HasOne(d => d.DeliveryFKNavigation)
                .WithOne(p => p.InvoiceFKNavigation)
                .HasForeignKey<Delivery>(d => d.InvoiceFK);

            });


            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                entity.HasOne(d => d.BookFKNavigation)
              .WithMany(p => p.InvoiceDetails)
              .HasForeignKey(d => d.BookFK)
              .HasConstraintName("FK_dbo.InvoiceDetails.BookFK");

                entity.HasOne(d => d.InvoiceFKNavigation)
              .WithMany(p => p.InvoiceDetails)
              .HasForeignKey(d => d.InvoiceFK)
              .HasConstraintName("FK_dbo.InvoiceDetails.InvoiceFK");

            });


            modelBuilder.Entity<Merchant>(entity =>
            {
                entity.HasKey(e => e.KeyId);

            });


            modelBuilder.Entity<MerchantContract>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                entity.HasOne(d => d.MerchantFKNavigation)
              .WithMany(p => p.MerchantContracts)
              .HasForeignKey(d => d.MerchantFK)
              .HasConstraintName("FK_dbo.MerchantContract.MerchantFK");

            });


            modelBuilder.Entity<WebMaster>(entity =>
            {
                entity.HasKey(e => e.KeyId);

                entity.HasOne(d => d.WebMasterTypeFKNavigation)
              .WithMany(p => p.WebMasters)
              .HasForeignKey(d => d.WebMasterTypeFK)
              .HasConstraintName("FK_dbo.WebMasters.WebMasterTypeFK");
            });


            modelBuilder.Entity<WebMasterType>(entity =>
            {
                entity.HasKey(e => e.KeyId);

            });
            /*
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
            */

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