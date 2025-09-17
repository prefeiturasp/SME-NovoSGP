using SME.SGP.Dominio;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SME.SGP.Infra.Dtos.PainelEducacional
{
    public class ContagemNumeroAlunosPapDto
    {
        [JsonIgnore]
        public TipoPap TipoPap { get; set; }

        [JsonProperty("TipoPap")]
        [JsonConverter(typeof(TipoPapJsonConverter))]
        public TipoPap TipoPapValue
        {
            get => TipoPap;
            set => TipoPap = value;
        }

        [JsonIgnore]
        public string TipoPapString
        {
            get => ObterNomeTipoPap(TipoPap);
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    TipoPap = TipoPap.PapColaborativo;
                    return;
                }
                TipoPap = MapearStringParaTipoPap(value);
            }
        }

        [JsonProperty("QuantidadeTurmas")]
        public int QuantidadeTurmas { get; set; }

        [JsonProperty("QuantidadeEstudantes")]
        public int QuantidadeEstudantes { get; set; }

        [JsonProperty("QuantidadeEstudantesComMenosDe75PorcentoFrequencia")]
        public int QuantidadeEstudantesComMenosDe75PorcentoFrequencia { get; set; }

        private static TipoPap MapearStringParaTipoPap(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return TipoPap.PapColaborativo;

            return valor.Trim() switch
            {
                "PAP 2º ano" => TipoPap.Pap2Ano,
                "PAP Colaborativo" => TipoPap.PapColaborativo,
                "Recuperação de Aprendizagens" => TipoPap.RecuperacaoAprendizagens,
                var v when v.Equals("PAP 2º ano", StringComparison.OrdinalIgnoreCase) => TipoPap.Pap2Ano,
                var v when v.Equals("PAP 2° ano", StringComparison.OrdinalIgnoreCase) => TipoPap.Pap2Ano,
                var v when v.Equals("PAP Colaborativo", StringComparison.OrdinalIgnoreCase) => TipoPap.PapColaborativo,
                var v when v.Equals("Recuperação de Aprendizagens", StringComparison.OrdinalIgnoreCase) => TipoPap.RecuperacaoAprendizagens,
                _ => MapearComNormalizacao(valor)
            };
        }

        private static TipoPap MapearComNormalizacao(string valor)
        {
            var normalized = Normalizar(valor);
            return normalized switch
            {
                "pap 2 ano" or "pap segundo ano" => TipoPap.Pap2Ano,
                "pap colaborativo" => TipoPap.PapColaborativo,
                "recuperacao de aprendizagens" => TipoPap.RecuperacaoAprendizagens,
                _ => TipoPap.PapColaborativo
            };
        }

        private static string Normalizar(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return string.Empty;

            var normalized = valor.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(c);
                if ((category != UnicodeCategory.NonSpacingMark) && (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)))
                {
                    sb.Append(c);
                }
            }

            var cleaned = Regex.Replace(sb.ToString(), @"\s+", " ").ToLowerInvariant().Trim();
            return cleaned;
        }

        private static string ObterNomeTipoPap(TipoPap tipoPap)
        {
            var field = typeof(TipoPap).GetField(tipoPap.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.Name ?? tipoPap.ToString();
        }

        public void SetTipoPap(TipoPap tipo)
        {
            TipoPap = Enum.IsDefined(typeof(TipoPap), tipo) ? tipo : TipoPap.PapColaborativo; 
        }
    }

    public class TipoPapJsonConverter : JsonConverter<TipoPap>
    {
        public override TipoPap ReadJson(JsonReader reader, Type objectType, TipoPap existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return TipoPap.PapColaborativo;

            if (reader.TokenType == JsonToken.Integer)
            {
                var enumValue = Convert.ToInt32(reader.Value);
                return Enum.IsDefined(typeof(TipoPap), enumValue) ? (TipoPap)enumValue : TipoPap.PapColaborativo; 
            }

            if (reader.TokenType == JsonToken.String)
            {
                var stringValue = reader.Value.ToString();
                return MapearStringParaTipoPap(stringValue);
            }

            return TipoPap.PapColaborativo;
        }

        public override void WriteJson(JsonWriter writer, TipoPap value, JsonSerializer serializer)
        {
            writer.WriteValue((int)value);
        }

        private static TipoPap MapearStringParaTipoPap(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return TipoPap.PapColaborativo;

            return valor.Trim() switch
            {
                "PAP 2º ano" => TipoPap.Pap2Ano,
                "PAP Colaborativo" => TipoPap.PapColaborativo,
                "Recuperação de Aprendizagens" => TipoPap.RecuperacaoAprendizagens,
                var v when v.Equals("PAP 2º ano", StringComparison.OrdinalIgnoreCase) => TipoPap.Pap2Ano,
                var v when v.Equals("PAP 2° ano", StringComparison.OrdinalIgnoreCase) => TipoPap.Pap2Ano,
                var v when v.Equals("PAP Colaborativo", StringComparison.OrdinalIgnoreCase) => TipoPap.PapColaborativo,
                var v when v.Equals("Recuperação de Aprendizagens", StringComparison.OrdinalIgnoreCase) => TipoPap.RecuperacaoAprendizagens,
                _ => TipoPap.PapColaborativo
            };
        }
    }
}