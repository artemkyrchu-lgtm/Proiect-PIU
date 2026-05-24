using GestionareaEchipei;

namespace GestionareaEchipei
{
    public class EchipaManagerFisierText
    {
        private readonly string DenumireaFisier;

        public EchipaManagerFisierText(string denumire)
        {
            DenumireaFisier = denumire;
            Stream stream = File.Open(DenumireaFisier, FileMode.OpenOrCreate);
            stream.Close();
        }

        public List<Echipa> GetEchipe()
        {
            List<Echipa> echipe = new List<Echipa>();
            using (StreamReader fisier = new StreamReader(DenumireaFisier))
            {
                string linie;
                while ((linie = fisier.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(linie))
                        echipe.Add(new Echipa(linie));
                }
            }
            return echipe;
        }

        private int GetNextId()
        {
            List<Echipa> echipe = GetEchipe();
            if (echipe.Count == 0) return 1;
            return echipe.Last().Id + 1;
        }

        public void AddEchipa(Echipa echipa)
        {
            echipa.setId(GetNextId());
            using (StreamWriter fisier = new StreamWriter(DenumireaFisier, true))
            {
                fisier.WriteLine(echipa.ConversieLaSir_PentruFisier());
            }
        }

        public bool UpdateEchipa(Echipa echipaActualizata)
        {
            List<Echipa> echipe = GetEchipe();
            bool succes = false;
            using (StreamWriter fisier = new StreamWriter(DenumireaFisier, false))
            {
                foreach (Echipa e in echipe)
                {
                    Echipa deSalvat = e.Id == echipaActualizata.Id ? echipaActualizata : e;
                    fisier.WriteLine(deSalvat.ConversieLaSir_PentruFisier());
                    if (e.Id == echipaActualizata.Id) succes = true;
                }
            }
            return succes;
        }

        public void SalveazaToate(List<Echipa> echipe)
        {
            using (StreamWriter fisier = new StreamWriter(DenumireaFisier, false))
            {
                foreach (Echipa e in echipe)
                {
                    fisier.WriteLine(e.ConversieLaSir_PentruFisier());
                }
            }
        }
    }
}
