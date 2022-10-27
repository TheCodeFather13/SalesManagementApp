using Microsoft.EntityFrameworkCore;
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

                await UpdateSalesOrderReportsTable(orderId, order);
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

        private async Task UpdateSalesOrderReportsTable(int orderId, Order order)
        {
            try
            {
                List<SalesOrderReport> srItems = await (from oi in _salesManagementDbContext.OrderItems
                                                        where oi.OrderId == orderId
                                                        select new SalesOrderReport
                                                        {
                                                            OrderId= orderId,
                                                            OrderDateTime = order.OrderDateTime,
                                                            OrderPrice = order.Price,
                                                            OrderQuantity = order.Quantity,
                                                            OrderItemId = oi.Id,
                                                            OrderItemPrice = oi.Price,
                                                            OrderItemQuantity = oi.Quantity,
                                                            EmployeeId = order.EmployeeId,
                                                            EmployeeFirstName = _salesManagementDbContext.Employees.FirstOrDefault(x => x.Id == order.EmployeeId).FirstName,
                                                            EmployeeLastName = _salesManagementDbContext.Employees.FirstOrDefault(x => x.Id == order.EmployeeId).LastName,
                                                            ProductId = oi.ProductId,
                                                            ProductName = _salesManagementDbContext.Products.FirstOrDefault(x => x.Id == oi.ProductId).Name,
                                                            ProductCategoryId = _salesManagementDbContext.Products.FirstOrDefault(x => x.Id == oi.ProductId).CategoryId,
                                                            ProductCategoryName = _salesManagementDbContext.ProductCategories.FirstOrDefault(x => x.Id == _salesManagementDbContext.Products.FirstOrDefault(x => x.Id == oi.ProductId).CategoryId).Name,
                                                            ClientId = order.ClientId,
                                                            ClientFirstName = _salesManagementDbContext.Clients.FirstOrDefault(x => x.Id == order.ClientId).FirstName,
                                                            ClientLastName = _salesManagementDbContext.Clients.FirstOrDefault(x => x.Id == order.ClientId).LastName,
                                                            RetailOutletId = _salesManagementDbContext.Clients.FirstOrDefault(x => x.Id == order.ClientId).RetailOutletId,
                                                            RetailOutletLocation = _salesManagementDbContext.RetailOutlets.FirstOrDefault(x => x.Id == _salesManagementDbContext.Clients.FirstOrDefault(x => x.Id == order.ClientId).RetailOutletId).Location
                                                        }).ToListAsync();

                _salesManagementDbContext.AddRange(srItems);
                await _salesManagementDbContext.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
