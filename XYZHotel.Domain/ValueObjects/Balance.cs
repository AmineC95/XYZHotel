using XYZHotel.Domain.Enums;

namespace XYZHotel.Domain.ValueObjects
{
    public class Balance
    {
        public decimal? Amount { get; }
        public Currency? Currency { get; }

    }
}
