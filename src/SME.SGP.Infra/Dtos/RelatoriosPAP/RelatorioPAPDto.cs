using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class RelatorioPAPDto
    {
        public RelatorioPAPDto()
        {
            Secoes = new List<RelatorioPAPSecaoDto>();
        }
        public long periodoRelatorioPAPId { get; set; }
        public long TurmaId { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public long? PAPTurmaId { get; set; }
        public long? PAPAlunoId { get; set; }
        public List<RelatorioPAPSecaoDto> Secoes { get; set; }
    }
}
