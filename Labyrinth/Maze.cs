using System;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth
{
    public class Maze
    {
        /// <summary>
        /// Grille permettant de représenter un matériau poreux
        /// Pour chaque élément, true case ouverte, false case bloquée
        /// </summary>
        private readonly Cell[,] _maze;

        private readonly int _lineSize;

        private readonly int _columnSize;

        /// <summary>
        /// Construction d'une grille de taille n * m
        /// </summary>
        /// <param name="size"></param>
        public Maze(int n, int m)
        {
            if (n <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "le nombre de lignes de la grille négatif ou null.");
            }

            if (m <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n), n, "le nombre de colonnes de la grille négatif ou null.");
            }

            _lineSize = n;
            _columnSize = m;
            _maze = new Cell[n, m];
        }
        public bool IsOpen(int i, int j, int w)
        {
            return _maze[i, j].Walls[w];
        }
        public bool IsMazeStart(int i, int j)
        {
            return _maze[i, j].Statut == 0;
        }
        public bool IsMazeEnd(int i, int j)
        {
            return _maze[i, j].Statut == 2;
        }
        public void Open(int i, int j, int w)
        {
            if (!IsOpen(i, j, w))
            {
                _maze[i, j].Walls[w] = true;
                switch (w)
                {
                    case 0:
                        Open(i - 1, j, 1);
                        break;
                    case 1:
                        Open(i + 1, j, 0);
                        break;
                    case 2:
                        Open(i, j - 1, 3);
                        break;
                    case 3:
                        Open(i, j + 1, 2);
                        break;
                    default:
                        throw new Exception("erreur de voisinnage");
                }
            }
            return;
        }
        private List<KeyValuePair<int, int>> CloseNeighbors(int i, int j)
        {
            List<KeyValuePair<int, int>> voisins = new List<KeyValuePair<int, int>>();
            if (i > 0)
            {
                voisins.Add(new KeyValuePair<int, int>(i - 1, j));
            }
            if (i < _lineSize - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i + 1, j));
            }
            if (j > 0)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j - 1));
            }
            if (j < _columnSize - 1)
            {
                voisins.Add(new KeyValuePair<int, int>(i, j + 1));
            }
            return voisins;
        }
        private void Init_maze()
        {
            for (int i = 0; i < _lineSize; i++)
            {
                for (int j = 0; j < _columnSize; j++)
                {
                    _maze[i, j].IsVisited = false;
                    _maze[i, j].Walls = new bool[] { false, false, false, false };
                }
            }
        }
        private int Determine_mur(int i, int j, int vi, int vj)
        {
            if(j - vj == 0)
            {
                return i - vi == 1 ? 0 : 1;
            }
            else 
            {
                return j - vj == 1 ? 2 : 3;
            }
        }

        public KeyValuePair<int, int> Generate()
        {
            Init_maze();
            var rand = new Random();
            int i = rand.Next(0, _lineSize);
            int j = rand.Next(0, _columnSize);
            _maze[i, j].IsVisited = true;
            var voisins = CloseNeighbors(i, j);
            var voisins_non_visites = voisins.Where(x => !_maze[x.Key, x.Value].IsVisited);
            // il reste à prendre un voisin au hazard et toute la suite aussi 
            Stack<Cell> pile = new Stack<Cell>();
            return new KeyValuePair<int, int>();
        }

        public string DisplayLine(int n)
        {
            return string.Empty;
        }

        public List<string> Display(int n)
        {
            return new List<string>();
        }
    }
}
