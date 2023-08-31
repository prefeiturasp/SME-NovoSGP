using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Mapeamentos
{
    public class RelatorioPeriodicoPAPRespostaMap : BaseMap<RelatorioPeriodicoPAPResposta>
    {
        public RelatorioPeriodicoPAPRespostaMap()
        {
            ToTable("relatorio_periodico_pap_resposta");

            Map(c => c.RelatorioPeriodicoQuestaoId).ToColumn("relatorio_periodico_pap_questao_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
