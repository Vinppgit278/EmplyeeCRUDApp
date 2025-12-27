using EmplyeeCRUDApp.Data;
using EmplyeeCRUDApp.Migrations;
using EmplyeeCRUDApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;


//using RepositoryUsingEFinMVC.DAL;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;

namespace EmplyeeCRUDApp.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;
        public EmployeeRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
        public async Task<IEnumerable<Country>> GetCountryListAsync()
        {
            try
            {
                return await _employeeDbContext.Countries.ToListAsync();
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }

        public async Task<List<Models.State>> GetStatesByCountryIdAsync(int countryId)
        {
            try
            {
                return await _employeeDbContext.States
                               .Where(s => s.CountryId == countryId)
                               .ToListAsync();
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }

        public async Task<List<City>> GetCitiesByStateIdAsync(int stateId)
        {
            try
            {
                return await _employeeDbContext.Cities
                             .Where(c => c.StateId == stateId)
                             .ToListAsync();
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }

        public async Task<IEnumerable<Department>> GetDepartmentListAsync()
        {
            try
            {
                return await _employeeDbContext.Departments.ToListAsync();
            }
            catch (Exception ex)
            {
                throw; // preserve original stack trace
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(string searchString = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchString))
                {
                    //return _employeeDbContext.Employees.Include(e => e.Departments).Include(e => e.Countries).Include(e => e.States.StateId).Include(e => e.Cities.CityId).AsQueryable();
                    return await _employeeDbContext.Employees
                        .Include(e => e.Departments)
                        .Include(e => e.Countries)
                        .Include(e => e.States)
                        .Include(e => e.Cities).ToListAsync();
                }
                else
                {
                    return await _employeeDbContext.Employees
                        .Where(e => e.FirstName.Contains(searchString) || e.LastName.Contains(searchString) || e.Email.Contains(searchString) ||
                            e.Departments.DepartmentName.Contains(searchString))
                        .Include(e => e.Departments)
                        .Include(e => e.Countries)
                        .Include(e => e.States)
                        .Include(e => e.Cities).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }
        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            try
            {
                return await _employeeDbContext.Employees.FindAsync(employeeId);
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }

        public async Task InsertAsync(Employee employee)
        {
            try
            {
                await _employeeDbContext.Employees.AddAsync(employee);
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }

        public Task UpdateAsync(Employee employee)
        {
            try
            {
                _employeeDbContext.Entry(employee).State = EntityState.Modified;
                return Task.CompletedTask; // No async work here
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public async Task DeleteAsync(int employeeId)
        {
            try
            {
                // Use FindAsync instead of Find
                Employee employee = await _employeeDbContext.Employees.FindAsync(employeeId);

                if (employee != null)
                {
                    _employeeDbContext.Employees.Remove(employee);
                }
            }
            catch (Exception ex)
            {
                throw; // preserve stack trace
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _employeeDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw; // rethrow the exception to the service/controller
            }
        }
    }
}
