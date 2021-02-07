using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class PlanoAEERespostaMap : BaseMap<PlanoAEEResposta>
    {
        public PlanoAEERespostaMap()
        {
            ToTable("plano_aee_resposta");
            Map(a => a.PlanoAEEQuestao).ToColumn("plano_questao_id");
            Map(a => a.RespostaId).ToColumn("resposta_id");
            Map(a => a.ArquivoId).ToColumn("arquivo_id");
            Map(a => a.PeriodoInicio).ToColumn("periodo_inicio");
            Map(a => a.PeriodoFim).ToColumn("periodo_fim");
        }
    }
}
