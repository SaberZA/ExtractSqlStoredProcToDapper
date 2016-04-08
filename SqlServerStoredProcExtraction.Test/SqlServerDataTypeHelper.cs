using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerStoredProcExtraction.Test
{
    public class SqlServerDataTypeHelper
    {
        public static Type GetClrType(string sqlType)
        {
            var sqlTypeLower = sqlType.ToLower();
            var indexOfParameter = sqlTypeLower.IndexOf("(", System.StringComparison.Ordinal);

            if (indexOfParameter >= 0)
            {
                sqlTypeLower = sqlTypeLower.Substring(0, indexOfParameter);
            }

            switch (sqlTypeLower)
            {
                case "bigint":
                    return typeof(long?);

                case "binary":
                case "image":
                case "timestamp":
                case "varbinary":
                    return typeof(byte[]);

                case "bit":
                    return typeof(bool?);

                case "char":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "text":
                case "varchar":
                case "xml":
                    return typeof(string);

                case "datetime":
                case "smalldatetime":
                case "date":
                case "time":
                case "datetime2":
                    return typeof(DateTime?);

                case "decimal":
                case "money":
                case "smallmoney":
                    return typeof(decimal?);

                case "float":
                    return typeof(double?);

                case "int":
                    return typeof(int?);

                case "real":
                    return typeof(float?);

                case "uniqueidentifier":
                    return typeof(Guid?);

                case "smallint":
                    return typeof(short?);

                case "tinyint":
                    return typeof(byte?);

                case "variant":
                case "udt":
                    return typeof(object);

                case "structured":
                    return typeof(DataTable);

                case "datetimeoffset":
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }

        public static Type GetClrType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(long?);

                case SqlDbType.Binary:
                case SqlDbType.Image:
                case SqlDbType.Timestamp:
                case SqlDbType.VarBinary:
                    return typeof(byte[]);

                case SqlDbType.Bit:
                    return typeof(bool?);

                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return typeof(string);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    return typeof(DateTime?);

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                    return typeof(decimal?);

                case SqlDbType.Float:
                    return typeof(double?);

                case SqlDbType.Int:
                    return typeof(int?);

                case SqlDbType.Real:
                    return typeof(float?);

                case SqlDbType.UniqueIdentifier:
                    return typeof(Guid?);

                case SqlDbType.SmallInt:
                    return typeof(short?);

                case SqlDbType.TinyInt:
                    return typeof(byte?);

                case SqlDbType.Variant:
                case SqlDbType.Udt:
                    return typeof(object);

                case SqlDbType.Structured:
                    return typeof(DataTable);

                case SqlDbType.DateTimeOffset:
                    return typeof(DateTimeOffset?);

                default:
                    throw new ArgumentOutOfRangeException("sqlType");
            }
        }
    }
}
