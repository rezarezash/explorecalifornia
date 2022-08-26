using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExploreCalifornia.DAL
{
    public class BlogIdentityDbContext : IdentityDbContext<User>
    {
        public BlogIdentityDbContext(DbContextOptions<BlogIdentityDbContext> options)
            :base(options)
        {           
        }
    }
}
