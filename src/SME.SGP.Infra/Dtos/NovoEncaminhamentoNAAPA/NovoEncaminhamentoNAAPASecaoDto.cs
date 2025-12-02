using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class NovoEncaminhamentoNAAPASecaoDto
    {
        public NovoEncaminhamentoNAAPASecaoDto()
        {
            Questoes = new List<NovoEncaminhamentoNAAPASecaoQuestaoDto>();
        }
        public long SecaoId { get; set; }
        public bool Concluido { get; set; }
        public List<NovoEncaminhamentoNAAPASecaoQuestaoDto> Questoes { get; set; }
    }
}