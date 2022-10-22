using SalesManagementApp.Data;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class ClientService : IClientService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;

        public ClientService(SalesManagementDbContext salesManagementDbContext)
        {
            _salesManagementDbContext = salesManagementDbContext;
        }


        public async Task<List<ClientModel>> GetClients()
        {
            try
            {
                return await _salesManagementDbContext.Clients.Convert(_salesManagementDbContext);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
