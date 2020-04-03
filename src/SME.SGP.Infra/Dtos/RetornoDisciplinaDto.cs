using Newtonsoft.Json;

namespace SME.SGP.Infra
{
    public class RetornoDisciplinaDto
    {
        public long CdComponenteCurricular { get; set; }
        [JsonProperty("codDisciplinaPai")]
        public int? CdComponenteCurricularPai { get; set; }
        public string Descricao { get; set; }
        public bool EhCompartilhada { get; set; }
        public bool EhRegencia { get; set; }
        public bool RegistraFrequencia { get; set; }
        public bool Territorio { get; set; }
        public bool LancaNota { get; set; }
    }
}