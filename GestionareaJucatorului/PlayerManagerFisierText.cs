using System;
using System.Collections.Generic;
using System.Linq;
using Jucator;

namespace GestionareaJucatorului
{
    public class PlayerManagerFisierText : StocareJucatorului
    {
        private string DenumireaFisier = string.Empty;

        public PlayerManagerFisierText(string Denumirea)
        {
            this.DenumireaFisier = Denumirea;
            Stream streamFisierText = File.Open(DenumireaFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }

        public List<Player> GetPlayers()
        {
            List<Player> Playeri = new List<Player>();
            using (StreamReader fisier = new StreamReader(DenumireaFisier))
            {
                string linie = string.Empty;
                while ((linie = fisier.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(linie))
                        Playeri.Add(new Player(linie));
                }
            }
            return Playeri;
        }

        public Player GetPlayerNickname(string Nickname)
        {
            using (StreamReader fisier = new StreamReader(DenumireaFisier))
            {
                string date = string.Empty;
                while ((date = fisier.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(date)) continue;
                    Player playeru = new Player(date);
                    if (playeru.Nickname.Equals(Nickname, StringComparison.OrdinalIgnoreCase))
                        return playeru;
                }
            }
            return null;
        }

        public bool updatePlayer(Player playerActualizat)
        {
            List<Player> playersTxt = GetPlayers();
            bool updateSucces = false;
            using (StreamWriter fisier = new StreamWriter(DenumireaFisier, false))
            {
                foreach (Player playerTxt in playersTxt)
                {
                    Player player = playerTxt;
                    if (playerTxt.Id == playerActualizat.Id)
                        player = playerActualizat;
                    fisier.WriteLine(player.ConversieLaSir_PentruFisier());
                }
                updateSucces = true;
            }
            return updateSucces;
        }

        private int GetNextIdPlayer()
        {
            List<Player> players = GetPlayers();
            if (players.Count == 0) return 1;
            return players.Last().Id + 1;
        }

        public void AddPlayer(Player player)
        {
            player.setId(GetNextIdPlayer());
            using (StreamWriter fisier = new StreamWriter(DenumireaFisier, true))
            {
                fisier.WriteLine(player.ConversieLaSir_PentruFisier());
            }
        }
    }
}