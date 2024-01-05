using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serie_IV
{
    public static class BracketsControl
    {
        public static bool BracketsControls(string sentence)
        {
            Stack<char> par = new Stack<char>();
            foreach (char letter in sentence)
            {
                if (letter == '(' || letter == '{' || letter == '[')
                {
                    par.Push(letter);
                }
                else if (letter == ')' || letter == '}' || letter == ']')
                {
                    switch (letter)
                    {
                        case ')':
                            if (!(par.Pop() == '('))
                            {
                                return false;
                            }
                            break;
                        case ']':
                            if (!(par.Pop() == '['))
                            {
                                return false;
                            }
                            break;
                        case '}':
                            if (!(par.Pop() == '{'))
                            {
                                return false;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
            return true;
        }
    }
}
