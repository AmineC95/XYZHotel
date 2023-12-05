using XYZHotel.Domain.ValueObjects;

namespace XYZHotel.Domain.Entities
{
    public class Customer
    {
        public Guid? Id { get; set; }
        public string? FullName { get; set; }
        public Email? Email { get;  set; }
        public PhonesNumber PhoneNumber { get;  set; }
        public string PasswordHash { get; set; }
    }
}
