using ClassLibrary;

var tablero = new Tablero(new Rey());

if (!tablero.Resolver8Piezas())
{
    Console.WriteLine("No hay solución");
}