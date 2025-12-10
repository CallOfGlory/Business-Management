using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSide.Database
{
    public class DataBase : DbContext
    {
        public DataBase()
        {
        }

        public DataBase(DbContextOptions<DataBase> options)
        : base(options)
        {
        }

        // Це коллекція обєктів користувачів, вона повязана з колекцією записів в таблиці БД
        // На основі DbSet створюються відповідні таблиці БД
        public DbSet<User> Users { get; set; }//DB table Users

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Server=DESKTOP-7BTDA28;Database=BusinessManagement;TrustServerCertificate=true;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}