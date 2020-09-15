using System.Text.Json.Serialization;

namespace SME.SGP.Infra
{
    public class TurmaParaCopiaPlanoAnualDto
    {
        [JsonPropertyName("nomeTurma")]
        public string Nome { get; set; }

        public bool PossuiPlano { get; set; }

        [JsonPropertyName("codTurma")]
        public int TurmaId { get; set; }
    }
}