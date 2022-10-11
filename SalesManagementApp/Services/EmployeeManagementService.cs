using Microsoft.EntityFrameworkCore;
using SalesManagementApp.Data;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;

        public EmployeeManagementService(SalesManagementDbContext salesManagementDbContext)
        {
            _salesManagementDbContext = salesManagementDbContext;
        }

        public async Task<List<EmployeeModel>> GetEmployees()
        {
            try
            {
                return await _salesManagementDbContext.Employees.Convert();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
