using Newtonsoft.Json;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class DisciplinaResposta
    {
        [JsonProperty("codDisciplina")]
        public long CodigoComponenteCurricular { get; set; }

        [JsonProperty("compartilhada")]
        public bool Compartilhada { get; set; }

        [JsonProperty("codDisciplinaPai")]
        public long? CodigoComponenteCurricularPai { get; set; }

        [JsonProperty("disciplina")]
        public string Nome { get; set; }

        [JsonProperty("regencia")]
        public bool Regencia { get; set; }

        [JsonProperty("registrofrequencia")]
        public bool RegistroFrequencia { get; set; }

        [JsonProperty("territoriosaber")]
        public bool TerritorioSaber { get; set; }

        [JsonProperty("lancaNota")]
        public bool LancaNota { get; set; }
    }
}