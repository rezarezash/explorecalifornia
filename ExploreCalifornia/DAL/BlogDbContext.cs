using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ExploreCalifornia.DAL
{
    public class BlogDbContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogDbContext(DbContextOptions<BlogDbContext> options):base(options)
        {

        }
        public IQueryable<MonthlySpecial> MonthlySpecials => new MonthlySpecial[]
                {
                    new MonthlySpecial
                    {
                         Key = "calm",
                          Name = "California calm package",
                          Type = "Day spa package",
                           Price = 250
                    },
                     new MonthlySpecial
                    {
                         Key = "desert",
                          Name = "From desert to sea",
                          Type = "2 Day salton sea",
                           Price = 350
                    }
                }.AsQueryable();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }       
    }
}
