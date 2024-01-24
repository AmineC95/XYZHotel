using XYZHotel.Domain.Enums;

namespace XYZHotel.Domain.ValueObjects
{
    public class Balance
    {
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }

    }
}
