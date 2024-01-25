﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("/CreateReservation")]
        public IActionResult CreateReservation([FromBody] Reservation newReservation)
        {
            var reservations = ReadReservationsFromCsv();

            newReservation.Id = new int();
            reservations.Add(newReservation);
            WriteReservationsToCsv(reservations);

            return CreatedAtAction(nameof(GetReservations), new { id = newReservation.Id }, newReservation);
        }

        [HttpPut("/UpdateReservation/{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation updatedReservation)
        {
            var reservations = ReadReservationsFromCsv();
            var reservationIndex = reservations.FindIndex(r => r.Id == id);
            if (reservationIndex < 0)
                return NotFound("Reservation not found.");

            reservations[reservationIndex] = updatedReservation;

            WriteReservationsToCsv(reservations);
            return NoContent();
        }

        [HttpDelete("/DeleteReservation/{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservations = ReadReservationsFromCsv();
            var reservation = reservations.FirstOrDefault(r => r.Id == id);
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
                if (int.TryParse(values[0], out var id))
                {
                    DateTime.TryParse(values[3], out var checkInDate);
                    DateTime.TryParse(values[4], out var checkOutDate);
                    int.TryParse(values[5], out var numberOfNights);
                    Enum.TryParse(values[6], out ReservationStatus status);
                    Enum.TryParse(values[2], out RoomType roomType);

                    var reservation = new Reservation
                    {
                        Id = id,
                        Customer = new Customer { PhoneNumber = new PhonesNumber(values[1]) },
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
            var lines = new List<string> { "Id,PhoneNumber,Room,CheckInDate,CheckOutDate,NumberOfNights,Status" };
            lines.AddRange(reservations.Select(reservation =>
                $"{reservation.Id}," +
                $"{reservation.Customer?.PhoneNumber.Number}," +
                $"{reservation.Room?.Type},{reservation.CheckInDate}," +
                $"{reservation.CheckOutDate},{reservation.NumberOfNights}," +
                $"{reservation.Status},"
                ));

            System.IO.File.WriteAllLines(_csvFilePath, lines);
        }
    }
}
