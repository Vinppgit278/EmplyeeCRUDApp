using EmplyeeCRUDApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmplyeeCRUDApp.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }


        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // seed departments and some lookup data (optional)
            modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentId = 1, DepartmentName = "HR" },
            new Department { DepartmentId = 2, DepartmentName = "Finance" },
            new Department { DepartmentId = 3, DepartmentName = "IT" }
            );


            // seed some countries/states/cities for demo
            modelBuilder.Entity<Country>().HasData(new Country { CountryId = 1, CountryName = "India" });
            modelBuilder.Entity<State>().HasData(new State { StateId = 1, CountryId = 1, StateName = "Maharashtra" });
            modelBuilder.Entity<City>().HasData(new City { CityId = 1, StateId = 1, CityName = "Pune" });
        }
    }
}

