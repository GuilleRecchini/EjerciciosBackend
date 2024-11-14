namespace ClassLibrary
{
    public class Tablero
    {
        public IPieza Pieza { get; set; }
        public int CantidadDePiezas { get; } = 8;
        public int[] CoordenadasPiezas { get; set; }

        public Tablero(IPieza pieza)
        {
            Pieza = pieza;
            CoordenadasPiezas = new int[CantidadDePiezas];
        }
        public bool Resolver8Piezas(int fila = 0)
        {
            if (fila == CantidadDePiezas)
            {
                ImprimirTablero();
                return true;
            }

            for (var col = 0; col < CantidadDePiezas; col++)
            {
                if (Pieza.PosicionEsValida(fila, col, CoordenadasPiezas))
                {
                    CoordenadasPiezas[fila] = col; // Colocar pieza
                    if (Resolver8Piezas(fila + 1))
                        return true;
                    CoordenadasPiezas[fila] = -1; // Retroceder
                }
            }

            return false;
        }

        void ImprimirTablero()
        {
            for (var i = 0; i < CantidadDePiezas; i++)
            {
                for (var j = 0; j < CantidadDePiezas; j++)
                {
                    Console.Write(CoordenadasPiezas[i] == j ? Pieza.SimboloPieza + " " : ". ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
