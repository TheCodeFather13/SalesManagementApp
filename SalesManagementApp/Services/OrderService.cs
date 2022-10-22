using SalesManagementApp.Data;
using SalesManagementApp.Entities;
using SalesManagementApp.Models;
using SalesManagementApp.Services.Contracts;

namespace SalesManagementApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly SalesManagementDbContext _salesManagementDbContext;

        public OrderService(SalesManagementDbContext salesManagementDbContext)
        {
            _salesManagementDbContext = salesManagementDbContext;
        }


        public async Task CreateOrder(OrderModel orderModel)
        {
            try
            {
                Order order = new Order
                {
                    OrderDateTime = DateTime.UtcNow,
                    ClientId = orderModel.ClientId,
                    EmployeeId = 9,
                    Price = orderModel.OrderItems.Sum(o => o.Price),
                    Quantity = orderModel.OrderItems.Sum(o => o.Quantity)
                };

                var addedOrder = await _salesManagementDbContext.Orders.AddAsync(order);
                await _salesManagementDbContext.SaveChangesAsync();
                int orderId = addedOrder.Entity.Id;

                var orderItemsToAdd = ReturnOrderItemsWithOrderId(orderId, orderModel.OrderItems);
                _salesManagementDbContext.AddRange(orderItemsToAdd);
               
                await _salesManagementDbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<OrderItem> ReturnOrderItemsWithOrderId(int orderId, List<OrderItem> orderItems)
        {
            return (from oi in orderItems
                   select new OrderItem
                   {
                       OrderId = orderId,
                       Price = oi.Price,
                       Quantity = oi.Quantity,
                       ProductId = oi.ProductId
                   }).ToList();
        }
    }
}
