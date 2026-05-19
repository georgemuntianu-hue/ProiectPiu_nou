using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GestiunePieseWPF
{
    public class AdministrarePiese_FisierText
    {
        private string numeFisier;

        public AdministrarePiese_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            // Dacă fișierul nu există, îl creăm gol
            if (!File.Exists(numeFisier))
                File.Create(numeFisier).Dispose();
        }

        // Format linie: Nume;CodPiesa;Pret;Locatie;Categorie;EsteDisponibilOnline;DataAdaugare
        private string ToFileString(PiesaAuto p)
        {
            string data = p.DataAdaugare.HasValue
                ? p.DataAdaugare.Value.ToString("yyyy-MM-dd")
                : "";
            return $"{p.Nume};{p.CodPiesa};{p.Pret.ToString(CultureInfo.InvariantCulture)};{p.Locatie};{p.Categorie};{p.EsteDisponibilOnline};{data}";
        }

        // Adaugă o piesă la sfârșitul fișierului
        public void AdaugaPiesa(PiesaAuto p)
        {
            File.AppendAllLines(numeFisier, new[] { ToFileString(p) });
        }

        // Rescrie complet fișierul cu lista curentă (folosit după modificare/ștergere)
        public void SalveazaToate(List<PiesaAuto> piese)
        {
            var linii = piese.Select(p => ToFileString(p));
            File.WriteAllLines(numeFisier, linii);
        }

        // Citește toate piesele din fișier
        public List<PiesaAuto> GetToatePiesele()
        {
            var piese = new List<PiesaAuto>();
            if (!File.Exists(numeFisier)) return piese;

            var linii = File.ReadAllLines(numeFisier);
            foreach (var linie in linii)
            {
                if (string.IsNullOrWhiteSpace(linie)) continue;
                var d = linie.Split(';');
                if (d.Length >= 6)
                {
                    DateTime? data = null;
                    if (d.Length >= 7 && !string.IsNullOrEmpty(d[6]))
                    {
                        if (DateTime.TryParseExact(d[6], "yyyy-MM-dd",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                            data = dt;
                    }

                    piese.Add(new PiesaAuto
                    {
                        Nume = d[0],
                        CodPiesa = d[1],
                        Pret = double.Parse(d[2], CultureInfo.InvariantCulture),
                        Locatie = d[3],
                        Categorie = d[4],
                        EsteDisponibilOnline = bool.Parse(d[5]),
                        DataAdaugare = data
                    });
                }
            }
            return piese;
        }
    }
}