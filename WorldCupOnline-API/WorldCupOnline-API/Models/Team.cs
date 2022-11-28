namespace WorldCupOnline_API.Models
{
    /// <summary>
    /// Team Model
    /// </summary>
    public class Team
    {
        public string id { get; set; }
        public string name { get; set; }
        public string confederation { get; set; }
        public int typeid { get; set; }
    }
}
