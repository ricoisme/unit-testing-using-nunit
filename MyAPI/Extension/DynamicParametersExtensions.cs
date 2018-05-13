using Dapper;
using System.Collections.Generic;
using System.Data;

namespace MyAPI.Extension
{
    public static class DynamicParametersExtensions
    {
        // http://msdn.microsoft.com/en-us/library/cc716729(v=vs.100).aspx
        static readonly Dictionary<SqlDbType, DbType?> sqlDbTypeMap = new Dictionary<SqlDbType, DbType?>
        {
            {SqlDbType.BigInt, DbType.Int64},
            {SqlDbType.Binary, DbType.Binary},
            {SqlDbType.Bit, DbType.Boolean},
            {SqlDbType.Char, DbType.AnsiStringFixedLength},
            {SqlDbType.DateTime, DbType.DateTime},
            {SqlDbType.Decimal, DbType.Decimal},
            {SqlDbType.Float, DbType.Double},
            {SqlDbType.Image, DbType.Binary},
            {SqlDbType.Int, DbType.Int32},
            {SqlDbType.Money, DbType.Decimal},
            {SqlDbType.NChar, DbType.StringFixedLength},
            {SqlDbType.NText, DbType.String},
            {SqlDbType.NVarChar, DbType.String},
            {SqlDbType.Real, DbType.Single},
            {SqlDbType.UniqueIdentifier, DbType.Guid},
            {SqlDbType.SmallDateTime, DbType.DateTime},
            {SqlDbType.SmallInt, DbType.Int16},
            {SqlDbType.SmallMoney, DbType.Decimal},
            {SqlDbType.Text, DbType.String},
            {SqlDbType.Timestamp, DbType.Binary},
            {SqlDbType.TinyInt, DbType.Byte},
            {SqlDbType.VarBinary, DbType.Binary},
            {SqlDbType.VarChar, DbType.AnsiString},
            {SqlDbType.Variant, DbType.Object},
            {SqlDbType.Xml, DbType.Xml},
            {SqlDbType.Udt,(DbType?)null}, // Dapper will take care of it
            {SqlDbType.Structured,(DbType?)null}, // Dapper will take care of it
            {SqlDbType.Date, DbType.Date},
            {SqlDbType.Time, DbType.Time},
            {SqlDbType.DateTime2, DbType.DateTime2},
            {SqlDbType.DateTimeOffset, DbType.DateTimeOffset}
        };

        public static void Add(this DynamicParameters parameter, string name, object value, SqlDbType? sqlDbType, ParameterDirection? direction, int? size)
        {
            parameter.Add(name, value, (sqlDbType != null ? sqlDbTypeMap[sqlDbType.Value] : (DbType?)null), direction, size);
        }

        public static void Add(this DynamicParameters parameter, string name, object value = null, SqlDbType? sqlDbType = null, ParameterDirection? direction = null, int? size = null, byte? precision = null, byte? scale = null)
        {
            parameter.Add(name, value, (sqlDbType != null ? sqlDbTypeMap[sqlDbType.Value] : (DbType?)null), direction, size, precision, scale);
        }

        public static Dictionary<string, object> Get(this DynamicParameters parameter)
        {
            return parameter.Get<object>();
        }

        // all the parameters are of the same type T
        public static Dictionary<string, T> Get<T>(this DynamicParameters parameter)
        {
            Dictionary<string, T> values = new Dictionary<string, T>();
            foreach (string parameterName in parameter.ParameterNames)
                values.Add(parameterName, parameter.Get<T>(parameterName));
            return values;
        }
    }
}
