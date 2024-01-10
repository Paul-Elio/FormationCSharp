namespace Labyrinth
{
    public struct Cell
    {
        // 0 : Haut, 1 : bas, 2 : gauche, 3 : droite
        public bool[] Walls { get; set; }

        public bool IsVisited { get; set; }

        public int Statut { get; set; }
        // 0 : Start, 1 : Lambda, 2 : End
    }
}
