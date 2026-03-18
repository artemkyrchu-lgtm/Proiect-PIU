using System;
using GestionareaJucatorului;
using Jucator;

namespace ManagerStatisticaJucatori
{
    class Program
    {
        static void Main(string[] args)
        {
            string optiunea;
            PlayerManager CatalogJucator = new PlayerManager();
            do
            {
                Console.WriteLine("MENU");
                Console.WriteLine("C. Citirea Jucatorului");
                Console.WriteLine("A. Afisarea Jucatorului");
                Console.WriteLine("S. Salvarea Jucatorului");
                Console.WriteLine("P. Afisarea listei Jucatori");
                Console.WriteLine("J. Cautarea Jucatorului Dupa Nickname");
                Console.WriteLine("K. Cautarea Jucatorilor");
                Console.WriteLine("X. Iesirea");
                Console.WriteLine("Selectati optiunea dv: ");
                optiunea = Console.ReadLine().ToUpper();

                switch (optiunea)
                {
                    case "C":
                        CatalogJucator.Player1 = Citirea();
                        break;
                    case "A":
                        if (CatalogJucator.Player1 == null)
                        {
                            Console.WriteLine("Nu ati introdus un jucator nou");
                            break;
                        }
                        
                        CatalogJucator.Player1.Afisare();
                        break;
                    case "S":
                        if (CatalogJucator.Player1 != null)
                        {
                            CatalogJucator.AddPlayer(CatalogJucator.Player1);
                        } else
                        {
                            Console.WriteLine("Nu ati citit niciun jucator, incercati din nou!");
                            break;
                        }
                        CatalogJucator.Player1 = null;
                        break;
                    case "P":
                        CatalogJucator.AfisareListeiPlayer();
                        break;
                    case "J":
                        Console.WriteLine("Introduceti nickname-ul jucatorului pe care doriti sa-l cautati: ");
                        Player player = CatalogJucator.GetPlayerByNickname(Console.ReadLine());

                        if(player != null)
                        {
                            player.Afisare();
                        } else
                        {
                            Console.WriteLine("Nu am gasit niciun jucator cu acest nickname, incercati din nou!");
                            break;
                        }
                        break;
                    case "K":
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

            Console.WriteLine("Id: ");
            id = int.Parse(Console.ReadLine());
            Console.WriteLine("Nickname: ");
            nickaname = Console.ReadLine();
            Console.WriteLine("Hero: ");
            hero = Console.ReadLine();
            Console.WriteLine("Role: ");
            role = Console.ReadLine();
            Console.WriteLine("Rank: ");
            rank = Console.ReadLine();
            Console.WriteLine("Games Played: ");
            gamesPlayed = int.Parse(Console.ReadLine());
            Console.WriteLine("Damage Dealt: ");
            damageDealt = int.Parse(Console.ReadLine());    
            Console.WriteLine("Healing Done: ");
            healingDone = int.Parse(Console.ReadLine());
            Console.WriteLine("Damage Taken: ");
            damageDealt += int.Parse(Console.ReadLine());

            return new Player(id, nickaname, hero, role, rank, gamesPlayed, damageDealt, healingDone, damageTaken);
        } 
        

    }

}