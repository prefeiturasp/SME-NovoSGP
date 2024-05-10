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
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Observacao).ToColumn("observacao");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
