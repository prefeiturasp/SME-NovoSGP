using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FiltroRelatorioPlanosAEEDto
    {
        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string[] CodigosTurma { get; set; }
        public int Situacao { get; set; }
        public bool ExibirEncerrados { get; set; }
        public string[] CodigosResponsavel { get; set; }
        public string PAAIResponsavel { get; set; }
    }
}
