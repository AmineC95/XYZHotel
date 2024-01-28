using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; }
        public Balance Balance { get; }

        public Wallet(Guid id, Balance balance)
        {
            Id = id;
            Balance = balance;
        }
    }
}
