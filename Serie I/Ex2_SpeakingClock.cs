using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_I
{
    public static class SpeakingClock
    {
        public static string GoodDay(int heure)
        {
            if (heure < 6)
            {
                Console.WriteLine("Il est " + heure + "h, Merveilleuse nuit !");
            }
            if ((heure >= 6) & (heure < 12))
            {
                Console.WriteLine("Il est " + heure + "h, Bonne matinée !");

            }
            if (heure == 12)
            {
                Console.WriteLine("Il est " + heure + "h, Bon appétit !");

            }
            if ((heure >= 13) & (heure < 18))
            {
                Console.WriteLine("Il est " + heure + "h, Profitez de votre après-midi !");

            }
            if (heure >18)
            {
                Console.WriteLine("Il est " + heure + "h, Passez une bonne soirée !");

            }
            return string.Empty;
        }
    }
}
