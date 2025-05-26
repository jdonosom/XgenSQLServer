using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XgenSQL2008.Connect;


/// <summary>
/// Representa un error de acceso a la base de datos.
/// </summary>
public class DatabaseException : ApplicationException
{

    /// <summary>
    /// Construye una instancia en base a un mensaje de error y la una excepción original.
    /// </summary>
    /// <param name="mensaje">El mensaje de error.</param>
    /// <param name="original">La excepción original.</param>
    public DatabaseException(string mensaje, Exception original) : base(mensaje, original) { }

    /// <summary>
    /// Construye una instancia en base a un mensaje de error.
    /// </summary>
    /// <param name="mensaje">El mensaje de error.</param>
    public DatabaseException(string mensaje) : base(mensaje) { }
}
