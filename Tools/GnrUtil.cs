#nullable disable
using System.Data.Common;
using System.Data;
using System.Reflection;
using XgenSQL2008.Connect;
using SauroGenerador.Plugin;
using Xgen.Plugin;

namespace Tools.Plugin;

internal class GnrUtil
{

    /// <summary>
    /// Convierte una cadena a UpperCamelCase
    /// </summary>
    /// <param name="value">Cadena de caracteres a convertir a camelcase</param>
    /// <returns>Retorna una cadena en formato CamelCase</returns>
    internal static string ToUpperCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value;
        }
        return char.ToUpper(value[0]) + value.Substring(1);
    }


    // crear funcion que agrege un slash al final del path si no existe
    internal static string AddSlash(string path)
    {
        if (!path.EndsWith("\\"))
        {
            path += "\\";
        }
        return path;
    }


    internal static string GetSingular(string plural)
    {
        string singular = string.Empty;

        if (plural.EndsWith("es"))
        {
            singular = plural.Substring(0, plural.Length - 2);
        }
        else if (plural.EndsWith("s"))
        {
            singular = plural.Substring(0, plural.Length - 1);
        }

        return (singular == "" ? plural : singular);
    }

    internal static void SaveFile(
            string filePath,
            string fileName,
            string content
        )
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        File.WriteAllText($"{filePath}{fileName}", content);
    }

}
