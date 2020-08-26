using Newtonsoft.Json;

namespace SME.SGP.Infra
{
    public class DisciplinaDto
    {
        public long CodigoComponenteCurricular { get; set; }
        [JsonProperty("codDisciplinaPai")]
        public long? CdComponenteCurricularPai { get; set; }
        public bool Compartilhada { get; set; }
        public string Nome { get; set; }
        public bool PossuiObjetivos { get; set; }
        public bool Regencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool TerritorioSaber { get; set; }
        public bool LancaNota { get; set; }
        public bool ObjetivosAprendizagemOpcionais { get; set; }
    }
}