using Jucator;

namespace GestionareaEchipei
{
    public class Echipa
    {
        private const int MAX_JUCATORI = 6;
        private const char SEPARATOR = ';';

        public int Id { get; private set; }
        public string Denumire { get; private set; }
        public List<int> IdJucatori { get; private set; }

        public Echipa(int id, string denumire, List<int> idJucatori)
        {
            Id = id;
            Denumire = denumire;
            IdJucatori = idJucatori ?? new List<int>();
        }

        public Echipa(string linie)
        {
            var parti = linie.Split(SEPARATOR);
            Id = Convert.ToInt32(parti[0].Trim());
            Denumire = parti[1].Trim();
            IdJucatori = new List<int>();

            if (parti.Length > 2 && !string.IsNullOrWhiteSpace(parti[2]))
            {
                foreach (string idStr in parti[2].Split('_'))
                {
                    if (int.TryParse(idStr.Trim(), out int id))
                        IdJucatori.Add(id);
                }
            }
        }

        public bool AdaugaJucator(int idJucator)
        {
            if (IdJucatori.Count >= MAX_JUCATORI) return false;
            if (IdJucatori.Contains(idJucator)) return false;
            IdJucatori.Add(idJucator);
            return true;
        }

        public bool ElinimareJucator(int idJucator)
        {
            return IdJucatori.Remove(idJucator);
        }

        public bool EsteMaxim => IdJucatori.Count >= MAX_JUCATORI;

        public void setId(int id) => Id = id;

        public string ConversieLaSir_PentruFisier()
        {
            string iduri = IdJucatori.Count > 0
                ? string.Join("_", IdJucatori)
                : "";
            return $"{Id}{SEPARATOR}{Denumire}{SEPARATOR}{iduri}";
        }
    }
}