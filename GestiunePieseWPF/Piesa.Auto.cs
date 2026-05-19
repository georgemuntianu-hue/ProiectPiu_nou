using System;

namespace GestiunePieseAuto
{
    public class PiesaAuto
    {
        public string Nume { get; set; }
        public string CodPiesa { get; set; }
        public double Pret { get; set; }
        public string Locatie { get; set; }
        public bool EsteDisponibilOnline { get; set; }

        public PiesaAuto(string nume, string cod, double pret, string locatie, bool online)
        {
            Nume = nume;
            CodPiesa = cod;
            Pret = pret;
            Locatie = locatie;
            EsteDisponibilOnline = online;
        }

        public override string ToString()
        {
            string tip = EsteDisponibilOnline ? "Online" : "Magazin Fizic";
            return $"Cod: {CodPiesa.PadRight(10)} | {Nume.PadRight(15)} | {Pret} RON | Locatie: {Locatie} ({tip})";
        }

        // Metodă pentru a transforma obiectul în linie de text pentru fișier
        public string ToFileString() => $"{Nume};{CodPiesa};{Pret};{Locatie};{EsteDisponibilOnline}";
    }
}