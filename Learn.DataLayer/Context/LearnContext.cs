using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Learn.DataLayer.Entities.Orders;
using Learn.DataLayer.Entities.Permissions;
using Learn.DataLayer.Entities.Products;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;
using Learn.DataLayer.Entities.Blog;
using Learn.DataLayer.Entities.Pages;

namespace Learn.DataLayer.Context
{

    public class LearnContext : DbContext
    {
        public LearnContext(DbContextOptions<LearnContext> options) : base(options)
        {

        }

        #region User

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserDiscountCode> UserDiscountCodes { get; set; }



        #endregion

        #region Wallet
        public DbSet<WalletType> WalletTypes { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        #endregion

        #region Permissions

        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }

        #endregion

        #region Product

        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Manba> Manba { get; set; }


        public DbSet<ProductLevel> ProductLevels { get; set; }

        public DbSet<ProductStatus> ProductStatuses { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<UserProduct> UserProducts { get; set; }

        public DbSet<ProductEpisode> ProductEpisodes { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductVote> ProductVotes { get; set; }
        public DbSet<Season> Seasons { get; set; }


        #endregion


        #region Order

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Discount> Discounts { get; set; }



        #endregion

        #region Blogs

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Blog_Groups> Blog_Groups { get; set; }



        #endregion

        #region pages

        public DbSet<Page> Pages { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
           .SelectMany(t => t.GetForeignKeys())
           .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDelete);

            modelBuilder.Entity<Role>()
              .HasQueryFilter(r => !r.IsDelete);
            modelBuilder.Entity<Blog_Groups>()
            .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<Blog>()
     .HasQueryFilter(r => !r.IsDelete);

            modelBuilder.Entity<Product>()
           .HasQueryFilter(g => !g.IsDelete);

            modelBuilder.Entity<ProductGroup>()
           .HasQueryFilter(g => !g.IsDelete);


            modelBuilder.Entity<ProductComment>()
            .HasQueryFilter(g => !g.IsDelete);
            modelBuilder.Entity<Season>()
          .HasQueryFilter(g => !g.IsDelete);
            modelBuilder.Entity<ProductEpisode>()
          .HasQueryFilter(g => !g.IsDelete);

            base.OnModelCreating(modelBuilder);
        }
    }
}
