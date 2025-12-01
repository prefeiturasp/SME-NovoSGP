using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA
{
    public class RespostaQuestaoNovoEncaminhamentoNAAPADto
    {
        public long Id { get; set; }
        public long QuestaoId { get; set; }
        public long? RespostaId { get; set; }
        public string Texto { get; set; }
        public Arquivo Arquivo { get; set; }
    }
}