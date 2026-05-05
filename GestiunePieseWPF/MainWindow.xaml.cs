using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GestiunePieseWPF
{
    public partial class MainWindow : Window
    {
        // Păstrăm lista de piese în memorie
        private List<PiesaAuto> listaPiese = new List<PiesaAuto>();
        private List<string> categorii = new List<string> { "Motor", "Caroserie", "Electrice", "Consumabile" };

        // Lista de furnizori (folosim ObservableCollection pentru actualizare automată în UI)
        public ObservableCollection<Furnizor> ListaFurnizori { get; set; } = new ObservableCollection<Furnizor>();

        public MainWindow()
        {
            InitializeComponent();
            cmbCategorie.ItemsSource = categorii;

            // Setăm lista de furnizori ca ItemsSource pentru lstFurnizori (se va defini în XAML)
            lstFurnizori.ItemsSource = ListaFurnizori;
            
            // Câteva date de test
            ListaFurnizori.Add(new Furnizor { Nume = "AutoParts SRL", CIF = "RO123456", Telefon = "0722111222" });
            ListaFurnizori.Add(new Furnizor { Nume = "MotorExpert SA", CIF = "RO987654", Telefon = "0733444555" });
        }

        private void MnuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Aplicație Gestiune Piese Auto\nRealizat pentru Proiect", "Despre", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            // Validări
            if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtCod.Text))
            {
                MessageBox.Show("Introduceți numele și codul piesei!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (double.TryParse(txtPret.Text, out double pret))
            {
                // Determinare locație pe baza RadioButton
                string locatie = "Depozit";
                if (rbMagazin.IsChecked == true) locatie = "Magazin";
                else if (rbFurnizor.IsChecked == true) locatie = "Furnizor";

                // Creare entitate cu noile câmpuri
                PiesaAuto piesa = new PiesaAuto
                {
                    Nume = txtNume.Text,
                    CodPiesa = txtCod.Text,
                    Pret = pret,
                    Locatie = locatie,
                    EsteDisponibilOnline = chkOnline.IsChecked ?? false,
                    Categorie = cmbCategorie.Text,
                    DataAdaugare = dpDataAdaugare.SelectedDate
                };

                // Adăugare în listă
                listaPiese.Add(piesa);

                // Curățăm câmpurile
                txtNume.Clear();
                txtCod.Clear();
                txtPret.Clear();
                rbDepozit.IsChecked = true;
                chkOnline.IsChecked = false;
                cmbCategorie.SelectedIndex = -1;
                cmbCategorie.Text = string.Empty;
                dpDataAdaugare.SelectedDate = null;
                
                // Actualizare UI
                ActualizareListaPiese(txtCautare.Text);
            }
            else
            {
                MessageBox.Show("Introduceți un preț valid!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtCautare_TextChanged(object sender, TextChangedEventArgs e)
        {
            ActualizareListaPiese(txtCautare.Text);
        }

        private void ActualizareListaPiese(string filtru = "")
        {
            // Căutare (Cerința 3) - după Nume sau Cod
            var pieseFiltrate = string.IsNullOrWhiteSpace(filtru) 
                ? listaPiese 
                : listaPiese.Where(p => p.Nume.Contains(filtru, StringComparison.OrdinalIgnoreCase) || 
                                        p.CodPiesa.Contains(filtru, StringComparison.OrdinalIgnoreCase)).ToList();

            lstPiese.ItemsSource = null;
            lstPiese.ItemsSource = pieseFiltrate;
        }

        private void LstPiese_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstPiese.SelectedItem is PiesaAuto piesaSelectata)
            {
                txtNume.Text = piesaSelectata.Nume;
                txtCod.Text = piesaSelectata.CodPiesa;
                txtPret.Text = piesaSelectata.Pret.ToString();
                
                if (piesaSelectata.Locatie == "Depozit") rbDepozit.IsChecked = true;
                else if (piesaSelectata.Locatie == "Magazin") rbMagazin.IsChecked = true;
                else if (piesaSelectata.Locatie == "Furnizor") rbFurnizor.IsChecked = true;

                chkOnline.IsChecked = piesaSelectata.EsteDisponibilOnline;
                
                cmbCategorie.Text = piesaSelectata.Categorie;
                dpDataAdaugare.SelectedDate = piesaSelectata.DataAdaugare;
            }
        }

        private void BtnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (lstPiese.SelectedItem is PiesaAuto piesaSelectata)
            {
                if (string.IsNullOrWhiteSpace(txtNume.Text) || string.IsNullOrWhiteSpace(txtCod.Text))
                {
                    MessageBox.Show("Introduceți numele și codul piesei!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (double.TryParse(txtPret.Text, out double pret))
                {
                    piesaSelectata.Nume = txtNume.Text;
                    piesaSelectata.CodPiesa = txtCod.Text;
                    piesaSelectata.Pret = pret;

                    string locatie = "Depozit";
                    if (rbMagazin.IsChecked == true) locatie = "Magazin";
                    else if (rbFurnizor.IsChecked == true) locatie = "Furnizor";
                    piesaSelectata.Locatie = locatie;

                    piesaSelectata.EsteDisponibilOnline = chkOnline.IsChecked ?? false;
                    piesaSelectata.Categorie = cmbCategorie.Text;
                    piesaSelectata.DataAdaugare = dpDataAdaugare.SelectedDate;

                    // Curățare
                    txtNume.Clear();
                    txtCod.Clear();
                    txtPret.Clear();
                    rbDepozit.IsChecked = true;
                    chkOnline.IsChecked = false;
                    cmbCategorie.SelectedIndex = -1;
                    cmbCategorie.Text = string.Empty;
                    dpDataAdaugare.SelectedDate = null;

                    lstPiese.SelectedItem = null; // deselectare
                    
                    ActualizareListaPiese(txtCautare.Text);
                    MessageBox.Show("Piesa a fost modificată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Introduceți un preț valid!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selectați o piesă din listă pentru a o modifica!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // --- CRUD Furnizori ---

        private void BtnAdaugaFurnizor_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFurnizorNume.Text) || string.IsNullOrWhiteSpace(txtFurnizorCIF.Text))
            {
                MessageBox.Show("Introduceți numele și CIF-ul furnizorului!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var furnizor = new Furnizor
            {
                Nume = txtFurnizorNume.Text,
                CIF = txtFurnizorCIF.Text,
                Telefon = txtFurnizorTelefon.Text
            };

            ListaFurnizori.Add(furnizor);

            // Curățăm câmpurile doar dacă nu am folosit Binding bidirecțional pe textbox-uri pentru adăugare.
            // Aici TextBox-urile sunt bindate la SelectedItem. Pentru adăugare curățăm textul explicit (dacă selectia e null).
            txtFurnizorNume.Text = string.Empty;
            txtFurnizorCIF.Text = string.Empty;
            txtFurnizorTelefon.Text = string.Empty;
            lstFurnizori.SelectedItem = null;
        }

        private void BtnStergeFurnizor_Click(object sender, RoutedEventArgs e)
        {
            if (lstFurnizori.SelectedItem is Furnizor furnizorSelectat)
            {
                ListaFurnizori.Remove(furnizorSelectat);
                MessageBox.Show("Furnizorul a fost șters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selectați un furnizor pentru a-l șterge!", "Atenție", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    // Clasa entității actualizată
    public class PiesaAuto
    {
        public string Nume { get; set; } = string.Empty;
        public string CodPiesa { get; set; } = string.Empty;
        public double Pret { get; set; }
        public string Locatie { get; set; } = string.Empty;
        public bool EsteDisponibilOnline { get; set; }
        public string Categorie { get; set; } = string.Empty;
        public DateTime? DataAdaugare { get; set; }

        public string PretAfisare => Pret.ToString("F2") + " RON";
        public string DataAdaugareFormatata => DataAdaugare.HasValue ? "Adăugat la: " + DataAdaugare.Value.ToShortDateString() : "Fără dată adăugare";
    }

    public class Furnizor : INotifyPropertyChanged
    {
        private string nume = string.Empty;
        private string cif = string.Empty;
        private string telefon = string.Empty;

        public string Nume
        {
            get => nume;
            set { nume = value; OnPropertyChanged(nameof(Nume)); }
        }

        public string CIF
        {
            get => cif;
            set { cif = value; OnPropertyChanged(nameof(CIF)); }
        }

        public string Telefon
        {
            get => telefon;
            set { telefon = value; OnPropertyChanged(nameof(Telefon)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}