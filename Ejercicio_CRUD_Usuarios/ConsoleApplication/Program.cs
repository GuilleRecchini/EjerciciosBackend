using DbConnection.DAOs;
using DbConnection.Entities;

Console.WriteLine("Inicio del Programa");

var Db = new UsuarioDao();
var continuar = true;

while (continuar)
{
    Console.WriteLine("\nSeleccione una opción:");
    Console.WriteLine("1. Crear Usuario");
    Console.WriteLine("2. Listado de Usuarios");
    Console.WriteLine("3. Actualizar Usuario");
    Console.WriteLine("4. Eliminar Usuario");
    Console.WriteLine("5. Salir");
    var opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            CrearUsuario();
            break;
        case "2":
            LeerUsuarios();
            break;
        case "3":
            ActualizarUsuario();
            break;
        case "4":
            EliminarUsuario();
            break;
        case "5":
            continuar = false;
            break;
        default:
            Console.WriteLine("Opción no válida.");
            break;
    }
}

void CrearUsuario()
{
    Console.Write("Ingrese DNI: ");
    var dni = int.Parse(Console.ReadLine());

    Console.Write("Ingrese Nombre: ");
    var nombre = Console.ReadLine();

    Console.Write("Ingrese Edad: ");
    var edad = int.Parse(Console.ReadLine());

    var UsuarioCreado = Db.InsertUsuario(new Usuario { Dni = dni, Nombre = nombre, Edad = edad });

    if (UsuarioCreado)
        Console.WriteLine("Usuario creado correctamente.");
    else
        Console.WriteLine("Error al crear el Usuario.");
}

void LeerUsuarios()
{
    var usuarios = Db.GetUsuariosActivos();

    Console.WriteLine("\nListado de Usuarios:");

    Console.WriteLine($"{"DNI",-10}{"Nombre",-20}{"Edad",-10}");
    Console.WriteLine(new string('-', 40));

    foreach (var usuario in usuarios)
    {
        Console.WriteLine($"{usuario.Dni,-10}{usuario.Nombre,-20}{usuario.Edad,-10}");
    }
}

void ActualizarUsuario()
{
    Console.Write("Ingrese el DNI del usuario a actualizar: ");

    var dni = int.Parse(Console.ReadLine());

    var usuario = Db.GetUsuario(dni);

    if (usuario != null)
    {
        Console.Write("Ingrese nuevo Nombre: ");
        usuario.Nombre = Console.ReadLine();

        Console.Write("Ingrese nueva Edad: ");
        usuario.Edad = int.Parse(Console.ReadLine());

        var UsuarioActualizado = Db.UpdateUsuario(usuario);

        if (UsuarioActualizado)
            Console.WriteLine("Usuario actualizado correctamente.");
        else
            Console.WriteLine("Error al actualizar el Usuario.");
    }
    else
    {
        Console.WriteLine("Usuario no encontrado.");
    }
}

void EliminarUsuario()
{
    Console.Write("Ingrese el DNI del usuario a eliminar: ");

    var dni = int.Parse(Console.ReadLine());

    var usuario = Db.GetUsuario(dni);

    if (usuario == null)
    {
        Console.WriteLine("Usuario no encontrado.");
        return;
    }

    if (usuario.Eliminado)
    {
        Console.WriteLine("El Usuario ha sido eliminado anteriormente");
        return;
    }

    var UsuarioEliminado = Db.DeleteUsuario(dni);

    if (UsuarioEliminado)
        Console.WriteLine("Usuario eliminado correctamente.");
    else
        Console.WriteLine("Error al eliminar el Usuario.");
}