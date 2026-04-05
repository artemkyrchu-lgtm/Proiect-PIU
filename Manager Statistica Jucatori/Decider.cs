using GestionareaJucatorului;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text;


namespace Manager_Statistică_Jucători
{
    public static class Decider
    {
        private const string FORMAT_SALVARE = "FormatSalvare";
        private const string NUME_FISIER = "NumeFisier";
        
        public static StocareJucatorului GetPlayerManager()
        {
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "";

            string numeFisier = ConfigurationManager.AppSettings[NUME_FISIER] ?? "";
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName ?? "";
            string caleCompletaFisier = locatieFisierSolutie + "\\" + numeFisier;

            Console.WriteLine($"Format de salvare selectat: {caleCompletaFisier}");
            Console.WriteLine($"{formatSalvare}");

            if (formatSalvare != null)
            {
                switch (formatSalvare)
                {
                    default:
                    case "memorie":
                        return new PlayerManager();
                    case "txt":
                        Console.WriteLine("Salvare în fisier text selectată.");
                        return new PlayerManagerFisierText(caleCompletaFisier + "." + formatSalvare);
                }
            }

            return null;
        }
    }
}


