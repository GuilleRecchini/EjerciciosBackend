namespace ClassLibrary
{
    public interface IPieza
    {
        public char SimboloPieza { get; }
        public bool PosicionEsValida(int fila, int col, int[] CoordenadasPiezas);
    }
}
