using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Percolation
{
    public class Percolation
    {
        private readonly bool[,] _open;
        private readonly bool[,] _full;
        private readonly int _size;
        private bool _percolate;

        public Percolation(int size)
        {
            if (size <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Taille de la grille négative ou nulle.");
            }

            _open = new bool[size, size];
            _full = new bool[size, size];
            _size = size;
        }

        public bool IsOpen(int i, int j)
        {
            return _open[i,j];
        }

        private bool IsFull(int i, int j)
        {
            return _full[i,j];
        }

        // on regarde si la dernière ligne du tableau contien une case d'eau
        public bool Percolate()
        {
            for (int j=0; j<_size; j++)
            {
                if (_full[_size - 1, j])
                {
                    return true;
                }
            }
            return false;
        }

        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = new List<KeyValuePair<int, int>>();
            /** version bourrine non optimale
            if (i>0 && j>0 && i<_size-1 && j < _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
                voisins.Add(new KeyValuePair<int, int>(i , j+1));
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
                voisins.Add(new KeyValuePair<int, int>(i , j-1));
            }
            else if (i==0 && j> 0 && j<_size-1)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
            }
            else if (i == _size-1 && j > 0 && j < _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
            }
            else if (j == 0 && i > 0 && i < _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            else if (j == _size - 1 && i > 0 && i < _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            else if (j==0 && i == 0)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            else if (j == 0 && i == _size-1)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
            }
            else if (j == _size-1 && i == 0)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            else if (j == _size - 1 && i == _size - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
            }
            else
            {
                Console.WriteLine("on ne devrait pas être là");
            }
            /**/
            if (!(i==0))
            {
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
            }
            if (!(i == _size-1))
            {
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            if (!(j == 0))
            {
                voisins.Add(new KeyValuePair<int, int>(i, j-1));
            }
            if (!(j == _size - 1))
            {
                voisins.Add(new KeyValuePair<int, int>(i, j+1));
            }
            return voisins;
        }

        public void Open(int i, int j)
        {
            _open[i, j] = true;
            List<KeyValuePair<int, int>> voisins = CloseNeighbors(i, j);
            if (i == 0)
            {
                _full[i, j] = true;
            }
            else
            {
                foreach (KeyValuePair<int, int> cell in voisins)
                {
                    if (IsFull(cell.Key, cell.Value))
                    {
                        _full[i, j] = true;
                        break;
                    }
                }
            }
            if (_full[i, j])
            {
                foreach (KeyValuePair<int, int> cell in voisins)
                {
                    if (IsOpen(cell.Key, cell.Value) && !IsFull(cell.Key, cell.Value))
                    {
                        Open(cell.Key, cell.Value);
                    }
                }
            }
            return;
        }
    }
}
