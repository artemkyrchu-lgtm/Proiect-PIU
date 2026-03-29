using GestionareaEnum;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jucator
{
    public class Player
    {
        public int Id { get; private set; }
        public string Nickname { get; private set; }
        public string Hero { get; private set; }
        public Rolu Role { get; private set; } = 0;

        public Ranku Rank { get; private set; } 
        public int GamesPlayed { get; private set; }
        public int DamageDealt { get; private set; }
        public int HealingDone { get; private set; }
        public int DamageTaken { get; private set; }

        private const int ID = 0;
        private const int NICKNAME = 1;
        private const int HERO = 2;
        private const int ROLU = 3;
        private const int RANKU = 4;
        private const int GAMES_PLAYED = 5;
        private const int DAMAGE_DEALT = 6;
        private const int HEALING_DONE = 7;
        private const int DAMAGE_TAKEN = 8;
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';
        private const char SEPARATOR_SECUNDAR_FISIER = '_';



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
        public Player(string linie)
        {
            List<string> date = linie.Split(SEPARATOR_PRINCIPAL_FISIER).ToList();

            this.Id = Convert.ToInt32(date[ID]);
            this.Nickname = date[NICKNAME].Trim();
            this.Hero = date[HERO];
            this.GamesPlayed = Convert.ToInt32(date[GAMES_PLAYED]);
            this.DamageDealt = Convert.ToInt32(date[DAMAGE_DEALT]);
            this.HealingDone = Convert.ToInt32(date[HEALING_DONE]);
            this.DamageTaken = Convert.ToInt32(date[DAMAGE_TAKEN]);

            foreach (string rolu in date[ROLU].Split(SEPARATOR_SECUNDAR_FISIER))
            {
                if (Enum.TryParse(rolu, true, out Rolu rolul)) { this.Role |= rolul; }
            }
            if (Enum.TryParse(date[RANKU], true, out Ranku rank)) { this.Rank = rank; }
        }
        public string ConversieLaSir_PentruFisier()
        {
            
            string roluri = string.Join(SEPARATOR_SECUNDAR_FISIER, Role.ToString().Split(',').Select(r => r.Trim()));

            return $"{Id}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{Nickname ?? "NECUNOSCUT"}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{Hero ?? "NECUNOSCUT"}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{roluri}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{Rank}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{GamesPlayed}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{DamageDealt}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{HealingDone}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{DamageTaken}";
        }

        public void setId(int id)
        {
            this.Id = id;
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
