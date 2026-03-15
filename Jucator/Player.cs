namespace Jucator
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Hero { get; set; }
        public string Role { get; set; }

        public string PerformanceStatus { get; set; } // adica cat de bun joace jucatorul
        public int GamesPlayed { get; set; }
        public int DamageDealt { get; set; }
        public int HealingDone { get; set; }
        public int DamageTaken { get; set; }

        public Player(int id, string nickname, string hero, string role, string performanceStatus, int gamesPlayed, int damageDealt, int healingDone, int damageTaken)
        {
            Id = id;
            Nickname = nickname;
            Hero = hero;
            Role = role;
            PerformanceStatus = performanceStatus;
            GamesPlayed = gamesPlayed;
            DamageDealt = damageDealt;
            HealingDone = healingDone;
            DamageTaken = damageTaken;
        }

    }
}
