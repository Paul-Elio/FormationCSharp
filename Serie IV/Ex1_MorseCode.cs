using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_IV
{
    public class Morse
    {
        private const string Taah = "===";
        private const string Ti = "=";
        private const string Point = ".";
        private const string PointLetter = "...";
        private const string PointWord = ".....";

        private readonly Dictionary<string, char> _alphabet;

        public Morse()
        {
            _alphabet = new Dictionary<string, char>()
            {
                {$"{Ti}.{Taah}", 'A'},
                {$"{Taah}.{Ti}.{Ti}.{Ti}", 'B'},
                {$"{Taah}.{Ti}.{Taah}.{Ti}", 'C'},
                {$"{Taah}.{Ti}.{Ti}", 'D'},
                {$"{Ti}", 'E'},
                {$"{Ti}.{Ti}.{Taah}.{Ti}", 'F'},
                {$"{Taah}.{Taah}.{Ti}", 'G'},
                {$"{Ti}.{Ti}.{Ti}.{Ti}", 'H'},
                {$"{Ti}.{Ti}", 'I'},
                {$"{Ti}.{Taah}.{Taah}.{Taah}", 'J'},
                {$"{Taah}.{Ti}.{Taah}", 'K'},
                {$"{Ti}.{Taah}.{Ti}.{Ti}", 'L'},
                {$"{Taah}.{Taah}", 'M'},
                {$"{Taah}.{Ti}", 'N'},
                {$"{Taah}.{Taah}.{Taah}", 'O'},
                {$"{Ti}.{Taah}.{Taah}.{Ti}", 'P'},
                {$"{Taah}.{Taah}.{Ti}.{Taah}", 'Q'},
                {$"{Ti}.{Taah}.{Ti}", 'R'},
                {$"{Ti}.{Ti}.{Ti}", 'S'},
                {$"{Taah}", 'T'},
                {$"{Ti}.{Ti}.{Taah}", 'U'},
                {$"{Ti}.{Ti}.{Ti}.{Taah}", 'V'},
                {$"{Ti}.{Taah}.{Taah}", 'W'},
                {$"{Taah}.{Ti}.{Ti}.{Taah}", 'X'},
                {$"{Taah}.{Ti}.{Taah}.{Taah}", 'Y'},
                {$"{Taah}.{Taah}.{Ti}.{Ti}", 'Z'},
            };
        }

        public int LettersCount(string code)
        {
            
            return code.Replace("...","$").Split('$').Length ;
        }

        public int WordsCount(string code)
        {
            return code.Replace(".....", "$").Split('$').Length;
        }

        public string MorseTranslation(string code)
        {
            string translation = "";
            foreach (string mot in code.Replace(".....","$").Split('$'))
            {
                foreach (string lettre in mot.Replace("...","$").Split('$'))
                {
                    try
                    {
                        translation += _alphabet[lettre];
                    }
                    catch (Exception)
                    {
                        translation += "+";
                    }
                    
                }
                translation += " ";
            }
            return translation ;
        }

        public string EfficientMorseTranslation(string code)
        {
            string morse = "";
            int ptsl = 0;
            foreach (char car in code)
            {

                if (car == '.')
                {
                    ptsl += 1;
                }
                else
                {
                    if (ptsl == 0)
                    {
                        morse += car;
                    }
                    else if (ptsl ==1 || ptsl ==2)
                    {
                        morse += "." + car;
                        ptsl = 0;

                    }
                    else if (ptsl == 3 || ptsl == 4)
                    {
                        morse += "..." + car;
                        ptsl = 0;

                    }
                    else if (ptsl >= 5)
                    {
                        morse += "....." +car;
                        ptsl = 0;

                    }
                }
            }
            return MorseTranslation(morse);
        }

        public string MorseEncryption(string sentence)
        {
            string morse = "";
            foreach (string mot in sentence.Split(' '))
            {
                foreach (char letter in mot)
                {
                    morse += GetMorseKey(letter);
                    morse += "...";
                }
                morse += "..";
            }
            return morse.Substring(0,morse.Length-5);
        }

        public string GetMorseKey(char letter)
        {
            if (_alphabet.ContainsValue(letter))
            {
                foreach (string symbole in _alphabet.Keys)
                {
                    if (_alphabet[symbole] == letter)
                    {
                        return symbole;
                    }
                }
            }
            return ("+");
        }
    }
}
