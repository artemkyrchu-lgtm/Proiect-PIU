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
                Console.WriteLine("J. Cautarea Jucatoru");
                Console.WriteLine("K. Cautarea Jucatorului");
                Console.WriteLine("X. Iesirea");
                Console.WriteLine("Selectati optiunea dv: ");
                optiunea = Console.ReadLine();

                switch (optiunea)
                {
                    case "C":
                        CatalogJucator.AddPlayer(Citirea());
                        break;
                    case "A":
                        break;
                    case "S":
                        break;
                    case "P":
                        break;
                    case "J":
                        break;
                    case "K":
                        break;
                    case "X":
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
            string performanceStatus;
            int gamesPlayed = 0;
            int damageDealt = 0;
            int healingDone = 0;
            int damageTaken = 0;

            Console.WriteLine("Id: ");
            nickaname = Console.ReadLine();
            Console.WriteLine("Hero: ");
            hero = Console.ReadLine();
            Console.WriteLine("Role: ");
            role = Console.ReadLine();
            Console.WriteLine("Performance Status: ");
            performanceStatus = Console.ReadLine();
            Console.WriteLine("Games Played: ");
            gamesPlayed = int.Parse(Console.ReadLine());
            Console.WriteLine("Damage Dealt: ");
            damageDealt = int.Parse(Console.ReadLine());    
            Console.WriteLine("Healing Done: ");
            healingDone = int.Parse(Console.ReadLine());
            Console.WriteLine("Damage Taken: ");
            damageDealt += int.Parse(Console.ReadLine());

            return new Player(id, nickaname, hero, role, performanceStatus, gamesPlayed, damageDealt, healingDone, damageTaken);
        } 

    }

}