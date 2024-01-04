using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_III
{
    public static class ClassCouncil
    {
        public static void SchoolMeans(string input, string output)
        {
            string[] classe = File.ReadAllLines(input);
            Dictionary<string, float[]> result = new Dictionary<string, float[]>();

            for (int i =0; i < classe.Length; i++)
            {
                string[] ligne = classe[i].Split(';');
                float note;
                float.TryParse(ligne[2].Replace('.',','), out note);

                if (!result.ContainsKey(ligne[1]))
                {
                    float[] tab = new float[] { note, 1 };
                    result.Add(ligne[1], tab);
                }
                else
                {
                    result[ligne[1]][0] +=  note;
                    result[ligne[1]][1] += 1;
                }
            }
            List<string> resultFile = new List<string>();
            foreach (string matiere in result.Keys) 
            {
                float moyenne = result[matiere][0] / result[matiere][1];
                resultFile.Add($"{matiere};{moyenne}");
            }
            File.WriteAllLines(output, resultFile.ToArray());
        }
    }
}
