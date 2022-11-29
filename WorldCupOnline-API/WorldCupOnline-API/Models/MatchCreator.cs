namespace WorldCupOnline_API.Models
{
    /// <summary>
    /// MatchCreator Model
    /// </summary>
    public class MatchCreator
    {
        public string team1 { get; set; }
        public string team2 { get; set; }  
        public DateTime startdate { get; set; }
        public DateTime starttime { get; set; }
        public string location { get; set; }
        public int tournamentid { get; set; }
        public int phaseid { get; set; }
    }
}
