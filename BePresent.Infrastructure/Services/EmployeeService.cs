using BePresent.Application.Interfaces;
using BePresent.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BePresent.Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmployeeService> _logger;
        public EmployeeService(ApplicationDbContext context,ILogger<EmployeeService> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<bool> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                    return true;

                }
                return false;
            }
            catch (Exception exc)
            {

                _logger.LogError(exc.Message);
                return false;
            }
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            try
            {
                return await _context.Employees.ToListAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return new List<Employee>();
            }
        }


        public async Task<Employee?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Employees.FindAsync(id);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception exc)
            {

                _logger.LogError(exc.Message);
                return false;
            }
        }
        public async Task<int> GetEmployeeCount()
        {
            try
            {
                return await _context.Employees.CountAsync();
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return 0; // safe fallback: 0 employees if an error happens
            }
        }
        public async Task<(List<Employee> Employees, int TotalCount)> GetPaginatedAsync(string searchText, int pageNumber, int pageSize)
        {
            try { 
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(e =>
                    //(!string.IsNullOrEmpty(e.First_name) && e.First_name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                    //(!string.IsNullOrEmpty(e.Last_name) && e.Last_name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(e.Email) && e.Email.ToLower().Contains(searchText.ToLower()))
                );
            }

            var totalCount = await query.CountAsync();

            var employees = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (employees, totalCount);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                return (new List<Employee>(), 0); 
            }
        }

    }
}
