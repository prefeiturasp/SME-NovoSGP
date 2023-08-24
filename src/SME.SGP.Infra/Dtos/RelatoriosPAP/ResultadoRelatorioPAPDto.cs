using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ResultadoRelatorioPAPDto
    {
        public ResultadoRelatorioPAPDto()
        {
            Secoes = new List<ResultadoRelatorioPAPSecaoDto>();
        }
        public long PAPTurmaId { get; set; }
        public long PAPAlunoId { get; set; }
        public List<ResultadoRelatorioPAPSecaoDto> Secoes { get; set; }
    }
}
