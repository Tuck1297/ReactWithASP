﻿using Newtonsoft.Json.Linq;
using NpgsqlTypes;

namespace ReactWithASP.Server.Helpers
{
    public class DbTypeConverter
    {
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
    }
}
