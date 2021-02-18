using Newtonsoft.Json;

namespace SME.SGP.Infra
{
    public class TurmaParaCopiaPlanoAnualDto
    {
        [JsonProperty("nomeTurma")]
        public string Nome { get; set; }

        public bool PossuiPlano { get; set; }

        [JsonProperty("codTurma")]
        public long TurmaId { get; set; }

        public long Id { get; set; }

        public int CodigoComponenteCurricular { get; set; }

        public int bimestre { get; set; }
    }
}