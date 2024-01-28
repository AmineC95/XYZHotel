namespace XYZHotel.Domain.Customers
{

    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
