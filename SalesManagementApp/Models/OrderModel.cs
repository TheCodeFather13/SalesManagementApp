using SalesManagementApp.Entities;

namespace SalesManagementApp.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
