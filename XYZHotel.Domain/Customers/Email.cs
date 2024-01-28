using System.ComponentModel.DataAnnotations;

namespace XYZHotel.Domain.Customers
{
    public class Email
    {
        [EmailAddress]
        public string Value { get; }

        public Email(string value)
        {
            Value = value;  
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
