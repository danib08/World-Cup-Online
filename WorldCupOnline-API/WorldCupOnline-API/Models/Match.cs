namespace WorldCupOnline_API.Models
{
    public class Match
    {
        public int id { get; set; }
        public DateTime startdate { get; set; }
        public DateTime starttime { get; set; }
        public string score { get; set; }
        public string location { get; set; }
        public int stateid { get; set; }
        public string tournamentid { get; set; }
        public int phaseid { get; set; }
    }
}
