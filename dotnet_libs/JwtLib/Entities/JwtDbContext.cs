using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtLib.Entities
{
    public class JwtDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public JwtDbContext()
        {

        }

        public JwtDbContext(DbContextOptions<JwtDbContext> options)
            : base(options)
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source=JwtDb");
            }
        }
    }
}
