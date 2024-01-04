using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_III
{
    public struct SortData
    {
        /// <summary>
        /// Moyenne pour le tri par insertion
        /// </summary>
        public long InsertionMean { get; set; }
        /// <summary>
        /// Écart-type pour le tri par insertion
        /// </summary>
        public long InsertionStd { get; set; }
        /// <summary>
        /// Moyenne pour le tri rapide
        /// </summary>
        public long QuickMean { get; set; }
        /// <summary>
        /// Écart-type pour le tri rapide
        /// </summary>
        public long QuickStd { get; set; }
    }

    public static class SortingPerformance
    {
        public static void DisplayPerformances(List<int> sizes, int count)
        {
            Console.WriteLine("n;MeanInsertion;StdInsertion;MeanQuick;StdQuick");
            List<SortData> performances = PerformancesTest(sizes, count);
            for (int i =0; i < performances.Count; i++)
            {
                Console.WriteLine($"{sizes[i]};{performances[i].InsertionMean};{performances[i].InsertionStd};{performances[i].QuickMean};{performances[i].QuickStd}");
            }
        }

        public static List<SortData> PerformancesTest(List<int> sizes, int count)
        {
            List<SortData> performances = new List<SortData>();
            for (int i = 0; i < sizes.Count; i++)
            {
                performances.Add(PerformanceTest(sizes[i], count));
            }
            return performances;
        }

        public static SortData PerformanceTest(int size, int count)
        {
            long sum_inser = 0;
            long sum_quick = 0;
            long sum_inser_squar = 0;
            long sum_quick_squar = 0;
            for (int i =0; i < count; i++)
            {
                List<int[]> arrays = ArraysGenerator(size);
                long inser = UseInsertionSort(arrays[0]);
                long quick = UseQuickSort(arrays[1]);
                sum_inser += inser ;
                sum_quick += quick;
                sum_inser_squar += inser*inser; 
                sum_quick_squar += quick*quick ;
            }
            SortData sd = new SortData();
            long inser_mean = sum_inser / count;
            long quick_mean = sum_quick / count;

            sd.InsertionMean = inser_mean;
            sd.QuickMean = quick_mean;
            sd.InsertionStd = (long)Math.Sqrt((sum_inser_squar / count) - inser_mean * inser_mean);
            sd.QuickStd = (long)Math.Sqrt((sum_quick_squar / count) - quick_mean * quick_mean);
            return sd;
        }

        private static List<int[]> ArraysGenerator(int size)
        {
            int[] tab1 = new int[size];
            int[] tab2 = new int[size];
            var ran = new Random();
            for (int i=0; i < size; i++)
            {
                int number = ran.Next(-1000, 1001);
                tab1[i] = number;
                tab2[i] = number;
            }
            List<int[]> arrays = new List<int[]>();
            arrays.Add(tab1);
            arrays.Add(tab2);
            return arrays ;
        }

        public static long UseInsertionSort(int[] array)
        {
            Stopwatch sw = Stopwatch.StartNew();
            InsertionSort(array);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        public static long UseQuickSort(int[] array)
        {
            Stopwatch sw = Stopwatch.StartNew();
            QuickSort(array,0, array.Length -1);
            sw.Stop();
            return sw.ElapsedMilliseconds;
        }

        private static void InsertionSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (array[j - 1] > array[j])
                    {
                        int tmp = array[j - 1];
                        array[j - 1] = array[j];
                        array[j] = tmp;
                    }
                }
            };
        }

        private static void QuickSort(int[] array, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(array, left, right);
                QuickSort(array, left, pivot - 1);
                QuickSort(array, pivot + 1, right);
            }
        }

        private static int Partition(int[] array, int left, int right)
        {
            int pivot = array[right];
            int i = left;
            for (int j = left; j < right; j++)
            {
                if (array[j] < pivot)
                {
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                    i++;
                }
            }
            int tmp = array[i];
            array[i] = array[right];
            array[right] = tmp;
            return i;
        }
    }
}
