using Newtonsoft.Json.Linq;
using NpgsqlTypes;

namespace ReactWithASP.Server.Helpers
{
    public class DbTypeConverter
    {
        // This method is now here for reference in maintaining method below
        public static NpgsqlDbType GetNpgsqlDbType(JTokenType jTokenType, JToken value)
        {

            if (jTokenType == JTokenType.String)
            {
                if (Guid.TryParse(value.ToString(), out var id))
                {
                    return NpgsqlDbType.Uuid;
                }
            }

            switch (jTokenType)
            {
                case JTokenType.String:
                    return NpgsqlDbType.Text;
                case JTokenType.Integer:
                    return NpgsqlDbType.Integer;
                case JTokenType.Float:
                    return NpgsqlDbType.Numeric;
                case JTokenType.Date:
                    if (DateTimeOffset.TryParse(value.ToString(), out DateTimeOffset dateTimeWithTimeZone))
                    {
                        return NpgsqlDbType.TimestampTz;
                    }
                    return NpgsqlDbType.Timestamp;
                case JTokenType.Boolean:
                    return NpgsqlDbType.Boolean;
                case JTokenType.Bytes:
                    return NpgsqlDbType.Bytea;
                case JTokenType.Guid:
                    return NpgsqlDbType.Uuid;
                case JTokenType.TimeSpan:
                    return NpgsqlDbType.Interval;
                case JTokenType.Null:
                    return NpgsqlDbType.Unknown;
                // Handle other data types as needed

                default:
                    throw new ArgumentOutOfRangeException(nameof(jTokenType), jTokenType, "Unsupported JTokenType");
            }
        }
        // This method is now here for reference in maintaining method below
        public static object ConvertJTokenToType(JToken token, JTokenType tokenType, NpgsqlDbType npgsqlType)
        {

            if (npgsqlType == NpgsqlDbType.Uuid) 
            {
                tokenType = JTokenType.Guid;
            }

            switch (tokenType)
            {
                case JTokenType.String:
                    return token.ToObject<string>();
                case JTokenType.Integer:
                    return token.ToObject<int>();
                case JTokenType.Float:
                    return token.ToObject<float>();
                case JTokenType.Date:

                    if (DateTimeOffset.TryParse(token.ToString(), out DateTimeOffset dateTimeWithTimeZone))
                    {
                        return token.ToObject<DateTimeOffset>();
                    }

                    return token.ToObject<DateTime>();
                case JTokenType.Boolean:
                    return token.ToObject<bool>();
                case JTokenType.Bytes:
                    return Convert.FromBase64String(token.ToObject<string>());
                case JTokenType.Guid:
                    return token.ToObject<Guid>();
                case JTokenType.TimeSpan:
                    return token.ToObject<TimeSpan>();
                case JTokenType.Null:
                    return null;
                default:
                    throw new ArgumentException($"Unsupported JTokenType: {tokenType}");
            }
        }

        public static (object, NpgsqlDbType) ConvertBasedOnColumnType(Dictionary<string, string> columnTypes, string colToConvertName, JToken dataToConvert)
        {
            // Check if the specified column exists in the dictionary
            if (columnTypes.TryGetValue(colToConvertName, out string postgresColumnType))
            {
                // Convert the data based on the PostgreSQL column type
                switch (postgresColumnType.ToLower())
                {
                    case "integer":
                        return (Convert.ToInt32(dataToConvert), NpgsqlDbType.Integer);

                    case "text":
                    case "character varying":
                        return (Convert.ToString(dataToConvert), NpgsqlDbType.Text);

                    case "timestamp":
                    case "timestamptz":
                    case "timestamp with time zone":
                        if (DateTimeOffset.TryParse(dataToConvert.ToString(), out DateTimeOffset dateTimeWithTimeZone))
                        {
                            return (dataToConvert.ToObject<DateTimeOffset>(), NpgsqlDbType.TimestampTz);
                        }

                        return (dataToConvert.ToObject<DateTimeOffset>(), NpgsqlDbType.Timestamp);

                    case "double precision":
                        return (Convert.ToDouble(dataToConvert), NpgsqlDbType.Double);

                    case "uuid":
                        return (Guid.Parse(dataToConvert.ToString()), NpgsqlDbType.Uuid);

                    case "boolean":
                        return (Convert.ToBoolean(dataToConvert), NpgsqlDbType.Boolean);

                    // Add more cases for other PostgreSQL types as needed

                    default:
                        throw new ArgumentException($"Unsupported or unknown PostgreSQL column type: {postgresColumnType}");
                }
            }
            else
            {
                // Column not found in the dictionary
                throw new ArgumentException($"Column '{colToConvertName}' not found in the column types dictionary");
            }
        }
    }
}
