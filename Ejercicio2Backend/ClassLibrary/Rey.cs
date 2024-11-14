namespace ClassLibrary
{
    public class Rey : IPieza
    {
        public char SimboloPieza { get; } = 'K';

        public bool PosicionEsValida(int fila, int col, int[] CoordenadasPiezas)
        {
            for (var i = 0; i < fila; i++)
            {
                int colPieza = CoordenadasPiezas[i];
                int filaPieza = i;

                // Verifica si el rey se mueve una casilla en cualquier dirección
                if (Math.Abs(colPieza - col) <= 1 && Math.Abs(filaPieza - fila) <= 1)
                    return false;
            }
            return true;

        }
    }
}
