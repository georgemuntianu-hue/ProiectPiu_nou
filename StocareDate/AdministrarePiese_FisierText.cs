using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GestiunePieseAuto
{
    public class AdministrarePiese_FisierText
    {
        private string numeFisier;

        public AdministrarePiese_FisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            if (!File.Exists(numeFisier)) File.Create(numeFisier).Dispose();
        }

        public void AdaugaPiesa(PiesaAuto p)
        {
            File.AppendAllLines(numeFisier, new[] { p.ToFileString() });
        }

        public List<PiesaAuto> GetToatePiesele()
        {
            var piese = new List<PiesaAuto>();
            var linii = File.ReadAllLines(numeFisier);
            foreach (var linie in linii)
            {
                var d = linie.Split(';');
                if (d.Length == 5)
                    piese.Add(new PiesaAuto(d[0], d[1], double.Parse(d[2]), d[3], bool.Parse(d[4])));
            }
            return piese;
        }

        public List<PiesaAuto> CautaPiesa(string termen) =>
            GetToatePiesele().Where(p => p.Nume.ToLower().Contains(termen.ToLower()) ||
                                         p.CodPiesa.ToLower() == termen.ToLower()).ToList();

        public List<PiesaAuto> FiltreazaDupaPret(double pretMaxim) =>
            GetToatePiesele().Where(p => p.Pret <= pretMaxim).ToList();

        public List<PiesaAuto> FiltreazaDupaLocatie(string locatie) =>
            GetToatePiesele().Where(p => p.Locatie.ToLower() == locatie.ToLower()).ToList();
    }
}