using GestionareaEnum;

namespace Jucator
{
    public class Player
    {
        public int Id { get; private set; }
        public string Nickname { get; private set; }
        public string Hero { get; private set; }
        public Rolu Role { get; private set; } = 0;

        public Ranku Rank { get; private set; } // adica cat de bun joace jucatorul
        public int GamesPlayed { get; private set; }
        public int DamageDealt { get; private set; }
        public int HealingDone { get; private set; }
        public int DamageTaken { get; private set; }

        public Player(int id, string nickname, string hero, Rolu role, Ranku rank, int gamesPlayed, int damageDealt, int healingDone, int damageTaken)
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
        public string InfoAf()
        {
            string info = "\n======= DATE JUCATOR =======\n";
            info += $"ID:                 {Id}\n";
            info += $"Nickname:           {Nickname}\n";    
            info += $"Erou:               {Hero}\n";
            info += $"Rol:                {Role}\n";
            info += $"Rank:               {Rank}\n";
            info += $"Meciuri Jucate:     {GamesPlayed}\n";
            info += $"Damage Dat:         {DamageDealt}\n";
            info += $"Healing:            {HealingDone}\n";
            info += $"Damage Primit:      {DamageTaken}\n";
            info += "============================\n";
            return info;
        }

    }
}
