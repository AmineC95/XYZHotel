using XYZHotel.Domain.Enums;
using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType? Type { get; set; }
        public Balance? PricePerNight { get; set; }
    }
}
