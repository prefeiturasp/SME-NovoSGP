using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class CopiarPapDto
    {
        public long PeriodoRelatorioPAPId { get; set; }
        public long RelatorioPapId { get; set; }
        public string CodigoTurma { get; set; }
        public IEnumerable<CopiarPapEstudantesDto> Estudantes { get; set; } = new List<CopiarPapEstudantesDto>();
        public IEnumerable<CopiarSecaoDto> Secoes { get; set; } = new List<CopiarSecaoDto>();
    }
}