namespace WorldCupOnline_API.Models
{
    public class BetCreator
    {
        public string team1id { get; set; }
        public int team1goals { get; set; }
        public string[] team1scorers { get; set; }
        public string[] team1assists { get; set; }
        public string team2id { get; set; }
        public int team2goals { get; set; }
        public string[] team2scorers { get; set; }
        public string[] team2assists { get; set; }
        public string mvpid { get; set; }
    }
}
