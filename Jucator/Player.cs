namespace Jucator
{
    public class Player
    {
        public int Id { get; private set; }
        public string Nickname { get; private set; }
        public string Hero { get; private set; }
        public string Role { get; private set; }

        public string Rank { get; private set; } // adica cat de bun joace jucatorul
        public int GamesPlayed { get; private set; }
        public int DamageDealt { get; private set; }
        public int HealingDone { get; private set; }
        public int DamageTaken { get; private set; }

        public Player(int id, string nickname, string hero, string role, string rank, int gamesPlayed, int damageDealt, int healingDone, int damageTaken)
        {
            Id = id;
            Nickname = nickname;
            Hero = hero;
            Role = role;
            Rank = rank;
            GamesPlayed = gamesPlayed;
            DamageDealt = damageDealt;
            HealingDone = healingDone;
            DamageTaken = damageTaken;
        }
        public void Afisare()
        {
            Console.WriteLine("\n======= DATE JUCATOR =======");
            Console.WriteLine($"ID:                 {Id}");
            Console.WriteLine($"Nickname:           {Nickname}");
            Console.WriteLine($"Erou:               {Hero}");
            Console.WriteLine($"Rol:                {Role}");
            Console.WriteLine($"Rank:               {Rank}");
            Console.WriteLine($"Meciuri Jucate:     {GamesPlayed}");
            Console.WriteLine($"Damage Dat:         {DamageDealt}");
            Console.WriteLine($"Healing:            {HealingDone}");
            Console.WriteLine($"Damage Primit:      {DamageTaken}");
            Console.WriteLine("============================\n");
        }

    }
}
