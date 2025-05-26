using Xgen.Plugin;

namespace SauroGenerador.Plugin;

public static class TypeSQL
{
    public static string ToSQLType(this SQLType type)
    {
        return type switch
        {
            SQLType.SQLServer2008 => "SQL Server 2008",
            _ => "Desconocido"
        };
    }
}

public enum SQLType
{
    SQLServer2008,
}