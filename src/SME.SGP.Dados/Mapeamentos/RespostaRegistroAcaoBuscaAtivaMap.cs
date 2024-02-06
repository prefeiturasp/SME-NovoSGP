using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RespostaRegistroAcaoBuscaAtivaMap : BaseMap<RespostaRegistroAcaoBuscaAtiva>
    {
        public RespostaRegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa_resposta");
            Map(c => c.QuestaoRegistroAcaoBuscaAtivaId).ToColumn("questao_registro_acao_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
