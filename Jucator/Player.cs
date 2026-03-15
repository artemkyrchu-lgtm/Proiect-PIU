namespace Jucator
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Hero { get; set; }
        public string Role { get; set; }

        public string PerformanceStatus { get; set; } // adica cat de bun joace jucatorul
        public double GamesPlayed { get; set; }
        public int DamageDealt { get; set; }
        public int HealingDone { get; set; }
        public int DamageTaken { get; set; }

    }
}
