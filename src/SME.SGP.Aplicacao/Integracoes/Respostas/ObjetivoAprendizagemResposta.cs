using System;
using System.Text.Json.Serialization;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class ObjetivoAprendizagemResposta
    {
        [JsonPropertyName("year_id")]
        public string Ano { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime AtualizadoEm { get; set; }

        [JsonPropertyName("code")]
        public string Codigo { get; set; }

        [JsonPropertyName("curricular_component_id")]
        public long ComponenteCurricularId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CriadoEm { get; set; }

        [JsonPropertyName("description")]
        public string Descricao { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}