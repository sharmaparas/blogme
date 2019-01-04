using ArticlePool.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticlePool.ApplicationContext
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() { }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ArticleTag> ArticleTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensures Article entry has title property on creation.
            modelBuilder.Entity<Article>()
                .Property(a => a.Title)
                .IsRequired();

            modelBuilder.Entity<Article>()
                .HasIndex(a => a.Title)
                .IsUnique();

            // Ensures Comment entry has a content property on creation with max length of 500, however, the validation is left to the Database provider.
            modelBuilder.Entity<Comment>()
                .Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(500);

            // Ensures Tag entry has title property on creation.
            modelBuilder.Entity<Tag>()
                .Property(t => t.Title)
                .IsRequired();

            // Ignores the LoadedFromDatabase property in Tag class
            modelBuilder.Entity<Tag>()
                .Ignore(t => t.DateAdded);

            // Declared composite primary key for Article-Tag
            modelBuilder.Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });
        }
    }
}
