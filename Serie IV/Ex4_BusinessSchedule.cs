using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_IV
{
    public class BusinessSchedule
    {
        public SortedDictionary<DateTime, TimeSpan> planning = new SortedDictionary<DateTime, TimeSpan>();
        public DateTime Begin;
        public DateTime End;

        public bool IsEmpty()
        {
            return planning.Count == 0;
        }

        public void SetRangeOfDates(DateTime begin, DateTime end)
        {
            Begin = begin;
            End = end;
            return;
        }

        private KeyValuePair<DateTime, DateTime> ClosestElements(DateTime beginMeeting)
        {
            DateTime voisin_bas = Begin;
            DateTime voisin_haut = End;
            foreach (DateTime meeting in planning.Keys)
            {
                if (meeting > voisin_bas && meeting <= beginMeeting)
                {
                    voisin_bas = meeting;
                }
                if (meeting < voisin_haut && meeting > beginMeeting)
                {
                    voisin_haut = meeting;
                }
            }
            return new KeyValuePair<DateTime, DateTime>(voisin_bas,voisin_haut);
        }

        public bool AddBusinessMeeting(DateTime date, TimeSpan duration)
        {
            if (IsEmpty())
            {
                if (date >= Begin && date + duration < End)
                {
                    planning.Add(date, duration);
                    return true;
                }
                else
                {
                    Console.WriteLine("La réunion est en dehors du planning");
                    return false;
                }
            }
            KeyValuePair<DateTime, DateTime> voisins = ClosestElements(date);
            try
            {
                if ((voisins.Key + planning[voisins.Key] < date) && (date + duration < voisins.Value))
                {
                    planning.Add(date, duration);
                    return true;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("La réunion est en dehors du planning");
            }
            return false;
        }

        public bool DeleteBusinessMeeting(DateTime date, TimeSpan duration)
        {
            try
            {
                planning.Remove(date);
            }
            catch (Exception)
            {

                Console.WriteLine("La réunion à supprimer n'est pas dans le planning");
                return false;
            }
            return true;
        }

        public int ClearMeetingPeriod(DateTime begin, DateTime end)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Pas de réunions programmées");
                return 0;
            }
            if (begin < Begin || end > End)
            {
                Console.WriteLine("Date de suppression en dehors du planning");
                return 0;
            }
            int count = 0;
            foreach (DateTime meeting in planning.Keys)
            {
                if ((meeting < begin && meeting + planning[meeting] > begin) || (meeting < end && meeting + planning[meeting] > end) || (meeting > begin && meeting < end))
                {
                    DeleteBusinessMeeting(meeting, planning[meeting]);
                    count += 1;
                }
            }
            return count;
        }

        public void DisplayMeetings()
        {
            Console.WriteLine($"Emploi du temps : {Begin} - {End}");
            Console.WriteLine("----------------------------------------------------------------");
            if (IsEmpty())
            {
                Console.WriteLine("Pas de réunions programmées");
                Console.WriteLine("----------------------------------------------------------------");
                return;
            }
            int count = 1;
            foreach (DateTime meeting in planning.Keys)
            {
                Console.WriteLine($"Réunion {count}       : {meeting} - {meeting + planning[meeting]}");
                count += 1;
            }
            Console.WriteLine("----------------------------------------------------------------");

        }
    }
}
