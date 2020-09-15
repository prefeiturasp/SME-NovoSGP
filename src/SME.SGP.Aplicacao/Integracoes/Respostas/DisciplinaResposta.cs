using System.Text.Json.Serialization;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class DisciplinaResposta
    {
        [JsonPropertyName("codDisciplina")]
        public long CodigoComponenteCurricular { get; set; }

        [JsonPropertyName("compartilhada")]
        public bool Compartilhada { get; set; }

        [JsonPropertyName("codDisciplinaPai")]
        public long? CodigoComponenteCurricularPai { get; set; }

        [JsonPropertyName("disciplina")]
        public string Nome { get; set; }

        [JsonPropertyName("regencia")]
        public bool Regencia { get; set; }

        [JsonPropertyName("registrofrequencia")]
        public bool RegistroFrequencia { get; set; }

        [JsonPropertyName("territoriosaber")]
        public bool TerritorioSaber { get; set; }

        [JsonPropertyName("lancaNota")]
        public bool LancaNota { get; set; }

        [JsonPropertyName("baseNacional")]
        public bool BaseNacional { get; set; }

        [JsonPropertyName("grupoMatriz")]
        public GrupoMatriz GrupoMatriz { get; set; }
    }
}