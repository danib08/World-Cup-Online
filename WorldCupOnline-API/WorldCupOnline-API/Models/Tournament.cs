namespace WorldCupOnline_API.Models
{
    /// <summary>
    /// Tournament Model
    /// </summary>
    public class Tournament
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string description { get; set; }
        public int typeid { get; set; }
    }
}
