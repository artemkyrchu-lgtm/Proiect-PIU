using GestionareaEnum;

namespace Jucator
{
    public class Player
    {
        public int    Id           { get; private set; }
        public string Nickname     { get; private set; }
        public Herou  Hero         { get; private set; }
        public Rolu   Role         { get; private set; } = 0;
        public Ranku  Rank         { get; private set; }
        public int    GamesPlayed  { get; private set; }
        public int    DamageDealt  { get; private set; }
        public int    HealingDone  { get; private set; }
        public int    DamageTaken  { get; private set; }

        private const int  ID                       = 0;
        private const int  NICKNAME                 = 1;
        private const int  HERO                     = 2;
        private const int  ROLU                     = 3;
        private const int  RANKU                    = 4;
        private const int  GAMES_PLAYED             = 5;
        private const int  DAMAGE_DEALT             = 6;
        private const int  HEALING_DONE             = 7;
        private const int  DAMAGE_TAKEN             = 8;
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';
        private const char SEPARATOR_SECUNDAR_FISIER  = '_';

        public Player(int id, string nickname, Herou hero, Rolu role, Ranku rank,
                      int gamesPlayed, int damageDealt, int healingDone, int damageTaken)
        {
            Id          = id;
            Nickname    = nickname;
            Hero        = hero;
            Role        = role;
            Rank        = rank;
            GamesPlayed = gamesPlayed;
            DamageDealt = damageDealt;
            HealingDone = healingDone;
            DamageTaken = damageTaken;
        }

        public Player(string linie)
        {
            var date = linie.Split(SEPARATOR_PRINCIPAL_FISIER).ToList();

            Id          = Convert.ToInt32(date[ID]);
            Nickname    = date[NICKNAME].Trim();
            GamesPlayed = Convert.ToInt32(date[GAMES_PLAYED]);
            DamageDealt = Convert.ToInt32(date[DAMAGE_DEALT]);
            HealingDone = Convert.ToInt32(date[HEALING_DONE]);
            DamageTaken = Convert.ToInt32(date[DAMAGE_TAKEN]);

            if (Enum.TryParse(date[HERO], true, out Herou hero)) Hero = hero;

            foreach (string rolu in date[ROLU].Split(SEPARATOR_SECUNDAR_FISIER))
            {
                if (Enum.TryParse(rolu, true, out Rolu rolul)) Role |= rolul;
            }

            if (Enum.TryParse(date[RANKU], true, out Ranku rank)) Rank = rank;
        }

        public string ConversieLaSir_PentruFisier()
        {
            string roluri = string.Join(SEPARATOR_SECUNDAR_FISIER,
                Role.ToString().Split(',').Select(r => r.Trim()));

            return $"{Id}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{Nickname ?? "NECUNOSCUT"}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{Hero}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{roluri}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{Rank}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{GamesPlayed}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{DamageDealt}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{HealingDone}{SEPARATOR_PRINCIPAL_FISIER}" +
                   $"{DamageTaken}";
        }

        public void setId(int id) => Id = id;

        public string InfoAf()
        {
            string info  = "\n======= DATE JUCATOR =======\n";
            info += $"ID:                 {Id}\n";
            info += $"Nickname:           {Nickname}\n";
            info += $"Erou:               {Hero.ToString().Replace('_', ' ')}\n";
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
