using GeeksForLessMVC.Controllers;
using GeeksForLessMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GeeksForLessMVC.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<TreeElement> TreeElements { get; set; }
    }
}