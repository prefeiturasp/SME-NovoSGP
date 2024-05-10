using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class SecaoTurmaAlunoPAPDto
    {
        public SecaoTurmaAlunoPAPDto()
        {
            Secoes = new List<SecaoPAPDto>();
        }
            
        public long? PAPTurmaId { get; set; }
        public long? PAPAlunoId { get; set; }
        public List<SecaoPAPDto> Secoes { get; set; }
    }
}
