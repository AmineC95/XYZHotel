using XYZHotel.Domain.Customers;

namespace XYZHotel.Domain.Customer
{
    public class Customer
    {
        public Guid? Id { get; set; }
        public string? FullName { get; set; }
        public Email? Email { get;  set; }
        public PhonesNumber? PhoneNumber { get;  set; }
        public string? PasswordHash { get; set; }
    }
}
