namespace WorldCupOnline_API.Models
{
    public class TournamentCreator
    {
        public string id { get; set; }
        public string name { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string description { get; set; }
        public int typeid { get; set; }
        public string[] teamsIds { get; set; }
        public string[] phases { get; set; } 
    }
}
