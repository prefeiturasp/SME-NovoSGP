using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class CopiarPapDto
    {
        public long IdrelatorioPap { get; set; }
        public long IdTurma { get; set; }
        public string[] Estudantes { get; set; }
        public IEnumerable<CopiarSecaoDto> Secoes { get; set; } = new List<CopiarSecaoDto>();
    }
}