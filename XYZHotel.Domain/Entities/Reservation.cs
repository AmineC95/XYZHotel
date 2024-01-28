using XYZHotel.Domain.Enums;

namespace XYZHotel.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Customer? Customer { get; set; }
        public Room? Room { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public int NumberOfNights { get; set; }
        public ReservationStatus? Status { get; set; }
    }
}
