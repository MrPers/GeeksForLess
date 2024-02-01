using GeeksForLessMVC.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GeeksForLessMVC
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<TreeElement> TreeElements { get; set; }
    }
}