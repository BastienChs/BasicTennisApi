namespace BasicTennisApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Shortname { get; set; } = "";
        public string Sex { get; set; } = "";
        public Country Country { get; set; } = new Country();
        public string Picture { get; set; } = "";
        public PlayerData Data { get; set; } = new PlayerData();
    }
}
