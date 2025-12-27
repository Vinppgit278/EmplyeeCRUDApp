using EmplyeeCRUDApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmplyeeCRUDApp.Repository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync(string searchInput = "");
        Task<Employee> GetByIdAsync(int employeeId);
        Task InsertAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int employeeId);

        Task<IEnumerable<Department>> GetDepartmentListAsync();
        Task<IEnumerable<Country>> GetCountryListAsync();

        Task<List<State>> GetStatesByCountryIdAsync(int countryId);
        Task<List<City>> GetCitiesByStateIdAsync(int stateId);

        Task SaveAsync();
    }
}
