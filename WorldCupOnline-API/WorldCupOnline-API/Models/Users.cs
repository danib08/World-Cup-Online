namespace WorldCupOnline_API.Models
{
    public class Users
    {
        public string username { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string countryid { get; set; }
        public DateTime birthdate { get; set; }
        public int isadmin { get; set; }
        public string password { get; set; }
    }
}
