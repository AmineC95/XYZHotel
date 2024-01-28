using XYZHotel.Domain.Rooms;

namespace XYZHotel.Domain.Bookings

{
    public class Booking
    {
        public int? Id { get; set; }
        public Customer.Customer? Customer { get; set; }
        public Room? Room { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public BookingStatus? Status { get; set; }
    }
}
