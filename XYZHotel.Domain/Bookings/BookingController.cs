using Microsoft.AspNetCore.Mvc;
using XYZHotel.Domain.Bookings;
using XYZHotel.Domain.Rooms;
using XYZHotel.Domain.Customers;
using XYZHotel.Domain.Customer;

namespace XYZHotel.Api.Bookings
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly string _csvFilePath;


        public BookingController()
        {
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "BookingDB.csv");
        }

        [HttpGet("/GetBookings")]
        public IActionResult GetBookings()
        {
            var bookings = ReadBookingsFromCsv();

            if (!bookings.Any())
                return NotFound("No bookings found.");

            return Ok(bookings);
        }

        [HttpPost("/CreateBooking")]
        public IActionResult CreateBooking([FromBody] Booking newBooking)
        {
            var bookings = ReadBookingsFromCsv();

            newBooking.Id = new int();
            bookings.Add(newBooking);
            WriteBookingsToCsv(bookings);

            return CreatedAtAction(nameof(GetBookings), new { id = newBooking.Id }, newBooking);
        }

        [HttpPut("/UpdateBooking/{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] Booking updatedBooking)
        {
            var bookings = ReadBookingsFromCsv();
            var bookingIndex = bookings.FindIndex(r => r.Id == id);
            if (bookingIndex < 0)
                return NotFound("Booking not found.");

            bookings[bookingIndex] = updatedBooking;

            WriteBookingsToCsv(bookings);
            return NoContent();
        }

        [HttpDelete("/DeleteBooking/{id}")]
        public IActionResult DeleteBooking(int id)
        {
            var bookings = ReadBookingsFromCsv();
            var booking = bookings.FirstOrDefault(r => r.Id == id);
            if (booking == null)
                return NotFound("Booking not found.");

            bookings.Remove(booking);
            WriteBookingsToCsv(bookings);
            return NoContent();
        }

        private List<Booking> ReadBookingsFromCsv()
        {
            var bookings = new List<Booking>();

            if (!System.IO.File.Exists(_csvFilePath))
                return bookings;

            var lines = System.IO.File.ReadAllLines(_csvFilePath);

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                if (int.TryParse(values[0], out var id))
                {
                    DateTime.TryParse(values[3], out var checkInDate);
                    DateTime.TryParse(values[4], out var checkOutDate);
                    int.TryParse(values[5], out var numberOfNights);
                    Enum.TryParse(values[6], out BookingStatus status);
                    Enum.TryParse(values[2], out RoomType roomType);

                    var booking = new Booking
                    {
                        Id = id,
                        Customer = new Domain.Customer.Customer { PhoneNumber = new PhonesNumber(values[1]) },
                        Room = new Room { Type = roomType },
                        CheckInDate = checkInDate,
                        CheckOutDate = checkOutDate,
                        NumberOfNights = numberOfNights,
                        Status = status
                    };
                    bookings.Add(booking);
                }
            }

            return bookings;
        }

        private void WriteBookingsToCsv(List<Booking> bookings)
        {
            var lines = new List<string> { "Id,PhoneNumber,Room,CheckInDate,CheckOutDate,NumberOfNights,Status" };
            lines.AddRange(bookings.Select(booking =>
                $"{booking.Id}," +
                $"{booking.Customer?.PhoneNumber.Number}," +
                $"{booking.Room?.Type},{booking.CheckInDate}," +
                $"{booking.CheckOutDate},{booking.NumberOfNights}," +
                $"{booking.Status},"
                ));

            System.IO.File.WriteAllLines(_csvFilePath, lines);
        }
    }
}
