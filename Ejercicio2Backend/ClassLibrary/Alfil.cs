namespace ClassLibrary
{
    public class Alfil : IPieza
    {
        public char SimboloPieza { get; } = 'A';

        public bool PosicionEsValida(int fila, int col, int[] CoordenadasPiezas)
        {
            for (var i = 0; i < fila; i++)
            {
                // Verifica las dos diagonales
                if (Math.Abs(CoordenadasPiezas[i] - col) == Math.Abs(i - fila))
                    return false;
            }
            return true;
        }
    }
}
