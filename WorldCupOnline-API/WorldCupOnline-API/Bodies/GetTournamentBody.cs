namespace WorldCupOnline_API.Bodies
{
    public class GetTournamentBody
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }
}
