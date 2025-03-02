using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IOrderRepository
{
    Task AddOrder(Order order);
    
    Task<IEnumerable<Order>> GetOrders();
    
    Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId);
    
    Task<Order?> GetOrderId(int id);
    
    Task UpdateOrder(Order order);
    
    Task DeleteOrder(Order order);
}