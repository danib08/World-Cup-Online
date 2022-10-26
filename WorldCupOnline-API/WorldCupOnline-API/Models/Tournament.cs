namespace WorldCupOnline_API.Models
{
    public class Tournament
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string description { get; set; }
        public int typeid { get; set; }
    }
}
