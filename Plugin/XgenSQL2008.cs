#nullable disable

using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Text;
using Xgen.Plugin;
using System.Drawing;

using XauroCommon.Interface;
using System.Configuration;
using System;

namespace SauroGenerador.Plugin;

public class XgenSQL2008 : ISqlGeneratorPlugin
{
    #region Variables  para colores del Plugins
    #endregion

    private Colors _colors;
    private IDatabase _database;
    public IDatabase Database
    {
        get => _database;
        set => _database = value;
    }

    public string Tabs => new string(' ', 4);
    public Image Icon { get; set; }
    public Dictionary<string, string> DataTypes { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, string> DataTypesDb { get; set; } = new Dictionary<string, string>();
    private Dictionary<string, string> TypesValueDb { get; set; } = new Dictionary<string, string>();
    // private Dictionary<string, string> DataTypesParam { get; set; } = new Dictionary<string, string>();
    public List<Table> Tables { get; set; } = new List<Table>(); // Changed from private set to public set to implement the interface
    public string SQLType => "SQLServer2008";
    public string DatabaseNameDefault => "master";
    public Colors Colors => _colors;
    public XgenSQL2008() { this.Inicialize(); }
    public XgenSQL2008(
            string tables
        )
    {

        this.Inicialize();
        LoadTables(tables);

    }

    public void Inicialize()
    {
        FillTypesValueDb();
        FillTypesDb();

        #region Leer configuracion de colores para el Plugin
        // Mapeo personalizado
        //
        string rutaConfig = AppDomain.CurrentDomain.BaseDirectory + @"Components\XgenSQL2008.dll.config";

        ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = rutaConfig
        };

        _colors = new Colors();
        // Cargar la configuración desde ese archivo
        //
        Configuration appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

        _colors.cmdSQLComments = "";
        if (appConfig.AppSettings.Settings["cmdSQLComments"] != null)
            _colors.cmdSQLComments = appConfig.AppSettings.Settings["cmdSQLComments"].Value ?? "";

        _colors.cmdSQLCommentsColor = "";
        if (appConfig.AppSettings.Settings["cmdSQLCommentsColor"] != null)
            _colors.cmdSQLCommentsColor = appConfig.AppSettings.Settings["cmdSQLCommentsColor"].Value ?? "";

        _colors.cmdSQLCommentsBloq = "";
        if (appConfig.AppSettings.Settings["cmdSQLCommentsBloq"] != null)
            _colors.cmdSQLCommentsBloq = appConfig.AppSettings.Settings["cmdSQLCommentsBloq"].Value ?? "";

        _colors.cmdSQLCommentsBloqColor = "";
        if (appConfig.AppSettings.Settings["cmdSQLCommentsBloqColor"] != null)
            _colors.cmdSQLCommentsBloqColor = appConfig.AppSettings.Settings["cmdSQLCommentsBloqColor"].Value ?? "";

        _colors.cmdSQLString = "";
        if (appConfig.AppSettings.Settings["cmdSQLString"] != null)
            _colors.cmdSQLString = appConfig.AppSettings.Settings["cmdSQLString"].Value ?? "";

        _colors.cmdSQLStringColor = "";
        if (appConfig.AppSettings.Settings["cmdSQLStringColor"] != null)
            _colors.cmdSQLStringColor = appConfig.AppSettings.Settings["cmdSQLStringColor"].Value ?? "";

        _colors.cmdSQLKey1 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey1"] != null)
            _colors.cmdSQLKey1 = appConfig.AppSettings.Settings["cmdSQLKey1"].Value ?? "";

        _colors.cmdSQLKey1Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey1Color"] != null)
            _colors.cmdSQLKey1Color = appConfig.AppSettings.Settings["cmdSQLKey1Color"].Value ?? "";

        _colors.cmdSQLKey2 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey2"] != null)
            _colors.cmdSQLKey2 = appConfig.AppSettings.Settings["cmdSQLKey2"].Value ?? "";

        _colors.cmdSQLKey2Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey2Color"] != null)
            _colors.cmdSQLKey2Color = appConfig.AppSettings.Settings["cmdSQLKey2Color"].Value ?? "";

        _colors.cmdSQLKey3 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey3"] != null)
            _colors.cmdSQLKey3 = appConfig.AppSettings.Settings["cmdSQLKey3"].Value ?? "";

        _colors.cmdSQLKey3Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey3Color"] != null)
            _colors.cmdSQLKey3Color = appConfig.AppSettings.Settings["cmdSQLKey3Color"].Value ?? "";

        _colors.cmdSQLKey4 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey4"] != null)
            _colors.cmdSQLKey4 = appConfig.AppSettings.Settings["cmdSQLKey4"].Value ?? "";

        _colors.cmdSQLKey4Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey4Color"] != null)
            _colors.cmdSQLKey4Color = appConfig.AppSettings.Settings["cmdSQLKey4Color"].Value ?? "";

        _colors.cmdSQLKey5 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey5"] != null)
            _colors.cmdSQLKey5 = appConfig.AppSettings.Settings["cmdSQLKey5"].Value ?? "";

        _colors.cmdSQLKey5Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey5Color"] != null)
            _colors.cmdSQLKey5Color = appConfig.AppSettings.Settings["cmdSQLKey5Color"].Value ?? "";

        _colors.cmdSQLKey6 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey6"] != null)
            _colors.cmdSQLKey6 = appConfig.AppSettings.Settings["cmdSQLKey6"].Value ?? "";

        _colors.cmdSQLKey6Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey6Color"] != null)
            _colors.cmdSQLKey6Color = appConfig.AppSettings.Settings["cmdSQLKey6Color"].Value ?? "";

        _colors.cmdSQLKey7 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey7"] != null)
            _colors.cmdSQLKey7 = appConfig.AppSettings.Settings["cmdSQLKey7"].Value ?? "";

        _colors.cmdSQLKey7Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey7Color"] != null)
            _colors.cmdSQLKey7Color = appConfig.AppSettings.Settings["cmdSQLKey7Color"].Value ?? "";

        _colors.cmdSQLKey8 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey8"] != null)
            _colors.cmdSQLKey8 = appConfig.AppSettings.Settings["cmdSQLKey8"].Value ?? "";

        _colors.cmdSQLKey8Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey8Color"] != null)
            _colors.cmdSQLKey8Color = appConfig.AppSettings.Settings["cmdSQLKey8Color"].Value ?? "";

        _colors.cmdSQLKey9 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey9"] != null)
            _colors.cmdSQLKey9 = appConfig.AppSettings.Settings["cmdSQLKey9"].Value ?? "";

        _colors.cmdSQLKey9Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey9Color"] != null)
            _colors.cmdSQLKey9Color = appConfig.AppSettings.Settings["cmdSQLKey9Color"].Value ?? "";

        _colors.cmdSQLKey10 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey10"] != null)
            _colors.cmdSQLKey10 = appConfig.AppSettings.Settings["cmdSQLKey10"].Value ?? "";

        _colors.cmdSQLKey10Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey10Color"] != null)
            _colors.cmdSQLKey10Color = appConfig.AppSettings.Settings["cmdSQLKey10Color"].Value ?? "";
        #endregion

    }

    /// <summary>
    /// Obtiene la lista de tablas de la base de datos configurada en la instancia de Database.
    /// </summary>
    /// <returns>Retorna una lista <see cref="List{string}"/> con los nombres de tablas</returns>
    public List<string> GetListTables()
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<string> tables = new List<string>();
        try
        {
            if (!Database.Conectar())
                return null;

            string sSql = Database.ListTablesCommand(Database.DatabaseName);
            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            if (dr == null)
                return null;

            while (dr.Read())
            {
                string name = dr.IsDBNull(dr.GetOrdinal("TABLE_NAME")) ? "" : dr.GetString(dr.GetOrdinal("TABLE_NAME"));
                tables.Add(name);
            }
            Database.Desconectar();
        }
        catch (Exception)
        {

            return tables;
        }
        return tables;
    }


    /// <summary>
    /// Setea los parametros indices para pasar a las funciones
    /// </summary>
    /// <param name="fields">Lista de campos</param>
    /// <returns>Retorna una cadena con los parametros de entrada</returns>
    public string SetParamProcIn(
            List<FieldList> fields
        )
    {
        // <Type0> <nameField0>, <Type1> <nameField1>, ...
        string asignacionParametrosSP = string.Empty;
        fields.ForEach(x =>
        {
            if (x.esClavePrimaria)
                // asignacionParametrosSP += $"{x.nombreCampo.ToLower()}, ";
                asignacionParametrosSP += $"{x.nombreCampo}, ";
        });
        // Param0, Param1, ...
        return asignacionParametrosSP.Substring(0, asignacionParametrosSP.Length - 2);
    }


    /// <summary>
    /// Carga las tablas y sus campos en la lista de tablas.
    /// </summary>
    /// <param name="tables">Lista de tablas separados por comas.</param>
    /// <exception cref="Exception">Genera excepción de tipo Exception</exception>
    public void LoadTables(
            string tables
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<Entity2> entity = new List<Entity2>();
        string index_keys = string.Empty;

        var _tables = new List<Table>();
        string[] tablasArray = tables.Split(',');

        foreach (string tabla in tablasArray)
        {
            Table t = new Table();
            t.nombreTabla = tabla;
            t.Campos = new List<FieldList>();

            try
            {
                index_keys = GetConstraintKeys(tabla);
                entity = GetEntity(tabla);
                entity.ForEach(x =>
                {
                    t.Campos.Add(new FieldList()
                    {
                        nombreCampo = x.Field.ToString(),
                        tipoCampo = ConvertType(x.Type.ToString()),
                        esNulo = ((string)x.IsNullable) == "yes",
                        esIdentidad = (bool)x.IsIdentity,
                        esClavePrimaria = ((bool)x.PrimaryKey),
                        esClaveForanea = false,
                        tablaRelacion = "",
                        campoRelacion = "",
                        defaultValue = SetDefaultValue(x.Type.ToString())
                    });
                });
                _tables.Add(t);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        Tables = _tables;

    }

    /// <summary>
    /// Convierte un tipo de dato de la base de datos a un tipo de dato utilizado en el código.
    /// </summary>
    /// <param name="type">Tipo de datos</param>
    /// <returns>Retorna el tipo de datos equivalente</returns>
    public string ConvertType(
            string type
        )
    {
        FillTypesDb();

        string _tipo = type.ToUpper();
        string tipoConvertido = string.Empty;

        if (DataTypes.ContainsKey(_tipo))
        {
            tipoConvertido = DataTypes[_tipo];
        }
        return tipoConvertido;
    }


    /// <summary>
    /// Genera el script de pie de página para un procedimiento almacenado.
    /// </summary>
    /// <param name="nsp"></param>
    /// <returns></returns>
    public string FooterSpMethod(
            string nsp
        )
    {
        string sBuffer = "";
        sBuffer += "IF OBJECT_ID('" + nsp + "') IS NOT NULL" + Environment.NewLine;
        sBuffer += "\t PRINT '<<< PROCEDIMIENTO " + nsp + " CREADO >>>'" + Environment.NewLine;
        sBuffer += "ELSE" + Environment.NewLine;
        sBuffer += "\t PRINT '<<< HA FALLADO LA CREACION DEL PROCEDIMIENTO " + nsp + " >>>'" + Environment.NewLine;
        sBuffer += "GO" + Environment.NewLine;
        return sBuffer;
    }


    /// <summary>
    /// Genera los procedimientos almacenados para las tablas especificadas.
    /// </summary>
    /// <param name="tables"></param>
    /// <param name="nsp"></param>
    /// <param name="pk"></param>
    /// <param name="pathOut"></param>
    /// <param name="projectName"></param>
    /// <param name="SQLType"></param>
    /// <param name="dataBaseName"></param>
    public void GenerarProcedimientos(
            List<Table> tables,
            string nsp,
            string pk,
            string pathOut,
            string projectName,
            string SQLType,
            string dataBaseName
        )
    {

        string pathDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string pathProyecto = GnrUtil.AddSlash($"{pathOut}");

        tables.ForEach(x =>
        {
            x.BaseDatos = dataBaseName;
            var selProc = GenerateSelectProc(x, SQLType);
            var updProc = GenerateUpdateProc(x, SQLType);
            var delProc = GenerateDeleteProc(x, SQLType);

            GnrUtil.SaveFile(pathProyecto, $"{x.nombreTabla}SelProc.sql", selProc);
            GnrUtil.SaveFile(pathProyecto, $"{x.nombreTabla}UpdProc.sql", updProc);
            GnrUtil.SaveFile(pathProyecto, $"{x.nombreTabla}DelProc.sql", delProc);

        });

    }

    /// <summary>
    /// Genera un procedimiento almacenado para eliminar registros de una tabla específica.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public string GenerateDeleteProc(
            Table table,
            string version
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamWhereSp = null;

        // Obtener los campos de la tabla
        List<Entity2> camposDB = GetEntity(table.nombreTabla);

        // Convertir el nombre de la tabla a UpperCamelCase
        var model = GnrUtil.ToUpperCamelCase(table.nombreTabla);

        string sParamSpSet = GetInputParamSpSet(camposDB, model);
        string spParamListField = GetListFieldSP(camposDB, model);

        // Generar los parámetros de entrada y la cláusula WHERE
        sParamSpPK = GetInputParamSpPK(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);

        // Nombre del procedimiento almacenado
        SP = $"dbo.{model}DelProc";

        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, version.ToString()));
        sb.AppendLine("USE " + table.BaseDatos);
        sb.AppendLine("GO");

        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine(")");
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}SET NOCOUNT ON");
        sb.AppendLine($"{Tabs}BEGIN TRANSACTION");
        sb.AppendLine($"{Tabs}DELETE FROM {model}");

        int nUltimoElemento = camposDB.Count - 1;
        string sFieldFirst = camposDB[0].Field.ToString();
        string sFieldLast = camposDB[nUltimoElemento].Field.ToString();

        if (sParamWhereSp != null)
        {
            sb.AppendLine($"{new string(' ', 5)}WHERE{sParamWhereSp}");
        }
        sb.AppendLine($"{Tabs}IF(@@Error != 0)");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}{GetRaisError(SQLType, 16, 1, model, SP)}");
        sb.AppendLine($"{Tabs}{Tabs}ROLLBACK TRANSACTION");
        sb.AppendLine($"{Tabs}{Tabs}RETURN(1)");
        sb.AppendLine($"{Tabs}END");
        sb.AppendLine($"{Tabs}COMMIT TRANSACTION");
        sb.AppendLine($"{Tabs}SET NOCOUNT OFF");
        sb.AppendLine($"{Tabs}RETURN(0)");
        sb.AppendLine("END");
        sb.AppendLine("GO");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));
        return sb.ToString();
    }


    /// <summary>
    /// Genera un procedimiento almacenado para seleccionar registros de una tabla específica.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public string GenerateSelectProc(
            Table table,
            string version
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamSpSet = null;
        string sParamWhereSp = null;
        string spParamListField = null;

        List<Entity2> camposDB = GetEntity(table.nombreTabla);

        var model = GnrUtil.ToUpperCamelCase(table.nombreTabla);

        sParamSpPK = GetInputParamSpPK(camposDB, model);
        sParamSpSet = GetInputParamSpSet(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);
        spParamListField = GetListFieldSP(camposDB, model);

        SP = $"dbo.{model}SelProc";
        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, version.ToString()));
        sb.AppendLine("USE " + table.BaseDatos);
        sb.AppendLine("GO");

        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine(")");
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.Append($"{Tabs}SELECT ");

        // Leer datos SELECT
        // Estilo:
        //     SELECT Campo1
        //           ,Campo2
        //
        int nUltimoElemento = camposDB.Count - 1;
        string sFieldFirst = camposDB[0].Field.ToString();
        string sFieldLast = camposDB[nUltimoElemento].Field.ToString();
        string sTabOrNull = "";

        foreach (Entity2 o in camposDB)
        {
            sb.AppendLine(sTabOrNull + o.Field + ", ");
            sTabOrNull = new string(' ', 11);
        }
        sb = sb.Remove(sb.Length - 4, 2);

        sb.AppendLine($"{new string(' ', 6)}FROM {model}");
        if (sParamWhereSp != null)
        {
            sb.AppendLine($"{new string(' ', 5)}WHERE{sParamWhereSp}");
        }
        sb.AppendLine($"{new string(' ', 5)}RETURN(0)");
        sb.AppendLine("END");
        sb.AppendLine("GO");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));

        return sb.ToString();
    }


    /// <summary>
    /// Genera el script SQL para crear una tabla específica.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string GenerateTableCreateScript(
            string tableName
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        var script = GetTableScript(tableName);
        return script;

    }


    /// <summary>
    /// Genera el script SQL para crear tablas a partir de una lista de nombres de tablas y los almacena en la ruta projectPath
    /// </summary>
    /// <param name="tables"></param>
    /// <param name="namespaceDB"></param>
    /// <param name="classModifiers"></param>
    /// <param name="projectPath"></param>
    /// <param name="projectName"></param>
    /// <param name="classType"></param>
    public void GenerateTables(
            List<string> tables,
            string namespaceDB,
            string classModifiers,
            string projectPath,
            string projectName,
            ClassType classType
        )
    {
        string pathDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string pathProyecto = GnrUtil.AddSlash($"{projectPath}");
        if (!Directory.Exists(pathProyecto))
        {
            Directory.CreateDirectory(pathProyecto);
        }

        foreach (var table in tables)
        {
            // Obtener la estructura de la tabla (esto debe ser implementado según tu base de datos)
            var columnas = GetTableStructure(table);

            // Generar el código SQL CREATE TABLE
            string createTableSQL = GenerateTableCreateScript(table);

            /***
            * 
            */
            if (!Directory.Exists(pathProyecto))
            {
                Directory.CreateDirectory(pathProyecto);
            }
            var pathOut = GnrUtil.AddSlash(pathProyecto);
            //File.WriteAllText($"{pathOut}{GnrUtil.GetSingular(table)}.cs", createTableSQL);
            File.WriteAllText($"{pathOut}{table}.cs", createTableSQL);

        }

    }


    /// <summary>
    /// Genera un procedimiento almacenado para actualizar registros de una tabla específica.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public string GenerateUpdateProc(
            Table table,
            string version
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamSpSet = null;
        string sParamWhereSp = null;
        string sParamWhereSp13 = null;
        string spParamListField = null;

        List<Entity2> camposDB = GetEntity(table.nombreTabla);

        var model = GnrUtil.ToUpperCamelCase(table.nombreTabla);
        var sParamSp = GetInputParamSp(camposDB);

        sParamSpPK = GetInputParamSpPK(camposDB, model);
        sParamSpSet = GetInputParamSpSet(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 21);
        sParamWhereSp13 = GetInputParamSpWhere(camposDB, model, false, 13);
        spParamListField = GetListFieldSP(camposDB, model);

        SP = $"dbo.{model}UpdProc";
        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, version.ToString()));
        sb.AppendLine("USE " + table.BaseDatos);
        sb.AppendLine("GO");

        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        sb.AppendLine(sParamSp);
        sb.AppendLine(")");
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}SET NOCOUNT ON");
        sb.AppendLine($"{Tabs}BEGIN TRANSACTION");
        sb.AppendLine($"{Tabs}IF (SELECT 1 FROM {model}");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{Tabs}WHERE{sParamWhereSp}) = 1");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}UPDATE {model}");
        sb.AppendLine($"{Tabs}{Tabs}SET {sParamSpSet}");
        if (sParamWhereSp != null)
        {
            sb.AppendLine($"{Tabs}{Tabs}WHERE{sParamWhereSp13}");
        }
        sb.AppendLine($"{Tabs}{Tabs}IF (@@ERROR != 0)");
        sb.AppendLine($"{Tabs}{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{GetRaisError(version, 16, 1, model, SP)}");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK TRAN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}RETURN(1)");
        sb.AppendLine($"{Tabs}{Tabs}END");
        sb.AppendLine($"{Tabs}END");
        sb.AppendLine($"{Tabs}ELSE");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}INSERT INTO {model}(");
        // Insertar parametros INSERT 
        // Estilo:
        //     INSERT INTO Tabla ( Campo1
        //                        ,Campo2 )
        //
        int nUltimoElemento = camposDB.Count - 1;
        string sFieldFirst = camposDB[0].Field.ToString();
        string sFieldLast = camposDB[nUltimoElemento].Field.ToString();
        //string sTabOrNull = Environment.NewLine + "\t\t\t\t ";
        string sTabOrNull = string.Concat(Enumerable.Repeat(Tabs, 4));
        foreach (Entity2 o in camposDB)
        {
            sb.Append($"{sTabOrNull}{o.Field}{(o.Field.ToString() == sFieldLast ? " )" : "\r\n")}");
            sTabOrNull = string.Concat(Enumerable.Repeat(Tabs, 4)) + ",";
        }
        // Insertar parametros VALUES
        // Estilo:
        //               VALUES ( Campo1
        //                       ,Campo2 )
        //
        sb.Append($"\r\n{Tabs}{Tabs} VALUES (");
        sTabOrNull = "";
        foreach (Entity2 o in camposDB)
        {
            sb.Append($"{sTabOrNull}@{o.Field}{(o.Field.ToString() == sFieldLast ? ")" : "\r\n")}");
            sTabOrNull = string.Concat(Enumerable.Repeat(Tabs, 4)) + ",";
        }
        sb.AppendLine("");

        sb.AppendLine($"{Tabs}{Tabs}IF (@@ERROR != 0)");
        sb.AppendLine($"{Tabs}{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{GetRaisError(version, 16, 1, model, SP)}");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK TRANSACTION");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}RETURN(1)");
        sb.AppendLine($"{Tabs}{Tabs}END");
        sb.AppendLine($"{Tabs}END");
        sb.AppendLine($"{Tabs}COMMIT TRANSACTION");
        sb.AppendLine("END");
        sb.AppendLine("GO");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));
        return sb.ToString();
    }


    /// <summary>
    /// Genera el encabezado del procedimiento almacenado.
    /// </summary>
    /// <param name="sp"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public string GetHeadSp(
            string sp,
            string version
        )
    {
        StringBuilder sb = new StringBuilder();

        // Longitud fija para justificar las líneas
        int lineLength = 80;

        // Función auxiliar para justificar una línea
        string JustifyLine(string content)
        {
            int padding = lineLength - content.Length - 4; // 4 para "/***" y "***/"
            return $"/*** {content}{new string(' ', padding)}***/";
        }
        //                 1         2         3         4         5         6         7         8         9         1
        //        12345678901234567890123456789012345678901234567890123456789012345678901234567890
        ///****** xauro Gen: Procedimiento Almacenado dbo.BoletasDelProc ******/
        ///****** Fecha: {DateTime.Now.ToString("dd/MM/yyyy").PadRight(nLongitud - 17)} ******/
        sb.AppendLine(JustifyLine($"SQLServer 2008 Stored Procedure: {sp}"));
        sb.AppendLine(JustifyLine($"Fecha   : {DateTime.Now.ToString("dd/MM/yyyy")}"));
        sb.AppendLine(JustifyLine($"Autor   : {Environment.UserName}"));
        sb.AppendLine(JustifyLine($"Versión : {version.ToString()}"));

        return sb.ToString();
    }


    /// <summary>
    /// Obtiene todos los parámetros de un procedimiento almacenado.
    /// </summary>
    /// <param name="fields">Lista de campos</param>
    /// <param name="tabs">Largo del tab</param>
    /// <returns>Retorna una lista de campos de tipo. int Param0, string Param1, ...</returns>
    public string GetAllParameters(
            List<FieldList> fields,
            int tabs = 0
        )
    {

        string Param = string.Empty;
        int extraTabs = 0;
        // Buscar el tipo de campo en la lista de campos
        fields.ForEach(x =>
        {

            // BaseProceso.DataTypesParam.TryGetValue(x.tipoCampo, out string type);
            //Param += $"{new string(' ', tabs + extraTabs)}{x.tipoCampo} {GnrUtil.ToUpperCamelCase(x.nombreCampo)}, \r\n";
            Param += $"{new string(' ', tabs + extraTabs)}{x.tipoCampo} p{x.nombreCampo}, \r\n";
            if (extraTabs == 0 && fields.Count > 4)
                extraTabs = 20; // x.tipoCampo.Length;
        });
        // int Param0, string Param1, ...
        return Param.Substring(0, Param.Length - 4);

    }


    /// <summary>
    /// Obtener los parametros para la llamada a la funcion de eliminacion
    /// </summary>
    /// <param name="index_keys">La PK de la tabla</param>
    /// <returns>Retorna una cadena con parametros de llammadas desde DataLayer</returns>
    /// <exception cref="NotImplementedException"></exception>
    public string SetParamCallProcDeletePKSp(
            string index_keys
        )
    {
        // <Type0> <nameField0>, <Type1> <nameField1>, ...

        string[] keys = index_keys.Split(',');
        string asignacionParametrosSP = string.Empty;
        string type = string.Empty;
        foreach (string key in keys)
        {
            DataTypesDb.TryGetValue(key, out type);
            asignacionParametrosSP += $"@{GnrUtil.ToUpperCamelCase(key)}, ";
        }
        return asignacionParametrosSP.Substring(0, asignacionParametrosSP.Length - 2);

    }


    /// <summary>
    /// Obtiene la lista de parametros para llamadas desde DataLayer
    /// </summary>
    /// <param name="index_keys">Cadena con lista de PK separadas por comas</param>
    /// <returns>Retorna cadena con parametros PK para llamada desde DataLayer</returns>
    public string GetParametersCallPKSp(
            string index_keys
        )
    {
        // @<Type0> <nameField0>, @<Type1> <nameField1>, ...

        string[] keys = index_keys.Split(',');
        string asignacionParametrosSP = string.Empty;
        string type = string.Empty;
        string typeDb = string.Empty;

        foreach (string key in keys)
        {
            //asignacionParametrosSP += $"@{GnrUtil.ToUpperCamelCase(key.Trim())}, ";
            asignacionParametrosSP += $"@{key.Trim()}, ";
        }
        // @Param0, @Param1, ...
        return asignacionParametrosSP.Substring(0, asignacionParametrosSP.Length - 2);
    }


    /// <summary>
    /// Obtiene los parámetros de entrada para un procedimiento almacenado.
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public string GetInputParamSp(
        List<Entity2> entities
    )
    {
        List<Entity2> ents = new List<Entity2>();
        int maxLen = 0;
        //
        foreach (Entity2 o in entities)
        {
            maxLen = ((int)o.LenField > maxLen ? (int)o.LenField : maxLen);
            ents.Add(o);
        }
        //
        string sFieldFirst = ents[0].Field.ToString();
        string sFieldLast = ents[ents.Count - 1].Field.ToString();

        string sParametrosSP = null;
        foreach (Entity2 o in ents)
        {
            // string sTab = TabInsert(o.LenField, NumMax);
            sParametrosSP += $"{new string(' ', 4)}@" + o.Field.ToString().PadRight(maxLen + 1) +
                             GetSpDataType(o) +
                             (sFieldLast == o.Field.ToString() ? "" : ", " + Environment.NewLine);
        }
        return sParametrosSP;
    }


    /// <summary>
    /// Obtiene los parámetros de entrada para un procedimiento almacenado, filtrando por claves primarias.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetInputParamSpPK(
            List<Entity2> entities,
            string tableName
        )
    {
        string sParFunc = null;
        List<Entity2> ents = new List<Entity2>();

        try
        {
            var index_keys = GetKeysTable(tableName);
            //
            int maxLen = 0;
            foreach (Entity2 o in entities)
            {
                maxLen = ((int)o.LenField > maxLen ? (int)o.LenField : maxLen);


                bool isKey = index_keys.Contains((string)o.Field);

                if (isKey)
                {

                    ents.Add(o);

                }
            }
            //
            if (ents.Count > 0)
            {

                string sFieldLast = ents[ents.Count - 1].Field.ToString();

                foreach (Entity2 o in ents)
                {
                    sParFunc += $"{Tabs}@" + o.Field.ToString().PadRight(maxLen) + GetSpDataType(o) + (o.Field.ToString() != sFieldLast ? ",\r\n" : "");
                }
            }
        }
        catch (Exception)
        {
            return null;
        }
        return sParFunc;
    }

    /// <summary>
    /// Obtiene los parámetros de entrada para un procedimiento almacenado, filtrando por claves primarias y generando una cláusula SELECT ALL.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetInputParamSpSelectAll(
            List<Entity2> entities,
            string tableName
        )
    {
        string sParFunc = null;
        List<Entity2> ents = new List<Entity2>();


        try
        {
            var index_keys = GetKeysTable(tableName);
            //
            int maxLen = 0;
            foreach (Entity2 o in entities)
            {
                maxLen = ((int)o.LenField > maxLen ? (int)o.LenField : maxLen);

                bool isKey = index_keys.Contains((string)o.Field);
                if (isKey)
                {

                    ents.Add(o);

                }
            }
            //
            if (ents.Count > 0)
            {

                string sFieldLast = ents[ents.Count - 1].Field.ToString();

                foreach (Entity2 o in ents)
                {
                    TypesValueDb.TryGetValue(o.Type.ToString().ToUpper(), out string value);
                    sParFunc += $"{Tabs}@" + o.Field.ToString().PadRight(maxLen) + GetSpDataType(o) + (o.Field.ToString() != sFieldLast ? $" = {value},\r\n" : $" = {value}");
                }

            }
        }
        catch (Exception)
        {
            return null;
        }
        return sParFunc;
    }




    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="table"></param>
    /// <returns></returns>
    public string GetInputParamSpSet(
            List<Entity2> entities,
            string table
        )
    {
        List<Entity2> ents = new List<Entity2>();
        double maxLen = 0;
        //
        foreach (Entity2 o in entities)
        {
            maxLen = ((int)o.LenField > maxLen ? (int)o.LenField : maxLen);
            ents.Add(o);
        }
        string sParametrosSP = null;
        string sTabOrNull = string.Empty;

        string sFirstField = ents[0].Field.ToString();
        string sLastField = ents[ents.Count - 1].Field.ToString();

        foreach (Entity2 o in entities)
        {
            if (!(bool)o.IsIdentity)
            {
                // sParametrosSP += sTabOrNull + o.Field.ToString().PadRight((int)maxLen) + " = @" + o.Field + Environment.NewLine;

                sParametrosSP += $"{sTabOrNull}{o.Field.ToString().PadRight((int)maxLen)}= @{o.Field}{(o.Field.ToString() == sLastField ? "" : "\r\n")}";
                sTabOrNull = $"{string.Concat(Enumerable.Repeat(Tabs, 3))},";
            }
        }
        return sParametrosSP;
    }


    /// <summary>
    /// Obtiene los parámetros de entrada para un procedimiento almacenado, generando una cláusula WHERE.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="tablename"></param>
    /// <param name="oneLine"></param>
    /// <param name="aling"></param>
    /// <returns></returns>
    public string GetInputParamSpWhere(
        List<Entity2> entities,
        string tablename,
        bool oneLine = false,
        int aling = 0
    )
    {
        string sParFunc = null;
        List<Entity2> ents = new List<Entity2>();

        try
        {
            var index_keys = GetKeysTable(tablename);
            //
            int maxLen = 0;
            foreach (Entity2 o in entities)
            {

                bool isKey = index_keys.Contains((string)o.Field);

                if (isKey)
                {

                    maxLen = (int)o.LenField > maxLen ? (int)o.LenField : maxLen;
                    ents.Add(o);

                }

            }
            //
            if (ents.Count > 0)
            {
                string sFirstField = ents[0].Field.ToString();
                string sLastField = ents[ents.Count - 1].Field.ToString();
                int nTabSeparator = 1;
                foreach (Entity2 o in ents)
                {
                    decimal nNewTab = (((decimal)o.LenField) / 4) % 1;
                    int nSpace = (int)(nNewTab / ((decimal)0.25));

                    string spaces = new string(' ', nSpace);
                    string prelinea = string.Concat(Enumerable.Repeat(Tabs, 2)) + spaces;
                    prelinea = new string(' ', 7);


                    int nTab = (((int)o.LenField + 1) / 4 <= 0 ? 0 : (int)o.LenField);

                    if (nTab < maxLen)
                    {
                        if (nTab + nTabSeparator < maxLen)
                            nTab = (nTab + nTabSeparator) + (maxLen - (nTab + nTabSeparator));
                        else
                            nTab = nTab + nTabSeparator;
                    }

                    if (oneLine)
                    {

                        sParFunc += $"{(o.Field.ToString() == sFirstField ? "" : " AND ")}{o.Field} = @{o.Field} ";

                    }
                    else
                    {

                        sParFunc += $"{(o.Field.ToString() == sFirstField ? "" : "AND".PadLeft(aling))} {o.Field} = @{o.Field}{(o.Field.ToString() == sLastField ? "" : "\r\n")}";

                    }
                }
            }
        }
        catch (Exception)
        {

            return null;

        }
        return sParFunc;
    }


    /// <summary>
    /// Obtiene las claves primarias de una tabla específica.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetKeysTable(
            string tableName
        )
    {
        string index_keys = string.Empty;

        if (!Database.Conectar())
        {
            return null;
        }

        try
        {
            //
            // Obtener indices de la tabla
            // var sSqlIndex = $"EXEC sys.sp_helpindex {tableName}";
            var sSqlIndex = $"EXEC sys.sp_helpconstraint [{tableName}], 'nomsg'";

            Database.CrearComando(sSqlIndex);
            DbDataReader reader = Database.EjecutarConsulta();

            while (reader.Read())
            {

                var data = reader.GetString(reader.GetOrdinal("constraint_type"));

                if (data.Contains("PRIMARY KEY"))
                {
                    //if (reader.Read())
                    //{
                    index_keys = reader.IsDBNull(reader.GetOrdinal("constraint_keys")) ? "" : reader.GetString(reader.GetOrdinal("constraint_keys"));
                    //}
                }

            }
            reader.Close();
            Database.Desconectar();
            return index_keys;

        }
        catch (Exception)
        {
            return null;
        }
    }



    /// <summary>
    /// Obtiene una lista de campos de una tabla específica para un procedimiento almacenado.
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public string GetListFieldSP(
        List<Entity2> entities,
        string tableName
    )
    {
        string sParList = null;
        List<Entity2> ents = new List<Entity2>();

        try
        {
            var index_keys = GetKeysTable(tableName);
            //
            foreach (Entity2 o in entities)
            {

                bool isKey = index_keys.Contains((string)o.Field);

                if (isKey)
                {
                    ents.Add(o);
                }

            }

            if (ents.Count > 0)
            {
                string sFieldFirst = ents[0].Field.ToString();
                string sFieldLast = ents[ents.Count - 1].Field.ToString();
                foreach (Entity2 o in ents)
                {
                    sParList += $"{(o.Field.ToString() == sFieldFirst ? "" : Tabs)}{o.Field}{GetSpDataType(o)}{(o.Field.ToString() != sFieldLast ? "," : "")}\r\n";
                }

            }
        }
        catch (Exception)
        {

            return null;

        }
        return sParList;
    }



    /// <summary>
    /// Genera un manejador de errores para procedimientos almacenados.
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="sqlState"></param>
    /// <param name="errorCode"></param>
    /// <param name="SP"></param>
    /// <returns></returns>
    private string HandlerError(
               string errorMessage
             , string sqlState
             , int errorCode
             , string SP
           )
    {
        // TODO:  La funcion HandlerError de la clase XgenSQL2008 hay que implementarla para SQLServer

        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{Tabs}-- Declaraci�n de variables para el manejo de errores");
        sb.AppendLine($"{Tabs}DECLARE v_error_message VARCHAR(1000);");
        sb.AppendLine($"{Tabs}DECLARE v_error_occurred BOOLEAN DEFAULT FALSE;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}-- Manejador para SQLEXCEPTION (errores de SQL)");
        sb.AppendLine($"{Tabs}DECLARE EXIT HANDLER FOR SQLEXCEPTION");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}-- Guarda el mensaje de error MySQL");
        sb.AppendLine($"{Tabs}{Tabs}GET DIAGNOSTICS CONDITION 1 v_error_message = MESSAGE_TEXT;");
        sb.AppendLine($"{Tabs}{Tabs}");
        sb.AppendLine($"{Tabs}{Tabs}-- Deshace la transacción de error MySQL");
        sb.AppendLine($"{Tabs}{Tabs}IF (SELECT @transaction_READ_ONLY = 0) THEN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK;");
        sb.AppendLine($"{Tabs}{Tabs}END IF;");
        sb.AppendLine($"{Tabs}{Tabs}");
        sb.AppendLine($"{Tabs}{Tabs}-- Lanza un error personalizado con detalles");
        sb.AppendLine($"{Tabs}{Tabs}SET v_error_message=CONCAT('Error en {SP}: ', v_error_message);");
        sb.AppendLine($"{Tabs}{Tabs}SIGNAL SQLSTATE '{sqlState}' SET MESSAGE_TEXT = v_error_message;");
        sb.AppendLine($"{Tabs}END;");
        sb.AppendLine($"{Tabs}");
        return sb.ToString();
    }


    /// <summary>
    /// Genera un mensaje de error para un procedimiento almacenado en SQL Server.
    /// </summary>
    /// <param name="SQLVersion"></param>
    /// <param name="severidad"></param>
    /// <param name="estado"></param>
    /// <param name="model"></param>
    /// <param name="sp"></param>
    /// <returns></returns>
    public string GetRaisError(
          string SQLVersion
        , int Severity
        , int State
        , string model
        , string spName
        )
    {
        return $"RAISERROR('{spName}: No pudo se actualizar la tabla {model} ({SQLVersion})', {Severity}, {State})";
        // return $"RAISERROR  {errorCode} '{sp}: No pudo se actualizar la tabla {model}'";
    }


    /// <summary>
    /// Obtiene el tipo de dato para un procedimiento almacenado basado en la entidad.
    /// </summary>
    /// <param name="entitie"></param>
    /// <returns></returns>
    public string GetSpDataType(
            Entity2 entitie
        )
    {
        string str = null;
        switch (entitie.Type)
        {
            case "char":
            case "varchar":
                str = $"{Tabs}{entitie.Type.ToString().Trim()}({(entitie.MaxLength.ToString().Trim() == "-1" ? "MAX" : entitie.MaxLength.ToString().Trim())})";
                break;
            case "numeric":
            case "decimal":
                str = $"{Tabs}{entitie.Type.ToString().Trim()}({entitie.Precision.ToString().Trim()},{entitie.Scale.ToString().Trim()})";
                break;
            default:
                str = $"{Tabs}{entitie.Type.ToString().Trim()}";
                break;
        }
        return str;
    }


    /// <summary>
    /// Obtiene el script SQL para crear una tabla específica.
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Retorna un script con comando create de la tabla</returns>
    public string GetTableScript(
            string tableName
        )
    {
        string Script = "";

        string Sql = $@"
            DECLARE @table VARCHAR(100)
            SET @table = '{tableName}'

            DECLARE @sql TABLE(s VARCHAR(MAX), id INT IDENTITY)

            INSERT INTO @sql(s) VALUES ('CREATE TABLE [dbo].[' + @table + '] (')

            -- Columnas
            INSERT INTO @sql(s)
            SELECT     
                '    [' + column_name + '] [' + data_type + ']' + 
                COALESCE('(' + CAST(character_maximum_length AS VARCHAR) + ')','') + ' ' + 
                CASE 
                    WHEN EXISTS ( 
                        SELECT id FROM syscolumns
                        WHERE object_name(id)=@table
                          AND name=column_name
                          AND columnproperty(id,name,'IsIdentity') = 1 
                    ) THEN
                        'IDENTITY(' + 
                        CAST(ident_seed(@table) AS VARCHAR) + ',' + 
                        CAST(ident_incr(@table) AS VARCHAR) + ')'
                    ELSE ''
                END + ' ' +
                CASE WHEN IS_NULLABLE = 'NO' THEN 'NOT ' ELSE '' END + 'NULL,' 
            FROM information_schema.columns 
            WHERE table_name = @table
            ORDER BY ordinal_position

            -- Primary Key
            DECLARE @pkname VARCHAR(100)
            DECLARE @pk_type VARCHAR(20)

            SELECT @pkname = tc.constraint_name
            FROM information_schema.table_constraints tc
            WHERE tc.table_name = @table AND tc.constraint_type = 'PRIMARY KEY'

            SELECT TOP 1 @pk_type = 
                CASE i.type
                    WHEN 1 THEN 'CLUSTERED'
                    WHEN 2 THEN 'NONCLUSTERED'
                END
            FROM sys.indexes i
            WHERE i.object_id = OBJECT_ID(@table)
              AND i.is_primary_key = 1

            IF (@pkname IS NOT NULL)
            BEGIN
                INSERT INTO @sql(s) VALUES('    CONSTRAINT [' + @pkname + '] PRIMARY KEY ' + @pk_type + ' (')

                INSERT INTO @sql(s)
                SELECT 
                    '        [' + c.name + '] ' + 
                    CASE ic.is_descending_key WHEN 1 THEN 'DESC' ELSE 'ASC' END + ',' 
                FROM sys.indexes i
                JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
                WHERE i.object_id = OBJECT_ID(@table)
                  AND i.is_primary_key = 1
                ORDER BY ic.key_ordinal

                UPDATE @sql SET s = LEFT(s, LEN(s) - 1) WHERE id = @@IDENTITY

                DECLARE @withOptions NVARCHAR(MAX), @filegroup NVARCHAR(100)
                SELECT 
                    @withOptions = 
                        'WITH (PAD_INDEX = ' + CASE is_padded WHEN 1 THEN 'ON' ELSE 'OFF' END + 
                        ', STATISTICS_NORECOMPUTE = OFF' +
                        ', IGNORE_DUP_KEY = ' + CASE ignore_dup_key WHEN 1 THEN 'ON' ELSE 'OFF' END +
                        ', ALLOW_ROW_LOCKS = ' + CASE allow_row_lockS WHEN 1 THEN 'ON' ELSE 'OFF' END +
                        ', ALLOW_PAGE_LOCKS = ' + CASE allow_page_locks WHEN 1 THEN 'ON' ELSE 'OFF' END +
                        ', OPTIMIZE_FOR_SEQUENTIAL_KEY = ' + CASE optimize_for_sequential_key WHEN 1 THEN 'ON' ELSE 'OFF' END + ')',
                    @filegroup = ds.name
                FROM sys.indexes i
                JOIN sys.data_spaces ds ON i.data_space_id = ds.data_space_id
                WHERE i.object_id = OBJECT_ID(@table)
                  AND i.is_primary_key = 1

                INSERT INTO @sql(s) VALUES('    ) ' + @withOptions + ' ON [' + @filegroup + ']')
            END
            ELSE 
            BEGIN
                UPDATE @sql SET s = LEFT(s, LEN(s) - 1) WHERE id = @@IDENTITY
            END

            INSERT INTO @sql(s) VALUES(') ON [PRIMARY]')
            SELECT s FROM @sql ORDER BY id
            ";



        if (!Database.Conectar())
        {
            return null;
        }
        Database.CrearComando(Sql);
        DbDataReader dr = Database.EjecutarConsulta();

        DataTable dt = new DataTable();

        // Convertimos el DataRead a DataTable
        //
        dt.TableName = MethodBase.GetCurrentMethod().DeclaringType.Name;
        dt.Load(dr);

        DataTableReader reader = new DataTableReader(dt);
        //
        if (reader == null)
            return null;

        while (reader.Read())
        {
            Script += reader.GetString(0) + Environment.NewLine;
        }
        return Script;
    }

    /// <summary>
    /// Obtiene la estructura de una tabla específica, incluyendo nombre de columna, tipo, si es nulo y si es clave primaria.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public List<(string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)> GetTableStructure(
        string tableName
    )
    {
        // Simulación de datos obtenidos de la base de datos
        // En un caso real, esto debería obtenerse de la base de datos o de un esquema definido
        //List<Entity2> campos = GetSqlTableInfo(tableName);
        List<Entity2> campos = GetEntity(tableName);

        var columnas = new List<(string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)>();

        foreach (var campo in campos)
        {
            string tipo = campo.Type.ToString();
            if (campo.MaxLength != null && int.TryParse(campo.MaxLength.ToString(), out int maxLength) && maxLength > 0)
            {
                tipo += $"({maxLength})";
            }

            columnas.Add((
                Nombre: campo.Field.ToString(),
                Tipo: tipo,
                EsNulo: campo.IsNullable != null && (bool)campo.IsNullable.Equals("yes"),
                EsClavePrimaria: campo.PrimaryKey != null && (bool)campo.PrimaryKey
            ));
        }

        return columnas;
    }


    /// <summary>
    /// Genera el encabezado para un procedimiento almacenado en SQL Server, eliminando el procedimiento si ya existe.
    /// </summary>
    /// <param name="spName"></param>
    /// <returns></returns>
    public string HeaderSpMethod(
            string spName
        )
    {
        string sBuffer = null;
        sBuffer += "IF OBJECT_ID('" + spName + "') IS NOT NULL" + Environment.NewLine;
        sBuffer += "BEGIN" + Environment.NewLine;
        sBuffer += $"{Tabs}DROP PROCEDURE " + spName + Environment.NewLine;
        sBuffer += $"{Tabs}PRINT '<<< PROCEDIMIENTO DROPEADO " + spName + " >>>'" + Environment.NewLine;
        sBuffer += "END" + Environment.NewLine;
        sBuffer += "GO" + Environment.NewLine;
        sBuffer += Environment.NewLine;
        return sBuffer;
    }


    /// <summary>
    /// Obtiene la información de una tabla de SQL Server.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public List<Entity2> GetSqlTableInfo(
        string tableName
        )
    {
        List<Entity2> entityList = new List<Entity2>();
        string index_keys = string.Empty;

        try
        {
            var connection = !Database.Conectar();

            string sSql = $@"
                    SELECT COLUMN_NAME AS Field,
                           DATA_TYPE AS Type,
                           CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                           NUMERIC_PRECISION AS Precision,
                           NUMERIC_SCALE AS Scale,
                           IS_NULLABLE AS IsNullable,
                           CASE WHEN COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 THEN 'auto_increment' ELSE '' END AS IsIdentity,
                           CASE WHEN COLUMN_NAME IN (
                               SELECT COLUMN_NAME
                               FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                               WHERE TABLE_NAME = '{tableName}' AND OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                           ) THEN 1 ELSE 0 END AS ColumnKey
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = '{tableName}'
                    ORDER BY ORDINAL_POSITION";

            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            while (dr.Read())
            {
                Entity2 entity = new Entity2
                {
                    Field = dr["Field"].ToString(),
                    Type = dr["Type"].ToString(),
                    MaxLength = dr["MaxLength"] != DBNull.Value ? dr["MaxLength"] : null,
                    Precision = dr["Precision"] != DBNull.Value ? dr["Precision"] : null,
                    Scale = dr["Scale"] != DBNull.Value ? dr["Scale"] : null,
                    IsNullable = dr["IsNullable"].ToString().ToLower() == "yes",
                    IsIdentity = dr["IsIdentity"].ToString().ToLower().Contains("auto_increment"),
                    PrimaryKey = dr["ColumnKey"].ToString().ToLower() == "1"
                };

                entityList.Add(entity);
            }

            dr.Close();
            return entityList;

        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            Database.Desconectar();
        }
    }


    /// <summary>
    /// Obtiene la lista de bases de datos
    /// </summary>
    /// <returns>Retorna una lista de tipo string con los nombres de las bases de datos</returns>
    public List<string> GetDataBases()
    {

        string sSql = "SHOW DATABASES;";
        var oObject = new List<string>();

        if (!Database.Conectar())
        {
            return null;
        }

        try
        {
            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            DataTable dt = new DataTable();

            // Convertimos el DataRead a DataTable
            //
            dt.TableName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            dt.Load(dr);

            DataTableReader reader = new DataTableReader(dt);
            //
            if (reader == null)
                return null;

            while (reader.Read())
            {
                var Databasename = reader.IsDBNull(reader.GetOrdinal("Database")) ? "" : reader.GetString(reader.GetOrdinal("name"));
                oObject.Add(Databasename);
            }
            return oObject;
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            Database.Desconectar();
        }
    }


    /// <summary>
    /// Obtiene la lista de tablas de la base datos donde esta conectado
    /// </summary>
    /// <returns>Retorna una lista de tipo string con nombre de tablas</returns>
    public List<string> GetListTables(
            string DatabaseName
        )
    {
        string sSql = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{DatabaseName}';";
        List<string> sLista = new List<string>();

        try
        {
            if (!Database.Conectar())
                return null;

            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            DataTable dt = new DataTable();

            // Convertimos el DataRead a DataTable
            //
            dt.TableName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            dt.Load(dr);

            DataTableReader reader = new DataTableReader(dt);
            //
            if (reader == null)
                return null;

            while (reader.Read())
            {
                string name = reader.IsDBNull(reader.GetOrdinal("TABLE_NAME")) ? "" : reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                sLista.Add(name);
            }
            Database.Desconectar();
        }
        catch (Exception)
        {
            return sLista;
        }
        return sLista;
    }


    /// <summary>
    /// Obtiene la estructura de una tabla específica en la base de datos.
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Retorna un List con la estructura de la tabla</returns>
    public List<Entity2> GetEntity(
            string tableName
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<Entity2> entityList = new List<Entity2>();

        // Consulta para obtener información de las columnas de la tabla
        string query = $@"
                SELECT COLUMN_NAME AS Field,
                       DATA_TYPE AS Type,
                       CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                       NUMERIC_PRECISION AS Precision,
                       NUMERIC_SCALE AS Scale,
                       IS_NULLABLE AS IsNullable,
                       COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') AS IsIdentity,
                       CASE WHEN COLUMN_NAME IN (
                           SELECT COLUMN_NAME
                           FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                           WHERE TABLE_NAME = '{tableName}' AND OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                       ) THEN 1 ELSE 0 END AS PrimaryKey
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = '{tableName}'
                ORDER BY ORDINAL_POSITION;";

        try
        {
            // Conectar a la base de datos
            if (!Database.Conectar())
            {
                throw new Exception("No se pudo conectar a la base de datos.");
            }

            // Crear el comando y ejecutar la consulta
            Database.CrearComando(query);
            DbDataReader dr = Database.EjecutarConsulta();

            while (dr.Read())
            {
                // Valor por defecto para el tipo de datos
                TypesValueDb.TryGetValue(dr["Type"].ToString().ToUpper(), out string value);

                // Crear la entidad con los datos obtenidos
                Entity2 entity = new Entity2
                {
                    Field = dr["Field"].ToString(),
                    Type = dr["Type"].ToString(),
                    MaxLength = dr["MaxLength"] != DBNull.Value ? dr["MaxLength"] : null,
                    Precision = dr["Precision"] != DBNull.Value ? dr["Precision"] : null,
                    Scale = dr["Scale"] != DBNull.Value ? dr["Scale"] : null,
                    IsNullable = dr["IsNullable"].ToString().ToLower() == "yes",
                    IsIdentity = dr["IsIdentity"] != DBNull.Value && Convert.ToBoolean(dr["IsIdentity"]),
                    PrimaryKey = dr["PrimaryKey"] != DBNull.Value && Convert.ToBoolean(dr["PrimaryKey"]),
                    ParamPrg = $"\"@{GnrUtil.ToUpperCamelCase(dr["Field"].ToString())}\", {GnrUtil.ToUpperCamelCase(dr["Field"].ToString())}",
                    LenField = dr["Field"].ToString().Length,
                    DefaultValue = value
                };

                entityList.Add(entity);
            }

            dr.Close();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las columnas de la tabla '{tableName}': {ex.Message}");
        }
        finally
        {
            // Desconectar de la base de datos
            Database.Desconectar();
        }

        return entityList;
    }

    /// <summary>
    /// Obtener los campos PK de la tabla
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Cadena separada por comas</returns>
    /// <exception cref="Exception"></exception>
    public string GetConstraintKeys(
            string tableName
        )
    {
        string index_keys = string.Empty;

        try
        {
            var connection = Database.Conectar();

            // Obtener indices de la tabla
            //
            if (!connection)
            {
                throw new Exception("No se pudo conectar a la base de datos");
            }

            Database.CrearComando($"EXEC sys.sp_helpconstraint [{tableName}], 'nomsg'");

            DbDataReader dr = Database.EjecutarConsulta();
            DataTable dt = new DataTable();
            // Convertimos el DataRead a DataTable
            //
            dt.TableName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            dt.Load(dr);

            DataTableReader reader = new DataTableReader(dt);
            //
            if (reader == null)
                throw new Exception("No se pudo obtener los campos de la tabla");

            while (reader.Read())
            {
                var data = reader.GetString(reader.GetOrdinal("constraint_type"));

                if (data.Contains("PRIMARY KEY"))
                {
                    index_keys = reader.IsDBNull(reader.GetOrdinal("constraint_keys")) ? "" : reader.GetString(reader.GetOrdinal("constraint_keys"));
                }
            }

            dr.Close();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las claves primarias de la tabla '{tableName}': {ex.Message}");
        }
        finally
        {
            Database.Desconectar();
        }

        return index_keys.Replace(" ", "");
    }


    /// <summary>
    /// Setea el valor por defecto de un tipo de dato
    /// </summary>
    /// <param name="type">Tipo de datos</param>
    /// <returns>Retorna cadena de caracteres con el valor por defecto para un tipo de datos</returns>
    public string SetDefaultValue(
            string type
        )
    {
        switch (type.ToLower())
        {
            case "int":
                return "0";
            case "float":
                return "0.0f";
            case "double":
                return "0.0d";
            case "decimal":
                return "0.0m";
            case "byte":
                return "0";
            case "sbyte":
                return "0";
            case "short":
                return "0";
            case "ushort":
                return "0";
            case "uint":
                return "0u";
            case "long":
                return "0L";
            case "ulong":
                return "0UL";
            case "char":
                return "'\0'";
            case "string":
            case "varchar":
                return "\"\"";
            case "bool":
            case "boolean":
                return "false";
            case "byte[]":
            case "sbyte[]":
                return "new byte[0]";
            case "datetime":
                return "DateTime.Parse(\"01/01/2000\")";
            case "guid":
                return "Guid.Empty.ToString()";
            case "timespan":
                return "TimeSpan.Zero.ToString()";
            case "object":
                return "null";
            default:
                return "null";
        }
    }

    /// <summary>
    /// Carga los tipos de datos y sus valores por defecto en un diccionario.
    /// </summary>
    public void FillTypesValueDb()
    {
        TypesValueDb.Clear();
        TypesValueDb.Add("BIGINT", "0"); // long
        TypesValueDb.Add("BINARY", "new byte[0]"); // byte[]
        TypesValueDb.Add("BIT", "false"); // bool
        TypesValueDb.Add("CHAR", "\'\'"); // string
        TypesValueDb.Add("DATE", "DateTime.MinValue"); // DateTime
        TypesValueDb.Add("DATETIME", "DateTime.MinValue"); // DateTime
        TypesValueDb.Add("DATETIME2", "DateTime.MinValue"); // DateTime
        TypesValueDb.Add("DATETIMEOFFSET", "DateTimeOffset.MinValue"); // DateTimeOffset
        TypesValueDb.Add("DECIMAL", "0.0m"); // decimal
        TypesValueDb.Add("FLOAT", "0.0d"); // double
        TypesValueDb.Add("IMAGE", "new byte[0]"); // byte[]
        TypesValueDb.Add("INT", "0"); // int
        TypesValueDb.Add("MONEY", "0.0m"); // decimal
        TypesValueDb.Add("NCHAR", "\'\'"); // string
        TypesValueDb.Add("NTEXT", "\'\'"); // string
        TypesValueDb.Add("NUMERIC", "0.0m"); // decimal
        TypesValueDb.Add("NVARCHAR", "\'\'"); // string
        TypesValueDb.Add("REAL", "0.0f"); // float
        TypesValueDb.Add("SMALLDATETIME", "DateTime.MinValue"); // DateTime
        TypesValueDb.Add("SMALLINT", "0"); // short
        TypesValueDb.Add("SMALLMONEY", "0.0m"); // decimal
        TypesValueDb.Add("SQL_VARIANT", "null"); // object
        TypesValueDb.Add("TEXT", "\'\'"); // string
        TypesValueDb.Add("TIME", "TimeSpan.Zero"); // TimeSpan
        TypesValueDb.Add("TIMESTAMP", "new byte[0]"); // byte[]
        TypesValueDb.Add("TINYINT", "0"); // byte
        TypesValueDb.Add("UNIQUEIDENTIFIER", "Guid.Empty"); // Guid
        TypesValueDb.Add("VARBINARY", "new byte[0]"); // byte[]
        TypesValueDb.Add("VARCHAR", "\'\'"); // string
        TypesValueDb.Add("XML", "\'\'"); // string
    }


    /// <summary>
    /// Carga los tipos de datos en la base de datos
    /// </summary>
    public void FillTypesDb()
    {
        DataTypes.Clear();
        DataTypes.Add("BIGINT", "long"); // Corregido, System.Int64 es lo mismo que long
        DataTypes.Add("BINARY", "byte[]"); // Correcto
        DataTypes.Add("BIT", "bool"); // Correcto
        DataTypes.Add("CHAR", "string"); // Correcto
        DataTypes.Add("DATE", "DateTime"); // Correcto
        DataTypes.Add("DATETIME", "DateTime"); // Correcto
        DataTypes.Add("DATETIME2", "DateTime"); // Correcto
        DataTypes.Add("DATETIMEOFFSET", "DateTimeOffset"); // Correcto
        DataTypes.Add("DECIMAL", "decimal"); // Correcto
        DataTypes.Add("FLOAT", "double"); // Correcto
        DataTypes.Add("IMAGE", "byte[]"); // Correcto, aunque está obsoleto en SQL Server
        DataTypes.Add("INT", "int"); // Correcto
        DataTypes.Add("MONEY", "decimal"); // Correcto
        DataTypes.Add("NCHAR", "string"); // Correcto
        DataTypes.Add("NTEXT", "string"); // Correcto, aunque está obsoleto en SQL Server
        DataTypes.Add("NUMERIC", "decimal"); // Correcto
        DataTypes.Add("NVARCHAR", "string"); // Correcto
        DataTypes.Add("REAL", "float"); // Corregido, System.Single es lo mismo que float
        DataTypes.Add("SMALLDATETIME", "DateTime"); // Correcto
        DataTypes.Add("SMALLINT", "short"); // Correcto (System.Int16 es lo mismo que short)
        DataTypes.Add("SMALLMONEY", "decimal"); // Correcto
        DataTypes.Add("SQL_VARIANT", "object"); // Correcto
        DataTypes.Add("TEXT", "string"); // Correcto, aunque está obsoleto en SQL Server
        DataTypes.Add("TIME", "TimeSpan"); // Correcto
        DataTypes.Add("TIMESTAMP", "byte[]"); // Correcto, pero considerar ROWVERSION en SQL Server
        DataTypes.Add("TINYINT", "byte"); // Correcto
        DataTypes.Add("UNIQUEIDENTIFIER", "Guid"); // Correcto
        DataTypes.Add("VARBINARY", "byte[]"); // Correcto
        DataTypes.Add("VARCHAR", "string"); // Correcto
        DataTypes.Add("XML", "string"); // Correcto
    }

    /// Obtiene la información de una tabla de SQL Server.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    private List<string> GetColumnsDefinition(
        string tableName
    )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        var columns = new List<string>();

        // Consulta SQL para obtener las definiciones de las columnas
        string sql = $@"
        SELECT 
            COLUMN_NAME, 
            DATA_TYPE, 
            IS_NULLABLE, 
            CHARACTER_MAXIMUM_LENGTH, 
            NUMERIC_PRECISION, 
            NUMERIC_SCALE, 
            COLUMN_DEFAULT,
            CASE WHEN COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 THEN 'auto_increment' ELSE '' END AS IsIdentity
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = '{tableName}' AND TABLE_SCHEMA = 'dbo'
        ORDER BY ORDINAL_POSITION";

        try
        {
            // Conectar a la base de datos
            if (!Database.Conectar())
            {
                throw new Exception("No se pudo conectar a la base de datos.");
            }

            // Ejecutar la consulta
            Database.CrearComando(sql);
            var dr = Database.EjecutarConsulta();
            while (dr.Read())
            {
                string colName = dr.GetString("COLUMN_NAME");
                string dataType = dr.GetString("DATA_TYPE");
                string isNullable = dr.GetString("IS_NULLABLE");
                object charLength = dr["CHARACTER_MAXIMUM_LENGTH"];
                object numPrecision = dr["NUMERIC_PRECISION"];
                object numScale = dr["NUMERIC_SCALE"];
                string extra = dr.GetString("IsIdentity");

                var colDef = new StringBuilder();
                colDef.Append($"  {colName} {dataType}");

                if (dataType is "varchar" or "char" or "binary" or "varbinary")
                {
                    if (charLength != DBNull.Value)
                        colDef.Append($"({charLength})");
                }
                else if (dataType is "decimal" or "float" or "double")
                {
                    if (numPrecision != DBNull.Value)
                    {
                        colDef.Append($"({numPrecision}");
                        if (numScale != DBNull.Value && Convert.ToInt32(numScale) > 0)
                            colDef.Append($",{numScale}");
                        colDef.Append(")");
                    }
                }

                colDef.Append(isNullable == "NO" ? " NOT NULL" : " NULL");

                if (extra.Contains("auto_increment", StringComparison.OrdinalIgnoreCase))
                    colDef.Append(" AUTO_INCREMENT");

                columns.Add(colDef.ToString());
            }
            Database.Desconectar();

        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las definiciones de las columnas de la tabla '{tableName}': {ex.Message}");
        }
        finally
        {
            Database.Desconectar();
        }

        return columns;
    }

    /// <summary>
    /// Obtiene la definición de la clave primaria de una tabla específica en SQL Server.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exception"></exception>
    private string GetPrimaryKeyDefinition(
        string tableName
    )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        var primaryKeyColumns = new List<string>();

        // Consulta SQL para obtener las columnas que forman parte de la clave primaria
        string sql = $@"
        SELECT COLUMN_NAME
        FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
        WHERE TABLE_NAME = '{tableName}'
          AND TABLE_SCHEMA = 'dbo'
          AND OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_NAME), 'IsPrimaryKey') = 1
        ORDER BY ORDINAL_POSITION";

        try
        {
            // Conectar a la base de datos
            if (!Database.Conectar())
            {
                throw new Exception("No se pudo conectar a la base de datos.");
            }

            // Ejecutar la consulta
            Database.CrearComando(sql);
            var reader = Database.EjecutarConsulta();

            while (reader.Read())
            {
                primaryKeyColumns.Add($"[{reader["COLUMN_NAME"]}]");
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener la clave primaria de la tabla '{tableName}': {ex.Message}");
        }
        finally
        {
            Database.Desconectar();
        }

        // Construir la definición de la clave primaria
        if (primaryKeyColumns.Count > 0)
        {
            return $"PRIMARY KEY ({string.Join(", ", primaryKeyColumns)})";
        }

        return null; // Si no hay clave primaria
    }

    /// <summary>
    /// Genera un procedimiento almacenado para seleccionar registros de una tabla específica.
    /// </summary>
    /// <param name="tabla"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string ProcSelProc(
        Table tabla
    )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        // Obtener los campos de la tabla
        List<Entity2> camposDB = GetEntity(tabla.nombreTabla);
        var first = camposDB.First();
        var last = camposDB.Last();

        // Convertir el nombre de la tabla a UpperCamelCase
        var model = GnrUtil.ToUpperCamelCase(tabla.nombreTabla);

        // Generar los parámetros de entrada y la cláusula WHERE
        string sParamSpPK = GetInputParamSpPK(camposDB, model);
        string sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);
        string sParamSpGetAll = GetInputParamSpSelectAll(camposDB, model);

        // Nombre del procedimiento almacenado
        string SP = $"dbo.{model}SelProc";

        // Construir el procedimiento almacenado
        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, SQLType));
        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        // sb.AppendLine(sParamSpPK);
        sb.AppendLine(sParamSpGetAll);
        sb.AppendLine(")");
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}SET NOCOUNT ON;");
        sb.AppendLine($"{Tabs}SELECT");

        sb.AppendLine($"{string.Join($"{Environment.NewLine}", camposDB.Select(c => Tabs + Tabs + (c.Field.Equals(first.Field) ? "  " + c.Field : ", " + c.Field)))}");
        sb.AppendLine($"{Tabs}FROM {tabla.nombreTabla}");

        // Agregar cláusula WHERE si es necesaria
        if (!string.IsNullOrEmpty(sParamWhereSp))
        {
            sb.AppendLine($"{Tabs}WHERE {sParamWhereSp}");

            // Agregar condición para obtener todos los registros
            // 
            if (!string.IsNullOrWhiteSpace(sParamSpGetAll))
            {
                #region Filtar campos PK
                // Lista de campos PK
                List<Entity2> camposPK = new List<Entity2>();
                camposDB.ForEach(pk =>
                {

                    if (pk.PrimaryKey != null && (bool)pk.PrimaryKey)
                    {
                        camposPK.Add(pk);
                    }
                });
                #endregion

                #region Lista de campos OR
                var LastField = camposPK.Last();
                string fieldNameLast = LastField.Field.ToString().ToLower();
                sb.Append($"{Tabs}   OR (");
                camposPK.ForEach(z =>
                {
                    //if (z.PrimaryKey != null && (bool)z.PrimaryKey)
                    //{
                    string field = z.Field.ToString().ToLower();
                    var _OR_ = (z.Field.ToString().ToLower() == fieldNameLast ? "" : " AND ");
                    sb.Append($"@{z.Field} = {z.DefaultValue}{_OR_}");
                    //}

                });
                sb.AppendLine(")");
                #endregion
            }
        }

        sb.AppendLine("END");
        sb.AppendLine("GO");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));

        return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tabla"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string ProcDelProc(
            Table tabla
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        // Obtener los campos de la tabla
        List<Entity2> camposDB = GetEntity(tabla.nombreTabla);

        // Convertir el nombre de la tabla a UpperCamelCase
        var model = GnrUtil.ToUpperCamelCase(tabla.nombreTabla);

        // Generar los parámetros de entrada y la cláusula WHERE
        string sParamSpPK = GetInputParamSpPK(camposDB, model);
        string sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);

        // Nombre del procedimiento almacenado
        string SP = $"dbo.{model}DelProc";

        // Construir el procedimiento almacenado
        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, SQLType));
        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine(")");
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}SET NOCOUNT ON;");
        sb.AppendLine($"{Tabs}BEGIN TRANSACTION;");
        sb.AppendLine($"{Tabs}DELETE FROM {tabla.nombreTabla}");

        // Agregar la cláusula WHERE si es necesaria
        if (!string.IsNullOrEmpty(sParamWhereSp))
        {
            sb.AppendLine($"{Tabs}WHERE {sParamWhereSp};");
        }

        // Manejo de errores y confirmación de la transacción
        sb.AppendLine($"{Tabs}IF (@@ERROR != 0)");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}ROLLBACK TRANSACTION;");
        sb.AppendLine($"{Tabs}{Tabs}{GetRaisError(SQLType, 16, 1, model, SP)}");
        sb.AppendLine($"{Tabs}{Tabs}RETURN(1);");
        sb.AppendLine($"{Tabs}END");
        sb.AppendLine($"{Tabs}COMMIT TRANSACTION;");
        sb.AppendLine($"{Tabs}SET NOCOUNT OFF;");
        sb.AppendLine($"{Tabs}RETURN(0);");
        sb.AppendLine("END");
        sb.AppendLine("GO");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));

        return sb.ToString();
    }

    /// <summary>
    /// Genera un procedimiento almacenado para actualizar o insertar registros en una tabla específica.
    /// </summary>
    /// <param name="tabla"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string ProcUpdProc(
            Table tabla
        )
    {

        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        // Obtener los campos de la tabla
        List<Entity2> camposDB = GetEntity(tabla.nombreTabla);

        // Convertir el nombre de la tabla a UpperCamelCase
        var model = GnrUtil.ToUpperCamelCase(tabla.nombreTabla);

        // Generar los parámetros de entrada y las cláusulas SET y WHERE
        var sParamSp = GetInputParamSp(camposDB);
        string sParamSpPK = GetInputParamSpPK(camposDB, model);
        string sParamSpSet = GetInputParamSpSet(camposDB, model);

        string sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);

        // Nombre del procedimiento almacenado
        string SP = $"dbo.{model}UpdProc";

        // Construir el procedimiento almacenado
        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, SQLType));
        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        sb.AppendLine(sParamSp);
        sb.AppendLine(")");
        sb.AppendLine("AS");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}SET NOCOUNT ON;");
        sb.AppendLine($"{Tabs}BEGIN TRANSACTION;");

        // Actualizar el registro si existe
        sb.AppendLine($"{Tabs}IF EXISTS (SELECT 1 FROM {tabla.nombreTabla} WHERE {sParamWhereSp})");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}UPDATE {tabla.nombreTabla}");
        sb.AppendLine($"{Tabs}{Tabs}SET {sParamSpSet}");
        sb.AppendLine($"{Tabs}{Tabs}WHERE {sParamWhereSp};");
        sb.AppendLine($"{Tabs}{Tabs}IF (@@ERROR != 0)");
        sb.AppendLine($"{Tabs}{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK TRANSACTION;");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{GetRaisError(SQLType, 16, 1, model, SP)}");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}RETURN(1);");
        sb.AppendLine($"{Tabs}{Tabs}END");
        sb.AppendLine($"{Tabs}END");
        sb.AppendLine($"{Tabs}ELSE");
        sb.AppendLine($"{Tabs}BEGIN");

        // Insertar un nuevo registro si no existe
        sb.AppendLine($"{Tabs}{Tabs}INSERT INTO {tabla.nombreTabla} (");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{string.Join($",{Environment.NewLine}{Tabs}{Tabs}{Tabs}", camposDB.Select(c => c.Field))}");
        sb.AppendLine($"{Tabs}{Tabs}) VALUES (");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{string.Join($",{Environment.NewLine}{Tabs}{Tabs}{Tabs}", camposDB.Select(c => $"@{c.Field}"))}");
        sb.AppendLine($"{Tabs}{Tabs});");
        sb.AppendLine($"{Tabs}{Tabs}IF (@@ERROR != 0)");
        sb.AppendLine($"{Tabs}{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK TRANSACTION;");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}{GetRaisError(SQLType, 16, 1, model, SP)}");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}RETURN(1);");
        sb.AppendLine($"{Tabs}{Tabs}END");
        sb.AppendLine($"{Tabs}END");

        // Confirmar la transacción
        sb.AppendLine($"{Tabs}COMMIT TRANSACTION;");
        sb.AppendLine($"{Tabs}SET NOCOUNT OFF;");
        sb.AppendLine($"{Tabs}RETURN(0);");
        sb.AppendLine("END");
        sb.AppendLine("GO");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));

        return sb.ToString();
    }


}
