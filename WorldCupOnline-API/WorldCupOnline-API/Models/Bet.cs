namespace WorldCupOnline_API.Models
{
    public class Bet
    {
        public int id { get; set; } 
        public int goalsteam1 { get; set; }
        public int goalsteam2 { get; set; }
        public int score { get; set; }
        public string mvp { get; set; }
        public string userid { get; set; }
        public int matchid { get; set; }    
    }
}
