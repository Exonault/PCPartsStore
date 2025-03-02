using PCPartsStore.Entities;

namespace PCPartsStore.Repository.Interfaces;

public interface IAddressRepository
{
    Task AddAddress(Address address);
    
    Task<IEnumerable<Address>> GetAddresses();
    
    Task<Address?> GetAddressById(int id);
    
    Task UpdateAddress(Address address);
    
    Task DeleteAddress(Address address);
}