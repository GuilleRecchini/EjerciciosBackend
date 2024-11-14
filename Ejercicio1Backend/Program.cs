var N = 8;
var tablero = new int[N];

while (true)
{
    Console.Write($"Ingrese la columna en la que colocará la reina en la primera fila (1-{N}): ");
    if (int.TryParse(Console.ReadLine(), out var col) && col > 0 && col <= N)
    {
        tablero[0] = col - 1;
        break;
    }
    Console.WriteLine($"Entrada no válida. Debes ingresar un número entre 1 y {N}. Inténtalo de nuevo:");
}

if (!ResolverReinas(1))
{
    Console.WriteLine("No hay solución");
}

bool EsValida(int fila, int col)
{
    for (var i = 0; i < fila; i++)
    {
        // Verifica la columna
        if (tablero[i] == col)
            return false;

        // Verifica las dos diagonales
        if (Math.Abs(tablero[i] - col) == Math.Abs(i - fila))
            return false;
    }
    return true;
}

bool ResolverReinas(int fila)
{
    if (fila == N)
    {
        ImprimirTablero();
        return true;
    }

    for (var col = 0; col < N; col++)
    {
        if (EsValida(fila, col))
        {
            tablero[fila] = col; // Colocar reina
            if (ResolverReinas(fila + 1))
                return true;
            tablero[fila] = -1; // Retroceder
        }
    }

    return false;
}

void ImprimirTablero()
{
    for (var i = 0; i < N; i++)
    {
        for (var j = 0; j < N; j++)
        {
            Console.Write(tablero[i] == j ? "Q " : ". ");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}
