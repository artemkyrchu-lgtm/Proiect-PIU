using System;
using System.Collections.Generic;
using System.Text;
using Jucator;

namespace GestionareaJucatorului
{
    public interface StocareJucatorului
    {
        void AddPlayer(Player newPlayer);
        List<Player> GetPlayers();
        Player GetPlayerNickname(string name);
        bool updatePlayer(Player updatedPlayer);
    }
}
