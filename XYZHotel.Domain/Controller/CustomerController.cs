using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

            if (newCustomer.Email?.Value != null)
            {
                if (customers.Any(c => c.Email.Value.Equals(newCustomer.Email.Value, StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest("Email already in use.");
                }
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
            customerToUpdate.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updatedCustomer.PasswordHash);

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

            var tokenString = GenerateJSONWebToken(customer);

            return Ok(new { token = tokenString });
        }

        [Authorize]
        [HttpGet("/GetUserInfo")]
        public IActionResult GetUserInfo()
        {
            var emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var email = emailClaim?.Value;


            var customers = ReadCustomersFromCsv();
            var customer = customers.FirstOrDefault(c => c.Email.Value == email);
            if (customer == null)
            {
                return NotFound("User not found.");
            }

            var user = new
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email.Value,
                PhoneNumber = customer.PhoneNumber,
            };

            return Ok(user);
        }

        private string GenerateJSONWebToken(Customer customer)
        {
            var jwtKey = _config["Jwt:Key"];
            if (jwtKey == null)
            {
                throw new ArgumentNullException(nameof(jwtKey));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.FullName),
                new Claim(JwtRegisteredClaimNames.Email, customer.Email.Value),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
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
