using GestionareaJucatorului;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace Manager_Statistică_Jucători
{
    public static class Decider
    {
        private const string FORMAT_SALVARE = "FormatSalvare";
        private const string NUME_FISIER    = "NumeFisier";

        public static StocareJucatorului GetPlayerManager()
        {
            string formatSalvare = ConfigurationManager.AppSettings[FORMAT_SALVARE] ?? "memorie";
            string numeFisier    = ConfigurationManager.AppSettings[NUME_FISIER]    ?? "Jucatori";

            string caleCompletaFisier = ResolveCaleFisier(numeFisier, formatSalvare);

            if (formatSalvare == "txt")
            {
                return new PlayerManagerFisierText(caleCompletaFisier);
            }

            return new PlayerManager();
        }

        private static string ResolveCaleFisier(string numeFisier, string extensie)
        {
            string numeFisierCuExtensie = $"{numeFisier}.{extensie}";

            // 1. Lângă executabil (bin\Debug\...) — cel mai simplu și fiabil
            string lângăExe = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                numeFisierCuExtensie);

            if (File.Exists(lângăExe)) return lângăExe;

            // 2. Urcăm până la 4 nivele față de executabil (acoperă bin\Debug\net8.0-windows)
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 4; i++)
            {
                string cale = Path.Combine(dir, numeFisierCuExtensie);
                if (File.Exists(cale)) return cale;
                string parent = Directory.GetParent(dir)?.FullName;
                if (parent == null) break;
                dir = parent;
            }

            // 3. Nu există — îl creăm lângă executabil
            return lângăExe;
        }
    }
}
