using GestionareaEnum;
using GestionareaJucatorului;
using GestionareaEchipei;
using Jucator;
using Manager_Statistică_Jucători;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        
        // BINDING INotifyPropertyChanged
        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyDen = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyDen));
        }

        private Player _JucatorCurent;
        public Player JucatorCurent
        {
            get => _JucatorCurent;
            set
            {
                _JucatorCurent = value;
                OnPropertyChanged();
            }
        }

        private const int NICKNAME_LUNGIME_MIN = 3;
        private const int NICKNAME_LUNGIME_MAX = 20;
        private const int GAMES_MIN            = 1;
        private const int STAT_MIN             = 0;
        private const int ECHIPA_DENUMIRE_MIN  = 2;
        private const int ECHIPA_DENUMIRE_MAX  = 30;
        private const int MAX_JUCATORI_ECHIPA  = 6;

        private static readonly Brush BORDER_DEFAULT = new SolidColorBrush(Color.FromRgb(0xD1, 0xD5, 0xDB));
        private static readonly Brush BORDER_EROARE  = new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26));
        private static readonly Brush BG_EROARE      = new SolidColorBrush(Color.FromRgb(0xFF, 0xF2, 0xF2));
        private static readonly Brush LABEL_DEFAULT  = new SolidColorBrush(Color.FromRgb(0x6B, 0x72, 0x80));
        private static readonly Brush LABEL_EROARE   = new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26));

        private static List<string> HeroDps    = CitesteFisierHero("HeroDps.txt");
        private static List<string> HeroTank   = CitesteFisierHero("HeroTank.txt");
        private static List<string> HeroHealer = CitesteFisierHero("HeroHealer.txt");

        private static List<string> CitesteFisierHero(string numeFisier)
        {
            string locatieSolutie = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName ?? "";
            string cale = System.IO.Path.Combine(locatieSolutie, numeFisier);
            if (!System.IO.File.Exists(cale)) return new List<string>();
            return System.IO.File.ReadAllLines(cale)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();
        }

        private readonly StocareJucatorului catalog;
        private readonly EchipaManagerFisierText echipaManager;
        private int? idJucatorModificat = null;

        private List<Player> jucatoriEchipaNoua = new List<Player>();

        private static readonly string CaleJucatoriTxt = ResolveCaleFisierTxt("Jucatori.txt");
        private static readonly string CaleEchipeTxt   = ResolveCaleFisierTxt("Echipa.txt");

        private static string ResolveCaleFisierTxt(string numeFisier)
        {
            string langaExe = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, numeFisier);
            if (System.IO.File.Exists(langaExe)) return langaExe;

            string dir = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 4; i++)
            {
                string cale = System.IO.Path.Combine(dir, numeFisier);
                if (System.IO.File.Exists(cale)) return cale;
                string parent = System.IO.Directory.GetParent(dir)?.FullName;
                if (parent == null) break;
                dir = parent;
            }
            return langaExe;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            catalog = Decider.GetPlayerManager();
            echipaManager = new EchipaManagerFisierText(CaleEchipeTxt);

            PopuleazaRank();
            PopuleazaModifRank();
            ActualizeazaHeroiDisponibili();
            PopuleazaCautareHero();
            RefreshGrid();
            ActualizeazaCounter();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SalveazaInFisier();
        }

        private void SalveazaInFisier()
        {
            try
            {
                List<Player> playeri = catalog.GetPlayers();
                using (var fisier = new System.IO.StreamWriter(CaleJucatoriTxt, false))
                {
                    for (int i = 0; i < playeri.Count; i++)
                    {
                        Player p = playeri[i];
                        p.setId(i + 1);
                        fisier.WriteLine(p.ConversieLaSir_PentruFisier());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la salvarea fisierului:\n{ex.Message}",
                                "Eroare salvare", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        
        // NAVIGARE MENIU SIDEBAR
        

        private void btnMeniuAdaugare_Click(object sender, RoutedEventArgs e)
        {
            ArataPanou("adaugare");
        }

        private void btnMeniuModificare_Click(object sender, RoutedEventArgs e)
        {
            ArataPanou("modificare");
            RefreshGrid();
        }

        private void btnMeniuCautare_Click(object sender, RoutedEventArgs e)
        {
            ArataPanou("cautare");
            dgCautareRezultate.ItemsSource = catalog.GetPlayers();
            borderMesajCautare.Visibility  = Visibility.Collapsed;
        }

        private void btnMeniuEchipe_Click(object sender, RoutedEventArgs e)
        {
            ArataPanou("echipe");
            PopuleazaJucatoriDisponibili();
            RefreshEchipeSalvate();
        }

        private void ArataPanou(string panou)
        {
            PanouAdaugare.Visibility   = Visibility.Collapsed;
            PanouModificare.Visibility = Visibility.Collapsed;
            PanouCautare.Visibility    = Visibility.Collapsed;
            PanouEchipe.Visibility     = Visibility.Collapsed;

            btnMeniuAdaugare.Style   = (Style)FindResource("SidebarBtn");
            btnMeniuModificare.Style = (Style)FindResource("SidebarBtn");
            btnMeniuCautare.Style    = (Style)FindResource("SidebarBtn");
            btnMeniuEchipe.Style     = (Style)FindResource("SidebarBtn");

            switch (panou)
            {
                case "adaugare":
                    PanouAdaugare.Visibility = Visibility.Visible;
                    btnMeniuAdaugare.Style   = (Style)FindResource("SidebarBtnActiv");
                    txtPaginaTitlu.Text      = "Adaugare Jucator Nou";
                    txtPaginaSubtitlu.Text   = "Completati campurile de mai jos pentru a inregistra un jucator nou";
                    break;

                case "modificare":
                    PanouModificare.Visibility = Visibility.Visible;
                    btnMeniuModificare.Style   = (Style)FindResource("SidebarBtnActiv");
                    txtPaginaTitlu.Text        = "Modificare Jucator";
                    txtPaginaSubtitlu.Text     = "Selectati un jucator din lista, editati datele si salvati";
                    break;

                case "cautare":
                    PanouCautare.Visibility = Visibility.Visible;
                    btnMeniuCautare.Style   = (Style)FindResource("SidebarBtnActiv");
                    txtPaginaTitlu.Text     = "Cautare Jucatori";
                    txtPaginaSubtitlu.Text  = "Cauta jucatori dupa Nickname sau dupa Erou";
                    break;

                case "echipe":
                    PanouEchipe.Visibility = Visibility.Visible;
                    btnMeniuEchipe.Style   = (Style)FindResource("SidebarBtnActiv");
                    txtPaginaTitlu.Text    = "Echipe";
                    txtPaginaSubtitlu.Text = "Creati echipe din jucatorii existenti, maxim 6 jucatori per echipa";
                    break;
            }
        }

        
        // INITIALIZARE COMBOURI
        

        private void PopuleazaRank()
        {
            cmbRank.ItemsSource   = Enum.GetNames(typeof(Ranku)).ToList();
            cmbRank.SelectedIndex = 0;
        }

        private void PopuleazaModifRank()
        {
            cmbModifRank.ItemsSource   = Enum.GetNames(typeof(Ranku)).ToList();
            cmbModifRank.SelectedIndex = 0;
        }

        private void PopuleazaCautareHero()
        {
            var toti = Enum.GetNames(typeof(Herou))
                           .Select(h => h.Replace('_', ' '))
                           .OrderBy(h => h)
                           .ToList();
            toti.Insert(0, "-- Toti eroii --");
            cmbCautareHero.ItemsSource   = toti;
            cmbCautareHero.SelectedIndex = 0;
        }

        private void PopuleazaJucatoriDisponibili()
        {
            var jucatori = catalog.GetPlayers();
            var listaAfisare = jucatori
                .Select(p => new { p.Id, Afisare = $"[{p.Id}] {p.Nickname} - {p.Hero.ToString().Replace('_', ' ')} ({p.Rank})" })
                .ToList();
            cmbJucatoriDisponibili.ItemsSource   = listaAfisare;
            cmbJucatoriDisponibili.DisplayMemberPath = "Afisare";
            cmbJucatoriDisponibili.SelectedValuePath = "Id";
            cmbJucatoriDisponibili.SelectedIndex = listaAfisare.Count > 0 ? 0 : -1;
        }

        private void ActualizeazaHeroiDisponibili()
        {
            bool dps    = chkDps.IsChecked    == true;
            bool tank   = chkTank.IsChecked   == true;
            bool healer = chkHealer.IsChecked == true;

            HashSet<string> heroi = new HashSet<string>();
            if (!dps && !tank && !healer)
            {
                heroi.UnionWith(HeroDps);
                heroi.UnionWith(HeroTank);
                heroi.UnionWith(HeroHealer);
            }
            else
            {
                if (dps)    heroi.UnionWith(HeroDps);
                if (tank)   heroi.UnionWith(HeroTank);
                if (healer) heroi.UnionWith(HeroHealer);
            }

            string sel = cmbHero.SelectedItem?.ToString();
            cmbHero.ItemsSource = heroi.OrderBy(h => h).ToList();
            cmbHero.SelectedItem = (sel != null && heroi.Contains(sel)) ? sel : null;
            if (cmbHero.SelectedItem == null) cmbHero.SelectedIndex = 0;
        }

        private void ActualizeazaHeroiModif()
        {
            bool dps    = chkModifDps.IsChecked    == true;
            bool tank   = chkModifTank.IsChecked   == true;
            bool healer = chkModifHealer.IsChecked == true;

            HashSet<string> heroi = new HashSet<string>();
            if (!dps && !tank && !healer)
            {
                heroi.UnionWith(HeroDps);
                heroi.UnionWith(HeroTank);
                heroi.UnionWith(HeroHealer);
            }
            else
            {
                if (dps)    heroi.UnionWith(HeroDps);
                if (tank)   heroi.UnionWith(HeroTank);
                if (healer) heroi.UnionWith(HeroHealer);
            }

            string sel = cmbModifHero.SelectedItem?.ToString();
            cmbModifHero.ItemsSource = heroi.OrderBy(h => h).ToList();
            cmbModifHero.SelectedItem = (sel != null && heroi.Contains(sel)) ? sel : null;
            if (cmbModifHero.SelectedItem == null && cmbModifHero.Items.Count > 0)
                cmbModifHero.SelectedIndex = 0;
        }

        private void chkRole_Changed(object sender, RoutedEventArgs e)
        {
            ActualizeazaHeroiDisponibili();
        }

        private void chkModifRole_Changed(object sender, RoutedEventArgs e)
        {
            ActualizeazaHeroiModif();
        }

        
        // REFRESH GRID SI COUNTER
        

        private void RefreshGrid()
        {
            dgJucatori.ItemsSource = null;
            dgJucatori.ItemsSource = catalog.GetPlayers();
            ActualizeazaCounter();
        }

        private void ActualizeazaCounter()
        {
            txtNrJucatori.Text = catalog.GetPlayers().Count.ToString();
        }

        private void RefreshEchipeSalvate()
        {
            var echipe = echipaManager.GetEchipe();
            var totiJucatori = catalog.GetPlayers();

            var randuri = echipe.Select(e => new
            {
                e.Id,
                e.Denumire,
                NrJucatori = e.IdJucatori.Count,
                ListaJucatori = string.Join(", ", e.IdJucatori
                    .Select(id => totiJucatori.FirstOrDefault(p => p.Id == id)?.Nickname ?? $"ID:{id}"))
            }).ToList();

            dgEchipeSalvate.ItemsSource = randuri;
        }

        private void ActualizeazaNrJucatoriEchipa()
        {
            txtNrJucatoriEchipa.Text = $"{jucatoriEchipaNoua.Count} / {MAX_JUCATORI_ECHIPA} jucatori";
        }

        
        // PANOUL ADAUGARE
        

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaAdaugare()) return;

            Rolu roluri = 0;
            if (chkDps.IsChecked    == true) roluri |= Rolu.Dps;
            if (chkTank.IsChecked   == true) roluri |= Rolu.Tank;
            if (chkHealer.IsChecked == true) roluri |= Rolu.Healer;

            Enum.TryParse(cmbRank.SelectedItem?.ToString(), out Ranku rank);

            string heroStr = cmbHero.SelectedItem?.ToString()?.Replace(' ', '_') ?? "";
            Enum.TryParse(heroStr, out Herou hero);

            int.TryParse(txtGamesPlayed.Text.Trim(), out int games);
            int.TryParse(txtDamageDealt.Text.Trim(), out int dmgDealt);
            int.TryParse(txtHealingDone.Text.Trim(), out int healing);
            int.TryParse(txtDamageTaken.Text.Trim(), out int dmgTaken);

            var jucator = new Player(0, txtNickname.Text.Trim(), hero, roluri, rank,
                                     games, dmgDealt, healing, dmgTaken);
            catalog.AddPlayer(jucator);

            ActualizeazaCounter();
            AfiseazaMesaj(borderMesajAdaugare, txtMesajAdaugare,
                          "Jucatorul a fost adaugat cu succes!", succes: true);
            ReseteazaAdaugare();
        }

        private void btnReseteazaAdaugare_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaAdaugare();
            borderMesajAdaugare.Visibility = Visibility.Collapsed;
        }

        private void ReseteazaAdaugare()
        {
            txtNickname.Clear();
            txtGamesPlayed.Clear();
            txtDamageDealt.Clear();
            txtHealingDone.Clear();
            txtDamageTaken.Clear();
            chkDps.IsChecked    = false;
            chkTank.IsChecked   = false;
            chkHealer.IsChecked = false;
            cmbRank.SelectedIndex = 0;
            ActualizeazaHeroiDisponibili();
            ReseteazaEroriAdaugare();
        }

        
        // PANOUL MODIFICARE
        

        private void dgJucatoriSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgJucatori.SelectedItem is not Player selectat) return;

            JucatorCurent = selectat;

            idJucatorModificat = selectat.Id;

            txtModifNickname.Text      = selectat.Nickname;
            chkModifDps.IsChecked      = selectat.Role.HasFlag(Rolu.Dps);
            chkModifTank.IsChecked     = selectat.Role.HasFlag(Rolu.Tank);
            chkModifHealer.IsChecked   = selectat.Role.HasFlag(Rolu.Healer);

            ActualizeazaHeroiModif();

            string heroAfisat = selectat.Hero.ToString().Replace('_', ' ');
            cmbModifHero.SelectedItem = heroAfisat;
            cmbModifRank.SelectedItem = selectat.Rank.ToString();

            txtModifGamesPlayed.Text = selectat.GamesPlayed.ToString();
            txtModifDamageDealt.Text = selectat.DamageDealt.ToString();
            txtModifHealingDone.Text = selectat.HealingDone.ToString();
            txtModifDamageTaken.Text = selectat.DamageTaken.ToString();

            SetModifCampuriEnabled(true);

            bannerSelectare.Visibility  = Visibility.Collapsed;
            btnSalveazaModif.IsEnabled  = true;
            borderMesajModif.Visibility = Visibility.Collapsed;
        }

        private void btnSalveazaModif_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaModificare()) return;

            Rolu roluri = 0;
            if (chkModifDps.IsChecked    == true) roluri |= Rolu.Dps;
            if (chkModifTank.IsChecked   == true) roluri |= Rolu.Tank;
            if (chkModifHealer.IsChecked == true) roluri |= Rolu.Healer;

            Enum.TryParse(cmbModifRank.SelectedItem?.ToString(), out Ranku rank);
            string heroStr = cmbModifHero.SelectedItem?.ToString()?.Replace(' ', '_') ?? "";
            Enum.TryParse(heroStr, out Herou hero);

            int.TryParse(txtModifGamesPlayed.Text.Trim(), out int games);
            int.TryParse(txtModifDamageDealt.Text.Trim(), out int dmgDealt);
            int.TryParse(txtModifHealingDone.Text.Trim(), out int healing);
            int.TryParse(txtModifDamageTaken.Text.Trim(), out int dmgTaken);

            var jucatorModif = new Player(idJucatorModificat.Value,
                                          txtModifNickname.Text.Trim(),
                                          hero, roluri, rank,
                                          games, dmgDealt, healing, dmgTaken);

            catalog.updatePlayer(jucatorModif);
            RefreshGrid();

            AfiseazaMesaj(borderMesajModif, txtMesajModif,
                          $"Jucatorul cu ID {idJucatorModificat.Value} a fost modificat cu succes!", succes: true);
            ReseteazaModificare();
        }

        private void btnAnuleazaModif_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaModificare();
            borderMesajModif.Visibility = Visibility.Collapsed;
        }

        private void ReseteazaModificare()
        {
            idJucatorModificat = null;
            dgJucatori.SelectedItem = null;

            txtModifNickname.Clear();
            txtModifGamesPlayed.Clear();
            txtModifDamageDealt.Clear();
            txtModifHealingDone.Clear();
            txtModifDamageTaken.Clear();
            chkModifDps.IsChecked    = false;
            chkModifTank.IsChecked   = false;
            chkModifHealer.IsChecked = false;
            cmbModifRank.SelectedIndex = 0;
            cmbModifHero.ItemsSource   = null;

            SetModifCampuriEnabled(false);
            bannerSelectare.Visibility = Visibility.Visible;
            btnSalveazaModif.IsEnabled = false;
            ReseteazaEroriModificare();
        }

        private void SetModifCampuriEnabled(bool enabled)
        {
            txtModifNickname.IsEnabled    = enabled;
            cmbModifHero.IsEnabled        = enabled;
            cmbModifRank.IsEnabled        = enabled;
            chkModifDps.IsEnabled         = enabled;
            chkModifTank.IsEnabled        = enabled;
            chkModifHealer.IsEnabled      = enabled;
            txtModifGamesPlayed.IsEnabled = enabled;
            txtModifDamageDealt.IsEnabled = enabled;
            txtModifHealingDone.IsEnabled = enabled;
            txtModifDamageTaken.IsEnabled = enabled;
        }

        
        // PANOUL CAUTARE
        

        private void btnCautaNickname_Click(object sender, RoutedEventArgs e)
        {
            string termen = txtCautareNickname.Text.Trim();
            if (string.IsNullOrEmpty(termen))
            {
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              "Introduceti un nickname pentru cautare!", succes: false);
                return;
            }

            Player gasit = catalog.GetPlayerNickname(termen);
            if (gasit != null)
            {
                dgCautareRezultate.ItemsSource = new List<Player> { gasit };
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              $"Jucatorul \"{gasit.Nickname}\" a fost gasit!", succes: true);
            }
            else
            {
                dgCautareRezultate.ItemsSource = new List<Player>();
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              $"Niciun jucator gasit cu nickname-ul \"{termen}\".", succes: false);
            }
        }

        private void btnCautaHero_Click(object sender, RoutedEventArgs e)
        {
            string selectat = cmbCautareHero.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectat) || selectat == "-- Toti eroii --")
            {
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              "Selectati un erou pentru cautare!", succes: false);
                return;
            }

            if (!Enum.TryParse(selectat.Replace(' ', '_'), true, out Herou heroVal))
            {
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              "Eroul selectat nu este valid.", succes: false);
                return;
            }

            var rezultate = catalog.GetPlayers()
                                   .Where(p => p.Hero == heroVal)
                                   .ToList();

            dgCautareRezultate.ItemsSource = rezultate;

            if (rezultate.Count > 0)
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              $"Gasiti {rezultate.Count} jucator(i) cu eroul \"{selectat}\".", succes: true);
            else
                AfiseazaMesaj(borderMesajCautare, txtMesajCautare,
                              $"Niciun jucator gasit cu eroul \"{selectat}\".", succes: false);
        }

        private void btnResetCautare_Click(object sender, RoutedEventArgs e)
        {
            txtCautareNickname.Clear();
            cmbCautareHero.SelectedIndex   = 0;
            dgCautareRezultate.ItemsSource = catalog.GetPlayers();
            borderMesajCautare.Visibility  = Visibility.Collapsed;
        }

        
        // PANOUL ECHIPE
        

        private void btnAdaugaJucatorInEchipa_Click(object sender, RoutedEventArgs e)
        {
            errJucatorEchipa.Visibility = Visibility.Collapsed;

            if (cmbJucatoriDisponibili.SelectedValue == null)
            {
                errJucatorEchipa.Text       = "Selectati un jucator";
                errJucatorEchipa.Visibility = Visibility.Visible;
                return;
            }

            if (jucatoriEchipaNoua.Count >= MAX_JUCATORI_ECHIPA)
            {
                AfiseazaMesaj(borderMesajEchipa, txtMesajEchipa,
                              $"Echipa poate avea maxim {MAX_JUCATORI_ECHIPA} jucatori!", succes: false);
                return;
            }

            int idSelectat = (int)cmbJucatoriDisponibili.SelectedValue;

            if (jucatoriEchipaNoua.Any(p => p.Id == idSelectat))
            {
                AfiseazaMesaj(borderMesajEchipa, txtMesajEchipa,
                              "Jucatorul este deja in echipa!", succes: false);
                return;
            }

            Player jucator = catalog.GetPlayers().FirstOrDefault(p => p.Id == idSelectat);
            if (jucator == null) return;

            jucatoriEchipaNoua.Add(jucator);
            RefreshTabelEchipaNoua();
            borderMesajEchipa.Visibility = Visibility.Collapsed;
        }

        private void btnStergeJucatorDinEchipa_Click(object sender, RoutedEventArgs e)
        {
            if (dgJucatoriEchipaNoua.SelectedItem is Player selectat)
            {
                jucatoriEchipaNoua.Remove(selectat);
                RefreshTabelEchipaNoua();
            }
            else
            {
                AfiseazaMesaj(borderMesajEchipa, txtMesajEchipa,
                              "Selectati un jucator din lista pentru a-l sterge.", succes: false);
            }
        }

        private void btnReseteazaEchipa_Click(object sender, RoutedEventArgs e)
        {
            ReseteazaPanouEchipa();
        }

        private void btnCreaziEchipa_Click(object sender, RoutedEventArgs e)
        {
            borderMesajEchipa.Visibility = Visibility.Collapsed;
            errDenumireEchipa.Visibility = Visibility.Collapsed;
            txtDenumireEchipa.BorderBrush = BORDER_DEFAULT;
            txtDenumireEchipa.Background  = Brushes.White;
            lblDenumireEchipa.Foreground  = LABEL_DEFAULT;

            string denumire = txtDenumireEchipa.Text.Trim();
            bool valid = true;

            if (string.IsNullOrEmpty(denumire) ||
                denumire.Length < ECHIPA_DENUMIRE_MIN ||
                denumire.Length > ECHIPA_DENUMIRE_MAX)
            {
                txtDenumireEchipa.BorderBrush = BORDER_EROARE;
                txtDenumireEchipa.Background  = BG_EROARE;
                lblDenumireEchipa.Foreground  = LABEL_EROARE;
                errDenumireEchipa.Text        = $"Denumirea: {ECHIPA_DENUMIRE_MIN}-{ECHIPA_DENUMIRE_MAX} caractere";
                errDenumireEchipa.Visibility  = Visibility.Visible;
                valid = false;
            }

            if (jucatoriEchipaNoua.Count == 0)
            {
                AfiseazaMesaj(borderMesajEchipa, txtMesajEchipa,
                              "Adaugati cel putin un jucator in echipa!", succes: false);
                valid = false;
            }

            if (!valid) return;

            var iduri = jucatoriEchipaNoua.Select(p => p.Id).ToList();
            var echipaNoua = new Echipa(0, denumire, iduri);
            echipaManager.AddEchipa(echipaNoua);

            AfiseazaMesaj(borderMesajEchipa, txtMesajEchipa,
                          $"Echipa \"{denumire}\" a fost creata cu succes!", succes: true);

            ReseteazaPanouEchipa();
            RefreshEchipeSalvate();
        }

        private void dgEchipeSalvate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void RefreshTabelEchipaNoua()
        {
            dgJucatoriEchipaNoua.ItemsSource = null;
            dgJucatoriEchipaNoua.ItemsSource = jucatoriEchipaNoua;
            ActualizeazaNrJucatoriEchipa();
        }

        private void ReseteazaPanouEchipa()
        {
            txtDenumireEchipa.Clear();
            txtDenumireEchipa.BorderBrush = BORDER_DEFAULT;
            txtDenumireEchipa.Background  = Brushes.White;
            lblDenumireEchipa.Foreground  = LABEL_DEFAULT;
            errDenumireEchipa.Visibility  = Visibility.Collapsed;
            errJucatorEchipa.Visibility   = Visibility.Collapsed;
            jucatoriEchipaNoua.Clear();
            RefreshTabelEchipaNoua();
            borderMesajEchipa.Visibility = Visibility.Collapsed;
        }

        
        // VALIDARE ADAUGARE
        

        private bool ValideazaAdaugare()
        {
            ReseteazaEroriAdaugare();
            bool valid = true;

            string nickname = txtNickname.Text.Trim();
            if (string.IsNullOrEmpty(nickname) ||
                nickname.Length < NICKNAME_LUNGIME_MIN ||
                nickname.Length > NICKNAME_LUNGIME_MAX)
            {
                MarcheazaEroare(txtNickname, errNickname, lblNickname,
                                $"Nickname: {NICKNAME_LUNGIME_MIN}-{NICKNAME_LUNGIME_MAX} caractere");
                valid = false;
            }

            if (cmbHero.SelectedIndex < 0)
            {
                MarcheazaEroareLabel(errHero, lblHero, "Selectati un erou");
                valid = false;
            }

            if (chkDps.IsChecked != true && chkTank.IsChecked != true && chkHealer.IsChecked != true)
            {
                errRole.Text       = "Selectati cel putin un rol";
                errRole.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!int.TryParse(txtGamesPlayed.Text.Trim(), out int games) || games < GAMES_MIN)
            {
                MarcheazaEroare(txtGamesPlayed, errGamesPlayed, lblGames, $"Minim {GAMES_MIN} meci");
                valid = false;
            }

            if (!int.TryParse(txtDamageDealt.Text.Trim(), out int dmgD) || dmgD < STAT_MIN)
            {
                MarcheazaEroare(txtDamageDealt, errDamageDealt, lblDamage, $"Min {STAT_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtHealingDone.Text.Trim(), out int heal) || heal < STAT_MIN)
            {
                MarcheazaEroare(txtHealingDone, errHealingDone, lblHealing, $"Min {STAT_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtDamageTaken.Text.Trim(), out int dmgT) || dmgT < STAT_MIN)
            {
                MarcheazaEroare(txtDamageTaken, errDamageTaken, lblDamageTaken, $"Min {STAT_MIN}");
                valid = false;
            }

            return valid;
        }

        
        // VALIDARE MODIFICARE
        

        private bool ValideazaModificare()
        {
            ReseteazaEroriModificare();
            bool valid = true;

            string nickname = txtModifNickname.Text.Trim();
            if (string.IsNullOrEmpty(nickname) ||
                nickname.Length < NICKNAME_LUNGIME_MIN ||
                nickname.Length > NICKNAME_LUNGIME_MAX)
            {
                MarcheazaEroare(txtModifNickname, errModifNickname, lblModifNickname,
                                $"Nickname: {NICKNAME_LUNGIME_MIN}-{NICKNAME_LUNGIME_MAX} caractere");
                valid = false;
            }

            if (cmbModifHero.SelectedIndex < 0)
            {
                MarcheazaEroareLabel(errModifHero, lblModifHero, "Selectati un erou");
                valid = false;
            }

            if (chkModifDps.IsChecked != true && chkModifTank.IsChecked != true && chkModifHealer.IsChecked != true)
            {
                errModifRole.Text       = "Selectati cel putin un rol";
                errModifRole.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!int.TryParse(txtModifGamesPlayed.Text.Trim(), out int games) || games < GAMES_MIN)
            {
                MarcheazaEroare(txtModifGamesPlayed, errModifGames, lblModifGames, $"Minim {GAMES_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtModifDamageDealt.Text.Trim(), out int dmgD) || dmgD < STAT_MIN)
            {
                MarcheazaEroare(txtModifDamageDealt, errModifDamage, lblModifDamage, $"Min {STAT_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtModifHealingDone.Text.Trim(), out int heal) || heal < STAT_MIN)
            {
                MarcheazaEroare(txtModifHealingDone, errModifHealing, lblModifHealing, $"Min {STAT_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtModifDamageTaken.Text.Trim(), out int dmgT) || dmgT < STAT_MIN)
            {
                MarcheazaEroare(txtModifDamageTaken, errModifDamageTaken, lblModifDamageTaken, $"Min {STAT_MIN}");
                valid = false;
            }

            return valid;
        }

        
        // MARCARE ERORI
        

        private void MarcheazaEroare(TextBox camp, TextBlock eroare, Label eticheta, string mesaj)
        {
            camp.BorderBrush    = BORDER_EROARE;
            camp.Background     = BG_EROARE;
            eroare.Text         = mesaj;
            eroare.Visibility   = Visibility.Visible;
            eticheta.Foreground = LABEL_EROARE;
        }

        private void MarcheazaEroareLabel(TextBlock eroare, Label eticheta, string mesaj)
        {
            eroare.Text         = mesaj;
            eroare.Visibility   = Visibility.Visible;
            eticheta.Foreground = LABEL_EROARE;
        }

        private void ReseteazaEroriAdaugare()
        {
            ReseteazaCamp(txtNickname,    errNickname,    lblNickname);
            ReseteazaCamp(txtGamesPlayed, errGamesPlayed, lblGames);
            ReseteazaCamp(txtDamageDealt, errDamageDealt, lblDamage);
            ReseteazaCamp(txtHealingDone, errHealingDone, lblHealing);
            ReseteazaCamp(txtDamageTaken, errDamageTaken, lblDamageTaken);
            errHero.Visibility = Visibility.Collapsed;
            lblHero.Foreground = LABEL_DEFAULT;
            errRole.Visibility = Visibility.Collapsed;
        }

        private void ReseteazaEroriModificare()
        {
            ReseteazaCamp(txtModifNickname,    errModifNickname,    lblModifNickname);
            ReseteazaCamp(txtModifGamesPlayed, errModifGames,       lblModifGames);
            ReseteazaCamp(txtModifDamageDealt, errModifDamage,      lblModifDamage);
            ReseteazaCamp(txtModifHealingDone, errModifHealing,     lblModifHealing);
            ReseteazaCamp(txtModifDamageTaken, errModifDamageTaken, lblModifDamageTaken);
            errModifHero.Visibility = Visibility.Collapsed;
            lblModifHero.Foreground = LABEL_DEFAULT;
            errModifRole.Visibility = Visibility.Collapsed;
        }

        private void ReseteazaCamp(TextBox camp, TextBlock eroare, Label eticheta)
        {
            camp.BorderBrush    = BORDER_DEFAULT;
            camp.Background     = Brushes.White;
            eroare.Visibility   = Visibility.Collapsed;
            eticheta.Foreground = LABEL_DEFAULT;
        }

        
        // AFISARE MESAJ
        

        private void AfiseazaMesaj(Border border, TextBlock text, string mesaj, bool succes)
        {
            text.Text = mesaj;
            if (succes)
            {
                border.Background  = new SolidColorBrush(Color.FromRgb(0xDC, 0xFC, 0xE7));
                text.Foreground    = new SolidColorBrush(Color.FromRgb(0x05, 0x96, 0x69));
            }
            else
            {
                border.Background  = new SolidColorBrush(Color.FromRgb(0xFF, 0xF2, 0xF2));
                text.Foreground    = new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26));
            }
            border.Visibility = Visibility.Visible;
        }
    }
}
