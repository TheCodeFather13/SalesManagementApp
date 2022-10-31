using SalesManagementApp.Data;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;

        public OrganisationService(SalesManagementDbContext salesManagementDbContext)
        {
            _salesManagementDbContext = salesManagementDbContext;
        }


        public async Task<List<OrganisationModel>> GetHierrarchy()
        {
            try
            {
                return await _salesManagementDbContext.Employees.ConvertToHierarchy(_salesManagementDbContext);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
