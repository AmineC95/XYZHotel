using XYZHotel.Domain.Enums;
using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Domain.Entities
{
    public class Room
    {
        public Guid? Id { get; private set; }
        public RoomType? Type { get; private set; }
        public Balance? PricePerNight { get; private set; }
    }
}
