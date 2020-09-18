using Newtonsoft.Json;
using System;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class ObjetivoAprendizagemResposta
    {
        [JsonProperty("year_id")]
        public string Ano { get; set; }

        [JsonProperty("updated_at")]
        public DateTime AtualizadoEm { get; set; }

        [JsonProperty("code")]
        public string Codigo { get; set; }

        [JsonProperty("curricular_component_id")]
        public long ComponenteCurricularId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CriadoEm { get; set; }

        [JsonProperty("description")]
        public string Descricao { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }
}