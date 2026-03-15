using System;
using System.Collections.Generic;
namespace ManagerStatisticaJucatori
{
    public class PlayerManager
    {
        public List<Player> PlayerList = new List<Player>();
        public void AddPlayer(Player newPlayer) { PlayerList.Add(newPlayer); }
        public void RemovePlayer(Player DelPlayer) { PlayerList.Remove(DelPlayer); }
        public Player FindPlayerById(int id) { return PlayerList.Find(p => p.Id == id); }
        public List<Player> GetPlayerByRole(string role) { return PlayerList.FindAll(p => p.Role == role); }
    }
}