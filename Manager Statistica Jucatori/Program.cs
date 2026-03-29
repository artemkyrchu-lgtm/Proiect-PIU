using System;
using GestionareaEnum;
using GestionareaJucatorului;
using Jucator;
using Manager_Statistică_Jucători;

namespace ManagerStatisticaJucatori
{
    class Program
    {
        static void Main(string[] args)
        {
            string optiunea;
            Player Player1 = null;


            StocareJucatorului CatalogJucator = Decider.GetPlayerManager();
            do
            {
                Console.WriteLine("MENU");
                Console.WriteLine("C. Citirea Jucatorului");
                Console.WriteLine("A. Afisarea Jucatorului");
                Console.WriteLine("S. Salvarea Jucatorului");
                Console.WriteLine("P. Afisarea listei Jucatori");
                Console.WriteLine("J. Cautarea Jucatorului Dupa Nickname");
          
                Console.WriteLine("X. Iesirea");
                Console.WriteLine("Selectati optiunea dv: ");
                optiunea = Console.ReadLine().ToUpper();

                switch (optiunea)
                {
                    case "C":
                        Player1 = Citirea();
                        break;
                    case "A":
                        if (Player1 == null)
                        {
                            Console.WriteLine("Nu ati introdus un jucator nou");
                            break;
                        }
                        
                        Console.WriteLine(Player1.InfoAf());
                        break;
                    case "S":
                        if (Player1 != null)
                        {
                            CatalogJucator.AddPlayer(Player1);
                            Console.WriteLine("Player este salvat!");
                        } else
                        {
                            Console.WriteLine("Nu ati citit niciun jucator, incercati din nou!");
                            break;
                        }
                        Player1 = null;
                        break;
                    case "P":
                        List<Player> players = CatalogJucator.GetPlayers();
                        if (players.Count == 0)
                        {
                            Console.WriteLine("Nu exista niciun jucator in lista, incercati din nou!");
                            break;
                        }

                        foreach (var item in players)
                        {
                            Console.WriteLine(item.InfoAf());
                        }
                        break;
                    case "J":
                        Console.WriteLine("Introduceti nickname-ul jucatorului pe care doriti sa-l cautati: ");
                        Player player = CatalogJucator.GetPlayerNickname(Console.ReadLine());

                        if(player != null)
                        {
                            Console.WriteLine(player.InfoAf());
                        } else
                        {
                            Console.WriteLine("Nu am gasit niciun jucator cu acest nickname, incercati din nou!");
                            break;
                        }
                        break;
                    
                    case "X":
                        Console.WriteLine("La revedere!");
                        break;
                    default:
                        Console.WriteLine("Ati selectat o optiune gresita, incercati din nou!");
                        break;
                }
            } while (optiunea != "X");
        }
        static Player Citirea()
        {
            int id = 0;
            string nickaname;
            string hero;
            string role;
            string rank;
            int gamesPlayed = 0;
            int damageDealt = 0;
            int healingDone = 0;
            int damageTaken = 0;

            do
            {
                Console.WriteLine("Id: ");
                int.TryParse(Console.ReadLine(), out id);
            }
            while (id <= 0);
            Console.WriteLine("Nickname: ");
            nickaname = Console.ReadLine();
            Console.WriteLine("Hero: ");
            hero = Console.ReadLine();

            var tRole = Enum.GetValues<Rolu>();
            Console.WriteLine("Available Roles: ");
            int cont = 1;
            foreach (var role1 in tRole)
            {
                Console.WriteLine($"{(cont++)} - {role1}");
            }
            int roleIndex;
            do
            {
                Console.WriteLine("Select Role: ");
                int.TryParse(Console.ReadLine(), out roleIndex);
            }
            while (roleIndex < 1 || roleIndex > 3);

            Rolu rolu = (Rolu)(1 << (roleIndex - 1));


            var tRank = Enum.GetValues<Ranku>();
            int rankSelectat = 0;

            Console.WriteLine("Sunt disponibile Rank: ");
            foreach (var rank1 in tRank)
            {
                Console.WriteLine($"{(int)rank1} - {rank1}");
            }

            do
            {
                Console.Write("Select Rank: ");
                int.TryParse(Console.ReadLine(), out rankSelectat);
            }
            while (!Enum.IsDefined(typeof(Ranku), rankSelectat));

            Ranku rankSel = (Ranku)rankSelectat;
            do
            {
                Console.WriteLine("Games Played: ");
                int.TryParse(Console.ReadLine(), out gamesPlayed);
            }
            while (gamesPlayed <= 0);
            do
            {
                Console.WriteLine("Damage Deatl: ");
                int.TryParse(Console.ReadLine(), out damageDealt);
            }
            while (damageDealt <= 0);
            do
            {
                Console.WriteLine("Healing Done: ");
                int.TryParse(Console.ReadLine(), out healingDone);
            }
            while (healingDone < 0);
            do
            {
                Console.WriteLine("Damage Taken: ");
                int.TryParse(Console.ReadLine(), out damageTaken);
            }
            while (damageTaken <= 0);

            
            

            return new Player(id, nickaname, hero, rolu, rankSel, gamesPlayed, damageDealt, healingDone, damageTaken);
        } 
        

    }

}