using SalesManagementApp.Data;
using SalesManagementApp.Extensions;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class ProductService : IProductService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;

        public ProductService(SalesManagementDbContext salesManagementDbContext)
        {
            _salesManagementDbContext = salesManagementDbContext;
        }

        public Task<List<ProductModel>> GetProducts()
        {
            try
            {
                var products = _salesManagementDbContext.Products.Convert(_salesManagementDbContext);
                return products;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
