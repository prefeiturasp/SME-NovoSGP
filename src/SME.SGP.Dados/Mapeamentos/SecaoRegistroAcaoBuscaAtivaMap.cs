using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class SecaoRegistroAcaoBuscaAtivaMap : BaseMap<SecaoRegistroAcaoBuscaAtiva>
    {
        public SecaoRegistroAcaoBuscaAtivaMap()
        {
            ToTable("secao_registro_acao_busca_ativa");
            Map(c => c.QuestionarioId).ToColumn("questionario_id");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Etapa).ToColumn("etapa");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.NomeComponente).ToColumn("nome_componente");
        }
    }
}
