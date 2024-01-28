using XYZHotel.Domain.Wallets;


namespace XYZHotel.Domain.Rooms
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType? Type { get; set; }
        public Balance? PricePerNight { get; set; }
    }
}
