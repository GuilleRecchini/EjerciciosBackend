namespace ClassLibrary
{
    public class Torre : IPieza
    {
        public char SimboloPieza { get; } = 'T';

        public bool PosicionEsValida(int fila, int col, int[] CoordenadasPiezas)
        {
            for (var i = 0; i < fila; i++)
            {
                // Verifica la columna
                if (CoordenadasPiezas[i] == col)
                    return false;
            }
            return true;
        }
    }
}
