using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddOrder(Order order)
    {
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId)
    {
        return await _dbContext.Orders
            .Where(o => o.User.Id == customerId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderId(int id)
    {
        return await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateOrder(Order order)
    {
        _dbContext.Entry(order).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteOrder(Order order)
    {
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync();
    }
}