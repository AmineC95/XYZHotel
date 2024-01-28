using Microsoft.AspNetCore.Mvc;
using XYZHotel.Domain.Entities;
using XYZHotel.Domain.Enums;
using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly string _csvFilePath;


        public ReservationController()
        {
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "ReservationDB.csv");
        }

        [HttpGet("/GetReservations")]
        public IActionResult GetReservations()
        {
            var reservations = ReadReservationsFromCsv();

            if (!reservations.Any())
                return NotFound("No reservations found.");

            return Ok(reservations);
        }

        [HttpGet("/GetReservation/{reservationId}")]
        public IActionResult GetReservation(Guid reservationId)
        {
            var reservations = ReadReservationsFromCsv();

            var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);

            if (reservation == null)
                return NotFound("No reservation found for the given reservation ID.");

            return Ok(reservation);
        }

        [HttpPost("/CreateReservation")]
        public IActionResult CreateReservation([FromBody] Reservation newReservation)
        {
            var reservations = ReadReservationsFromCsv();

            newReservation.Id = (Guid)newReservation.Customer.Id;
            reservations.Add(newReservation);
            WriteReservationsToCsv(reservations);

            return CreatedAtAction(nameof(GetReservations), new { id = newReservation.Id }, newReservation);
        }

        [HttpPut("/UpdateReservationStatus/{reservationId}")]
        public IActionResult UpdateReservationStatus(Guid reservationId, [FromBody] ReservationStatus newStatus)
        {
            var reservations = ReadReservationsFromCsv();
            var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null)
                return NotFound("Reservation not found.");

            reservation.Status = newStatus;
            WriteReservationsToCsv(reservations);
            return Ok(reservation);
        }

        [HttpDelete("/DeleteReservation/{reservationId}")]
        public IActionResult DeleteReservation(Guid reservationId)
        {
            var reservations = ReadReservationsFromCsv();
            var reservation = reservations.FirstOrDefault(r => r.Id == reservationId);
            if (reservation == null)
                return NotFound("Reservation not found.");

            reservations.Remove(reservation);
            WriteReservationsToCsv(reservations);
            return NoContent();
        }

        private List<Reservation> ReadReservationsFromCsv()
        {
            var reservations = new List<Reservation>();

            if (!System.IO.File.Exists(_csvFilePath))
                return reservations;

            var lines = System.IO.File.ReadAllLines(_csvFilePath);

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                if (Guid.TryParse(values[0], out var id))
                {
                    DateTime.TryParse(values[4], out var checkInDate);
                    DateTime.TryParse(values[5], out var checkOutDate);
                    int.TryParse(values[6], out var numberOfNights);
                    Enum.TryParse(values[7], out ReservationStatus status);
                    Enum.TryParse(values[3], out RoomType roomType);

                    var reservation = new Reservation
                    {
                        Id = id,
                        Customer = new Customer { FullName = values[1], PhoneNumber = new PhonesNumber(values[2]) },
                        Room = new Room { Type = roomType },
                        CheckInDate = checkInDate,
                        CheckOutDate = checkOutDate,
                        NumberOfNights = numberOfNights,
                        Status = status
                    };
                    reservations.Add(reservation);
                }
            }

            return reservations;
        }

        private void WriteReservationsToCsv(List<Reservation> reservations)
        {
            var lines = new List<string> { "Id,FullName,PhoneNumber,Room,CheckInDate,CheckOutDate,NumberOfNights,Status" };
            lines.AddRange(reservations.Select(reservation =>
                $"{reservation.Id}," +
                $"{(reservation.Customer != null ? reservation.Customer.FullName : string.Empty)}," +
                $"{(reservation.Customer != null && reservation.Customer.PhoneNumber != null ? reservation.Customer.PhoneNumber.Number : string.Empty)}," +
                $"{(reservation.Room != null ? reservation.Room.Type : string.Empty)}," +
                $"{reservation.CheckInDate}," +
                $"{reservation.CheckOutDate},{reservation.NumberOfNights}," +
                $"{reservation.Status},"
                ));

            System.IO.File.WriteAllLines(_csvFilePath, lines);
        }
    }
}
