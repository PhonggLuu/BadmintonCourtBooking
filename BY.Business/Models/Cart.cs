namespace BY.Business
{
    public class Cart
    {
        public int CourtId { get; set; }
        public string? CourtName { get; set; }
        public string? Image { get; set; }
        public DateOnly DatePlay { get; set; }
        public TimeOnly TimePlay { get; set; }
    }
}
