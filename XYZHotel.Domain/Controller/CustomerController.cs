using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using XYZHotel.Domain.Entities;
using XYZHotel.Domain.ValueObjects;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace XYZHotel.Domain.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly string _csvFilePath;
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
        {
            _config = config;
            _csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "CustomerDB.csv");
        }

        [HttpGet("/GetCustomer")]
        public IActionResult GetCustomers()
        {
            var customers = new List<Customer>();

            if (!System.IO.File.Exists(_csvFilePath))
                return NotFound("CSV file not found.");

            var lines = System.IO.File.ReadAllLines(_csvFilePath);

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                var customer = new Customer
                {
                    Id = new Guid(values[0]),
                    FullName = values[1],
                    Email = new Email(values[2]),
                    PhoneNumber = new PhonesNumber(values[3]),
                    PasswordHash = values[4]
                };
                customers.Add(customer);
            }

            return Ok(customers);
        }

        [HttpPost("/CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] Customer newCustomer)
        {
            var customers = ReadCustomersFromCsv();

            if (customers.Any(c => c.Email.Value.Equals(newCustomer.Email.Value, StringComparison.OrdinalIgnoreCase)))
            {
                return BadRequest("Email already in use.");
            }

            newCustomer.Id = Guid.NewGuid();
            newCustomer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newCustomer.PasswordHash);
            customers.Add(newCustomer);
            WriteCustomersToCsv(customers);

            return CreatedAtAction(nameof(GetCustomers), new { id = newCustomer.Id }, newCustomer);
        }


        [HttpPut("/UpdateCustomer/{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] Customer updatedCustomer)
        {
            var customers = ReadCustomersFromCsv();
            var customerIndex = customers.FindIndex(c => c.Id == id);
            if (customerIndex < 0)
                return NotFound("Customer not found.");

            var customerToUpdate = customers[customerIndex];
            customerToUpdate.FullName = updatedCustomer.FullName;
            customerToUpdate.Email = updatedCustomer.Email;
            customerToUpdate.PhoneNumber = updatedCustomer.PhoneNumber;
            customerToUpdate.PasswordHash = updatedCustomer.PasswordHash;

            WriteCustomersToCsv(customers);
            return NoContent();
        }

        [HttpDelete("/DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            var customers = ReadCustomersFromCsv();
            var customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
                return NotFound("Customer not found.");

            customers.Remove(customer);
            WriteCustomersToCsv(customers);
            return NoContent();
        }

        private void WriteCustomersToCsv(List<Customer> customers)
        {
            var lines = new List<string> { "Id,FullName,Email,PhoneNumber,PasswordHash" };
            lines.AddRange(customers.Select(customer =>
                $"{customer.Id},{customer.FullName},{customer.Email},{customer.PhoneNumber},{customer.PasswordHash}"));

            System.IO.File.WriteAllLines(_csvFilePath, lines);
        }
        private List<Customer> ReadCustomersFromCsv()
        {
            var customers = new List<Customer>();

            var lines = System.IO.File.ReadAllLines(_csvFilePath);

            foreach (var line in lines.Skip(1))
            {
                var values = line.Split(',');
                var customer = new Customer
                {
                    Id = new Guid(values[0]),
                    FullName = values[1],
                    Email = new Email(values[2]),
                    PhoneNumber = new PhonesNumber(values[3]),
                    PasswordHash = values[4]
                };
                customers.Add(customer);
            }

            return customers;
        }

        [HttpPost("/Login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var customers = ReadCustomersFromCsv();

            var customer = customers.FirstOrDefault(c => c.Email.Value == loginRequest.Email);
            if (customer == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            bool passwordVerified = BCrypt.Net.BCrypt.Verify(loginRequest.Password, customer.PasswordHash);
            if (!passwordVerified)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Générer le token JWT
            var tokenString = GenerateJSONWebToken(customer);

            return Ok(new { token = tokenString });
        }

        private string GenerateJSONWebToken(Customer customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //public void SetCustomerPassword(Guid customerId, string newPassword)
        //{
        //    var customers = ReadCustomersFromCsv();
        //    var customer = customers.FirstOrDefault(c => c.Id == customerId);
        //    if (customer == null)
        //    {
        //        throw new Exception("Customer not found.");
        //    }

        //    customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
        //    WriteCustomersToCsv(customers);
        //}

    }
}
