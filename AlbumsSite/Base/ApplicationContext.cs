using AlbumsSite.Entities;
using AlbumsSite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsSite
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Dislike> Dislikes { get; set; }
        public DbSet<JobMessage> JobMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.MyAlbums).WithOne(x => x.Author).OnDelete(DeleteBehavior.Cascade);    
            modelBuilder.Entity<User>().HasMany(x => x.Dislikes).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);    
            modelBuilder.Entity<User>().HasMany(x => x.Likes).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);    
            modelBuilder.Entity<User>().HasMany(x => x.Pictures).WithOne(x => x.Author).OnDelete(DeleteBehavior.NoAction);    
   
        }
    }
}
