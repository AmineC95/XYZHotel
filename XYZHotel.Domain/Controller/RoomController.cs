using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using XYZHotel.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using XYZHotel.Domain.ValueObjects;
using XYZHotel.Domain.Enums;

namespace XYZHotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly string _csvFilePath;
        private readonly IConfiguration _config;

        public RoomController(IConfiguration config)
        {
            _config = config;
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "RoomDB.csv");
        }

        [HttpGet]
        public ActionResult<List<Room>> GetRooms()
        {
            if (!System.IO.File.Exists(_csvFilePath))
                return NotFound("CSV file not found.");

            using var reader = new StreamReader(_csvFilePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<RoomMap>();
            var records = csv.GetRecords<Room>().ToList();

            return Ok(records);
        }
    }
}

public sealed class RoomMap : ClassMap<Room>
{
    public RoomMap()
    {
        Map(m => m.Id).Name("Id");
        Map(m => m.Type).Name("Type");
        Map(m => m.PricePerNight).Convert(row =>
        {
            var price = row.Row.GetField<string>("PricePerNight");
            return new Balance { Amount = decimal.Parse(price), Currency = Currency.EUR };
        });
        Map(m => m.Infos).Convert(row =>
        {
            var infos = row.Row.GetField<string>("Infos");
            return infos.Split(',').ToList();
        });
    }
}