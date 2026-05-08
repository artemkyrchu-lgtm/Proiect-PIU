using GestionareaEnum;
using GestionareaJucatorului;
using Jucator;
using Manager_Statistică_Jucători;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf
{
    public partial class MainWindow : Window
    {
        private const int NICKNAME_LUNGIME_MIN = 3;
        private const int NICKNAME_LUNGIME_MAX = 20;
        private const int GAMES_MIN            = 1;
        private const int STAT_MIN             = 0;

        private static readonly Brush BORDER_DEFAULT = new SolidColorBrush(Color.FromRgb(0xC8, 0xC8, 0xD4));
        private static readonly Brush BORDER_EROARE  = new SolidColorBrush(Color.FromRgb(0xD0, 0x02, 0x1B));
        private static readonly Brush BG_EROARE      = new SolidColorBrush(Color.FromRgb(0xFF, 0xF0, 0xF2));
        private static readonly Brush LABEL_DEFAULT  = new SolidColorBrush(Color.FromRgb(0x44, 0x44, 0x66));
        private static readonly Brush LABEL_EROARE   = new SolidColorBrush(Color.FromRgb(0xD0, 0x02, 0x1B));

        private readonly StocareJucatorului catalog;
        private int? idJucatorModificat = null;

        public MainWindow()
        {
            InitializeComponent();
            catalog = Decider.GetPlayerManager();
            PopuleazaRank();
            PopuleazaHero();
            RefreshGrid();
        }

        private void PopuleazaRank()
        {
            cmbRank.ItemsSource = Enum.GetNames(typeof(Ranku)).ToList();
            cmbRank.SelectedIndex = 0;
        }

        private void PopuleazaHero()
        {
            cmbHero.ItemsSource = Enum.GetValues(typeof(Herou))
                                      .Cast<Herou>()
                                      .Select(h => h.ToString().Replace('_', ' '))
                                      .ToList();
            cmbHero.SelectedIndex = 0;
        }

        private void RefreshGrid()
        {
            dgJucatori.ItemsSource = null;
            dgJucatori.ItemsSource = catalog.GetPlayers();
        }

        private void dgJucatori_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnModifica.IsEnabled = dgJucatori.SelectedItem != null;
        }

        private void btnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (dgJucatori.SelectedItem is not Player selectat) return;

            idJucatorModificat = selectat.Id;

            txtNickname.Text      = selectat.Nickname;
            cmbHero.SelectedItem  = selectat.Hero.ToString().Replace('_', ' ');
            cmbRank.SelectedItem  = selectat.Rank.ToString();
            chkDps.IsChecked      = selectat.Role.HasFlag(Rolu.Dps);
            chkTank.IsChecked     = selectat.Role.HasFlag(Rolu.Tank);
            chkHealer.IsChecked   = selectat.Role.HasFlag(Rolu.Healer);
            txtGamesPlayed.Text   = selectat.GamesPlayed.ToString();
            txtDamageDealt.Text   = selectat.DamageDealt.ToString();
            txtHealingDone.Text   = selectat.HealingDone.ToString();
            txtDamageTaken.Text   = selectat.DamageTaken.ToString();

            btnAdauga.Content          = "✔ Salvează modificările";
            btnAdauga.Background       = new SolidColorBrush(Color.FromRgb(0x0A, 0x6E, 0x3A));
            txtSubtitle.Text           = $"Modificați datele jucătorului cu ID {selectat.Id} și apăsați „Salvează modificările";
            borderMesaj.Visibility     = Visibility.Collapsed;
        }

        private void btnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!Valideaza()) return;

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

            if (idJucatorModificat.HasValue)
            {
                var jucatorModificat = new Player(
                    idJucatorModificat.Value,
                    txtNickname.Text.Trim(),
                    hero, roluri, rank,
                    games, dmgDealt, healing, dmgTaken
                );

                catalog.updatePlayer(jucatorModificat);
                RefreshGrid();
                AfiseazaMesaj($"Jucătorul cu ID {idJucatorModificat.Value} a fost modificat cu succes!", succes: true);
                InchideModulModificare();
            }
            else
            {
                var jucator = new Player(
                    0,
                    txtNickname.Text.Trim(),
                    hero, roluri, rank,
                    games, dmgDealt, healing, dmgTaken
                );

                catalog.AddPlayer(jucator);
                RefreshGrid();
                AfiseazaMesaj("Jucătorul a fost adăugat cu succes!", succes: true);
            }

            Reseteaza();
        }

        private void btnReseteaza_Click(object sender, RoutedEventArgs e)
        {
            Reseteaza();
            InchideModulModificare();
            borderMesaj.Visibility = Visibility.Collapsed;
        }

        private void InchideModulModificare()
        {
            idJucatorModificat         = null;
            btnAdauga.Content          = "＋ Adaugă jucător";
            btnAdauga.Background       = new SolidColorBrush(Color.FromRgb(0x3B, 0x3B, 0x8E));
            txtSubtitle.Text           = "Completați câmpurile de mai jos pentru a adăuga un jucător nou";
            dgJucatori.SelectedItem    = null;
            btnModifica.IsEnabled      = false;
        }

        private void Reseteaza()
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
            cmbHero.SelectedIndex = 0;
            ReseteazaToateErori();
        }

        private bool Valideaza()
        {
            ReseteazaToateErori();
            bool valid = true;

            string nickname = txtNickname.Text.Trim();
            if (string.IsNullOrEmpty(nickname) || nickname.Length < NICKNAME_LUNGIME_MIN || nickname.Length > NICKNAME_LUNGIME_MAX)
            {
                MarcheazaEroareCmb(errNickname, lblNickname,
                    $"⚠ Nickname: {NICKNAME_LUNGIME_MIN}–{NICKNAME_LUNGIME_MAX} caractere");
                txtNickname.BorderBrush = BORDER_EROARE;
                txtNickname.Background  = BG_EROARE;
                valid = false;
            }

            if (cmbHero.SelectedIndex < 0)
            {
                MarcheazaEroareCmb(errHero, lblHero, "⚠ Selectați un erou");
                valid = false;
            }

            if (chkDps.IsChecked != true && chkTank.IsChecked != true && chkHealer.IsChecked != true)
            {
                errRole.Text       = "⚠ Selectați cel puțin un rol";
                errRole.Visibility = Visibility.Visible;
                valid = false;
            }

            if (!int.TryParse(txtGamesPlayed.Text.Trim(), out int games) || games < GAMES_MIN)
            {
                MarcheazaEroare(txtGamesPlayed, errGamesPlayed, lblGames, $"⚠ Meciuri jucate: minim {GAMES_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtDamageDealt.Text.Trim(), out int dmgDealt) || dmgDealt < STAT_MIN)
            {
                MarcheazaEroare(txtDamageDealt, errDamageDealt, lblDamage, $"⚠ Damage dealt: minim {STAT_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtHealingDone.Text.Trim(), out int healing) || healing < STAT_MIN)
            {
                MarcheazaEroare(txtHealingDone, errHealingDone, lblHealing, $"⚠ Healing done: minim {STAT_MIN}");
                valid = false;
            }

            if (!int.TryParse(txtDamageTaken.Text.Trim(), out int dmgTaken) || dmgTaken < STAT_MIN)
            {
                MarcheazaEroare(txtDamageTaken, errDamageTaken, lblDamageTaken, $"⚠ Damage taken: minim {STAT_MIN}");
                valid = false;
            }

            return valid;
        }

        private void MarcheazaEroare(TextBox camp, TextBlock eroare, Label eticheta, string mesaj)
        {
            camp.BorderBrush      = BORDER_EROARE;
            camp.Background       = BG_EROARE;
            eroare.Text           = mesaj;
            eroare.Visibility     = Visibility.Visible;
            eticheta.Foreground   = LABEL_EROARE;
        }

        private void MarcheazaEroareCmb(TextBlock eroare, Label eticheta, string mesaj)
        {
            eroare.Text         = mesaj;
            eroare.Visibility   = Visibility.Visible;
            eticheta.Foreground = LABEL_EROARE;
        }

        private void ReseteazaToateErori()
        {
            ReseteazaCamp(txtNickname,    errNickname,    lblNickname);
            ReseteazaCamp(txtGamesPlayed, errGamesPlayed, lblGames);
            ReseteazaCamp(txtDamageDealt, errDamageDealt, lblDamage);
            ReseteazaCamp(txtHealingDone, errHealingDone, lblHealing);
            ReseteazaCamp(txtDamageTaken, errDamageTaken, lblDamageTaken);
            errHero.Visibility  = Visibility.Collapsed;
            lblHero.Foreground  = LABEL_DEFAULT;
            errRole.Visibility  = Visibility.Collapsed;
        }

        private void ReseteazaCamp(TextBox camp, TextBlock eroare, Label eticheta)
        {
            camp.BorderBrush    = BORDER_DEFAULT;
            camp.Background     = Brushes.White;
            eroare.Visibility   = Visibility.Collapsed;
            eticheta.Foreground = LABEL_DEFAULT;
        }

        private void AfiseazaMesaj(string text, bool succes)
        {
            txtMesaj.Text = text;
            if (succes)
            {
                borderMesaj.Background = new SolidColorBrush(Color.FromRgb(0xE6, 0xF9, 0xEE));
                txtMesaj.Foreground    = new SolidColorBrush(Color.FromRgb(0x0A, 0x6E, 0x3A));
            }
            else
            {
                borderMesaj.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xF0, 0xF2));
                txtMesaj.Foreground    = new SolidColorBrush(Color.FromRgb(0xD0, 0x02, 0x1B));
            }
            borderMesaj.Visibility = Visibility.Visible;
        }
    }
}
