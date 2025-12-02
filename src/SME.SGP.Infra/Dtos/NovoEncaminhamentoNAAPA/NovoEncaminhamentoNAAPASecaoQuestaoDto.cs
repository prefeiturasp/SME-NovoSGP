using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class NovoEncaminhamentoNAAPASecaoQuestaoDto
    {
        public long RespostaEncaminhamentoId { get; set; }
        public long QuestaoId { get; set; }
        public TipoQuestao TipoQuestao { get; set; }
        public string Resposta { get; set; }
    }
}
