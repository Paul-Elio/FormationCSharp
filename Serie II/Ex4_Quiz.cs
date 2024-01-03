using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_II
{
    public struct Qcm
    {
        public string Question;
        public string[] Answers;
        public int Solution;
        public int Weight;
    }

    public static class Quiz
    {
        public static void AskQuestions(Qcm[] qcms)
        {
            int sum = 0;
            int tot = 0;
            for (int i = 0; i < qcms.Length; i++)
            {
                if (QcmValidity(qcms[i]))
                {
                    sum += AskQuestion(qcms[i]);
                    tot += qcms[i].Weight;
                    Console.WriteLine();
                }

            }
            Console.WriteLine($"Résultat du questionnaire : {sum}/{tot}");
        }

        public static int AskQuestion(Qcm qcm)
        {
            string input;
            Console.WriteLine(qcm.Question);
            for (int i = 1; i<= qcm.Answers.Length; i++)
            {
                Console.Write($"{i}. {qcm.Answers[i-1]} ");
            }
            Console.WriteLine();
            bool rep = false;
            int number;
            while(!rep)
            {
                do
                {
                    Console.WriteLine("Réponse : ");
                    input = Console.ReadLine();

                } while (!int.TryParse(input, out number));
                if ((number-1) == qcm.Solution)
                {
                    return qcm.Weight;
                }
                else
                {
                    Console.WriteLine("Réponse invalide !");
                }
            }           
            return -1;
        }

        public static bool QcmValidity(Qcm qcm)
        {
            if ((0<=qcm.Solution) && (qcm.Solution < qcm.Answers.Length) && (qcm.Weight >= 0))
            {
                return true;
            }
            return false;
        }
    }
}
