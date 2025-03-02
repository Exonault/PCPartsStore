using Microsoft.EntityFrameworkCore;
using PCPartsStore.Data;
using PCPartsStore.Entities;
using PCPartsStore.Repository.Interfaces;

namespace PCPartsStore.Repository;

public class AddressRepository : IAddressRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AddressRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task AddAddress(Address address)
    {
        await _dbContext.UserAddress.AddAsync(address);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Address>> GetAddresses()
    {
        return await _dbContext.UserAddress.ToListAsync();
    }

    public async Task<Address?> GetAddressById(int id)
    {
        return await _dbContext.UserAddress.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAddress(Address address)
    {
        _dbContext.Entry(address).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAddress(Address address)
    {
        _dbContext.UserAddress.Remove(address);
        await _dbContext.SaveChangesAsync();
    }
}