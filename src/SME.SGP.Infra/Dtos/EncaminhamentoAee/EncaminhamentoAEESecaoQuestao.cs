using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAEESecaoQuestao
    {
        public long QuestaoId { get; set; }
        public TipoQuestao TipoQuestao { get; set; } //resposta, texto, arquivo
        public string Resposta { get; set; }
    }
}
