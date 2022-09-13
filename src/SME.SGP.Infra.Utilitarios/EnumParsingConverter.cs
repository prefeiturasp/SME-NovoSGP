using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SME.SGP.Infra
{
    public sealed class EnumParsingConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(DictionaryValueConverterInner<>).MakeGenericType(
                    new Type[] { type }));

            return converter;
        }

        private sealed class DictionaryValueConverterInner<TValue> : JsonConverter<TValue> where TValue : struct, Enum
        {
            public override TValue Read(
                ref Utf8JsonReader reader,
                Type typeToConvert,
                JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    var numberValue = reader.GetInt32();
                    return (TValue)(object)numberValue;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    var valueString = reader.GetString();

                    if (Enum.TryParse(valueString, ignoreCase: false, out TValue value)
                        || Enum.TryParse(valueString, ignoreCase: true, out value))
                    {
                        return value;
                    }

                    throw new JsonException(
                        $"Unable to convert \"{valueString}\" to Enum \"{typeof(TValue)}\".");

                }

                throw new JsonException();
            }

            public override void Write(
                Utf8JsonWriter writer,
                TValue value,
                JsonSerializerOptions options) => writer.WriteNumberValue((int)(object)value);
        }
    }
}
