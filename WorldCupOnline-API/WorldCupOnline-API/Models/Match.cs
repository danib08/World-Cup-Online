namespace WorldCupOnline_API.Models
{
    /// <summary>
    /// Match Model
    /// </summary>
    public class Match
    {
        public int id { get; set; }
        public DateTime startdate { get; set; }
        public TimeSpan starttime { get; set; }
        public int goalsteam1 { get; set; }
        public int goalsteam2 { get; set; }
        public string location { get; set; }
        public int stateid { get; set; }
        public string tournamentid { get; set; }
        public int phaseid { get; set; }
        public string mvp { get; set; }
    }
}
