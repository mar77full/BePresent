using BePresent.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Application.Interfaces
{
    public interface IEmployeeService
    {
         Task<List<Employee>> GetAllAsync();
         Task<Employee?> GetByIdAsync(int id);
        Task<bool> AddAsync(Employee employee);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(Employee employee);
        Task<(List<Employee> Employees, int TotalCount)> GetPaginatedAsync(string searchText, int pageNumber, int pageSize);
        Task<int> GetEmployeeCount();

    }
}
