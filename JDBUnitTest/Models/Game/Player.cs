namespace JDBUnitTest.Models.Game
{
    public class Player
    {
        public int _id { get; set; }
        public string Username { get; set; }
        public int Health { get; set; }
        public int Speed { get; set; }
        public Player() { }
        public Player(int id, string username)
            : this(id, username, health: 100, speed: 10)
        { }
        public Player(int id, string username, int health, int speed)
        {
            _id = id;
            Username = username;
            Health = health;
            Speed = speed;
        }
    }
}
