using GestionareaJucatorului;
using System;
using System.Configuration;
using System.IO;

namespace Manager_Statistică_Jucători
{
    public static class Decider
    {
        private const string FORMAT_SALVARE = "FormatSalvare";
        private const string NUME_FISIER = "NumeFisier";

        public static StocareJucatorului GetPlayerManager()
        {
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "memorie";
            string numeFisier = ConfigurationManager.AppSettings[NUME_FISIER] ?? "Jucatori";

            if (formatSalvare == "txt")
            {
                string locatieSolutie = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName ?? "";
                string locatieCompleta = locatieSolutie + "//" + numeFisier + "." + formatSalvare;
                return new PlayerManagerFisierText(locatieCompleta);
            }

            return new PlayerManager();
        }
    }
}
