using System;
using Jucator;

namespace GestionareaJucatorului

{
    public class PlayerManager
    {
        public List<Player> PlayerList;
        public Player Player1 = null;
        public PlayerManager()
        {
            PlayerList = new List<Player>();
        }
        public void AddPlayer(Player newPlayer) { PlayerList.Add(newPlayer); }
        public void RemovePlayer(Player DelPlayer) { PlayerList.Remove(DelPlayer); }
        public Player FindPlayerById(int id) { return PlayerList.Find(p => p.Id == id); }
        public Player GetPlayerByNickname(string nickname) { return PlayerList.Find(p => p.Nickname == nickname ); }
        public void AfisareListeiPlayer()
        {
            foreach (Player player in PlayerList)
            {
                Console.WriteLine($"ID: {player.Id}, Nickname: {player.Nickname}");
            }
        }
    }
}