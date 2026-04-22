using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GestiunePieseWPF
{
    public partial class MainWindow : Window
    {
        // Păstrăm lista de piese în memorie
        private List<PiesaAuto> listaPiese = new List<PiesaAuto>();

        public MainWindow()
        {
            InitializeComponent();
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
                    EsteDisponibilOnline = chkOnline.IsChecked ?? false
                };

                // Adăugare în listă
                listaPiese.Add(piesa);

                // Curățăm câmpurile
                txtNume.Clear();
                txtCod.Clear();
                txtPret.Clear();
                rbDepozit.IsChecked = true;
                chkOnline.IsChecked = false;
                
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
            pnlListaPiese.Children.Clear();

            // Căutare (Cerința 3) - după Nume sau Cod
            var pieseFiltrate = string.IsNullOrWhiteSpace(filtru) 
                ? listaPiese 
                : listaPiese.Where(p => p.Nume.Contains(filtru, StringComparison.OrdinalIgnoreCase) || 
                                        p.CodPiesa.Contains(filtru, StringComparison.OrdinalIgnoreCase)).ToList();

            if (pieseFiltrate.Count == 0)
            {
                TextBlock lblInfoText = new TextBlock
                {
                    Text = listaPiese.Count == 0 ? "Nu există piese în listă." : "Nu s-au găsit piese care să corespundă căutării.",
                    Foreground = Brushes.Gray,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                pnlListaPiese.Children.Add(lblInfoText);
                return;
            }

            // Recreăm UI-ul pentru fiecare piesă filtrată
            foreach (var piesa in pieseFiltrate)
            {
                Border card = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(15),
                    Margin = new Thickness(0, 0, 0, 10)
                };

                StackPanel continut = new StackPanel();
                
                // Header: Nume și Disponibilitate
                StackPanel headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
                headerPanel.Children.Add(new TextBlock { Text = piesa.Nume, FontSize = 17, FontWeight = FontWeights.Bold, Foreground = (Brush)new BrushConverter().ConvertFromString("#2C3E50") });
                
                if (piesa.EsteDisponibilOnline)
                {
                    Border badge = new Border { Background = Brushes.LightGreen, CornerRadius = new CornerRadius(4), Padding = new Thickness(5,2,5,2), Margin = new Thickness(10,0,0,0), VerticalAlignment = VerticalAlignment.Center };
                    badge.Child = new TextBlock { Text = "Online", FontSize = 10, FontWeight = FontWeights.Bold, Foreground = Brushes.DarkGreen };
                    headerPanel.Children.Add(badge);
                }
                
                continut.Children.Add(headerPanel);
                
                // Info
                continut.Children.Add(new TextBlock { Text = $"Cod: {piesa.CodPiesa} | Locație: {piesa.Locatie}", Foreground = Brushes.Gray, Margin = new Thickness(0, 5, 0, 0) });
                continut.Children.Add(new TextBlock { Text = piesa.Pret.ToString("F2") + " RON", FontSize = 16, FontWeight = FontWeights.Bold, Foreground = Brushes.Green, Margin = new Thickness(0, 5, 0, 0) });

                card.Child = continut;
                pnlListaPiese.Children.Add(card);
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
    }
}