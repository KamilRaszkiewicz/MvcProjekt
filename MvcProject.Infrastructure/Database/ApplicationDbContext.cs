using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcProject.Domain.Models;
using MvcProject.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<BookBorrowing> BookBorrowings { get; set; }
        public DbSet<Category> Category { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
      
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasMany(x => x.Searches);

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.WishList)
                .WithOne(x => (ApplicationUser)x.User)
                .HasForeignKey(x => x.UsersId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<Wish>().HasKey(x => new { x.UsersId, x.BooksId });
            builder.Entity<Wish>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Wishes)
                .HasForeignKey(x => x.BooksId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<ApplicationUser>()
                .HasMany(x => x.Borrowings)
                .WithOne(x => (ApplicationUser)x.User)
                .HasForeignKey(x => x.UsersId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<BookBasket>().HasKey(x => new { x.UsersId, x.BooksId });

            builder.Entity<BookBasket>()
                .HasOne(x => (ApplicationUser)x.User)
                .WithMany(x => x.Basket)
                .HasForeignKey(x => x.UsersId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<BookBasket>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Baskets)
                .HasForeignKey(x => x.BooksId)
                .HasPrincipalKey(x => x.Id);


            builder.Entity<BookBorrowing>()
                .HasOne(x => x.Book)
                .WithMany(x => x.Borrowings)
                .HasForeignKey(x => x.BooksId)
                .HasPrincipalKey(x => x.Id);

            builder.Entity<Book>()
                .HasMany(x => x.TableOfContents)
                .WithOne()
                .HasForeignKey(x => x.BooksId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}
