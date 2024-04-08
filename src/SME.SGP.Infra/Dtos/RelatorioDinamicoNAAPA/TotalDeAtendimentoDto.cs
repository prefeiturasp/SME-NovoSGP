using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class TotalDeAtendimentoDto
    {
        public int Total { get; set; }
        public List<TotalAtendimentoQuestaoDto> TotalAtendimentoQuestoes { get; set; }
    }
}
