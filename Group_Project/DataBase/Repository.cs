using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Group_Project.Models;

namespace Group_Project.DataBase
{

    public class Repository : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<DoctorC> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        private readonly string _path = @"C:\Users\samit\Desktop\Group_Project\Group_Project\Group_Project\DB\Users.db";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={_path}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("UserTable");
            modelBuilder.Entity<DoctorC>().ToTable("DoctorTable");
            modelBuilder.Entity<Patient>().ToTable("PatientTable");
        }
    }
}
