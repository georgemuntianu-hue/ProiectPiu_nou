using System;
using System.Collections.Generic;

namespace GestiunePieseAuto
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initializam administrarea cu fisierul text
            var admin = new AdministrarePiese_FisierText("piese.txt");
            bool continua = true;

            while (continua)
            {
                Console.WriteLine("\n--- MENIU GESTIUNE PIESE AUTO ---");
                Console.WriteLine("1. Adauga Piesa");
                Console.WriteLine("2. Afiseaza Toate Piesele");
                Console.WriteLine("3. Cauta Piesa (Nume/Cod)");
                Console.WriteLine("4. Filtreaza dupa Pret Maxim");
                Console.WriteLine("5. Filtreaza dupa Locatie");
                Console.WriteLine("0. Iesire");
                Console.Write("Selectati optiunea: ");

                string optiune = Console.ReadLine();
                switch (optiune)
                {
                    case "1":
                        Console.Write("Nume: "); string nume = Console.ReadLine();
                        Console.Write("Cod: "); string cod = Console.ReadLine();
                        Console.Write("Pret: "); double pret = double.Parse(Console.ReadLine());
                        Console.Write("Locatie: "); string loc = Console.ReadLine();
                        Console.Write("Online (da/nu): "); bool online = Console.ReadLine().ToLower() == "da";

                        admin.AdaugaPiesa(new PiesaAuto(nume, cod, pret, loc, online));
                        Console.WriteLine("Piesa a fost salvata in fisier!");
                        break;

                    case "2":
                        AfiseazaLista(admin.GetToatePiesele());
                        break;

                    case "3":
                        Console.Write("Introdu termenul de cautare: ");
                        AfiseazaLista(admin.CautaPiesa(Console.ReadLine()));
                        break;

                    case "4":
                        Console.Write("Introdu pretul maxim: ");
                        if (double.TryParse(Console.ReadLine(), out double pMax))
                            AfiseazaLista(admin.FiltreazaDupaPret(pMax));
                        break;

                    case "5":
                        Console.Write("Introdu locatia: ");
                        AfiseazaLista(admin.FiltreazaDupaLocatie(Console.ReadLine()));
                        break;

                    case "0":
                        continua = false;
                        break;

                    default:
                        Console.WriteLine("Optiune invalida!");
                        break;
                }
            }
        }

        static void AfiseazaLista(List<PiesaAuto> lista)
        {
            Console.WriteLine("\nRezultate:");
            if (lista.Count == 0) Console.WriteLine("Nu s-au gasit piese.");
            else lista.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}