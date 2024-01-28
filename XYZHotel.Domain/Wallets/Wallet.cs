

namespace XYZHotel.Domain.Wallets
{
    public class Wallet
    {
        public int Id { get; private set; }
        public Customer.Customer? Owner { get; private set; }
        public Balance? Balance { get; private set; }
    }
}
