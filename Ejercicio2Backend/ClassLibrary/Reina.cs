namespace ClassLibrary
{
    public class Reina : IPieza
    {
        public char SimboloPieza { get; } = 'Q';

        public bool PosicionEsValida(int fila, int col, int[] CoordenadasPiezas)
        {
            for (var i = 0; i < fila; i++)
            {
                // Verifica la columna
                if (CoordenadasPiezas[i] == col)
                    return false;

                // Verifica las dos diagonales
                if (Math.Abs(CoordenadasPiezas[i] - col) == Math.Abs(i - fila))
                    return false;
            }
            return true;
        }
    }
}
