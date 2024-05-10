using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioPAPAlunoConselhoClasseDto
    {
        public RelatorioPAPAlunoConselhoClasseDto()
        {}
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
        public long PeriodoRelatorioPAPId { get; set; }
    }
}
