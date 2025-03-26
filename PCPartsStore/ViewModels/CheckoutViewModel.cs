using PCPartsStore.Entities;
using PCPartsStore.ViewModels;

namespace AspStore.ViewModels;

public class CheckoutViewModel
{
    public List<Address> UserAddress { get; set; }
    public HashSet<CartViewModel> Cart { get; set; }
}