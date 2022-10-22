namespace WorldCupOnline_API.Models
{
    public class Tournament
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool local { get; set; }
        public string description { get; set; }
    }
}
