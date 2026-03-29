using System;
using Jucator;

namespace GestionareaJucatorului

{
    public class PlayerManager : StocareJucatorului 
    {
        public List<Player> PlayerList;
        public PlayerManager()
        {
            PlayerList = new List<Player>();
        }
        public void AddPlayer(Player newPlayer) { PlayerList.Add(newPlayer); }
        public void RemovePlayer(Player DelPlayer) { PlayerList.Remove(DelPlayer); }
        
        public Player GetPlayerNickname(string nickname) { return PlayerList.Find(p => p.Nickname == nickname ); }
        public List<Player> GetPlayers() { return PlayerList; }
        public bool updatePlayer(Player playerAct)
        {
            int index = PlayerList.FindIndex(p => p.Id == playerAct.Id);
            if(index == -1)
            {
                return false;
            }
            PlayerList[index] = playerAct;
            return true;
        }
        public void AfisareListeiPlayer()
        {
            foreach (Player player in PlayerList)
            {
                Console.WriteLine($"ID: {player.Id}, Nickname: {player.Nickname}");
            }
        }
    }
}