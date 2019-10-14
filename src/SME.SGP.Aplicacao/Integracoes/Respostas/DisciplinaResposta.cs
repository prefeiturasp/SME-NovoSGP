using Newtonsoft.Json;

namespace SME.SGP.Aplicacao.Integracoes.Respostas
{
    public class DisciplinaResposta
    {
        [JsonProperty("codDisciplina")]
        public int CodigoComponenteCurricular { get; set; }

        [JsonProperty("disciplina")]
        public string Nome { get; set; }

        [JsonProperty("regencia")]
        public bool Regencia { get; set; }
    }
}