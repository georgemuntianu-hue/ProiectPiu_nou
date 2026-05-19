using System.Collections.Generic;
using System.Linq;
using GestiunePieseAuto;

namespace GestiunePieseAuto
{
    public class MagazinAuto
    {
        private List<PiesaAuto> inventar = new List<PiesaAuto>();

        public void AdaugaPiesa(PiesaAuto piesa)
        {
            inventar.Add(piesa);
        }

        public List<PiesaAuto> GetToatePiesele() => inventar;

        public List<PiesaAuto> CautaPiesa(string termenCautare)
        {
            return inventar.Where(p => p.Nume.ToLower().Contains(termenCautare.ToLower()) ||
                                     p.CodPiesa.ToLower() == termenCautare.ToLower()).ToList();
        }

        public List<PiesaAuto> FiltreazaDupaPret(double pretMaxim)
        {
            return inventar.Where(p => p.Pret <= pretMaxim).ToList();
        }

        public List<PiesaAuto> FiltreazaDupaLocatie(string locatie)
        {
            return inventar.Where(p => p.Locatie.ToLower() == locatie.ToLower()).ToList();
        }
    }
}