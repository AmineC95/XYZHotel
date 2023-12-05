using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Domain.Entities
{
    public class Wallet
    {
        public Guid? Id { get; private set; }
        public Customer? Owner { get; private set; }
        public Balance? Balance { get; private set; }
    }
}
