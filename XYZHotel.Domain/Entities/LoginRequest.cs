namespace XYZHotel.Domain.Entities
{

    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
