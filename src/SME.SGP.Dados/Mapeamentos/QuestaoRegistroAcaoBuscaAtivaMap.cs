using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class QuestaoRegistroAcaoBuscaAtivaMap : BaseMap<QuestaoRegistroAcaoBuscaAtiva>
    {
        public QuestaoRegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa_questao");
            Map(c => c.RegistroAcaoBuscaAtivaSecaoId).ToColumn("registro_acao_busca_ativa_secao_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
