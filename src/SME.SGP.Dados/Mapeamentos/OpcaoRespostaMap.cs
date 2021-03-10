using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class OpcaoRespostaMap : BaseMap<OpcaoResposta>
    {
        public OpcaoRespostaMap()
        {
            ToTable("opcao_resposta");
            Map(a => a.QuestaoId).ToColumn("questao_id");            
        }
    }
}
