using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RegistroAcaoBuscaAtivaSecaoMap : BaseMap<RegistroAcaoBuscaAtivaSecao>
    {
        public RegistroAcaoBuscaAtivaSecaoMap()
        {
            ToTable("registro_acao_busca_ativa_secao");
            Map(c => c.RegistroAcaoBuscaAtivaId).ToColumn("registro_acao_busca_ativa_id");
            Map(c => c.SecaoRegistroAcaoBuscaAtivaId).ToColumn("secao_registro_acao_id");
            Map(c => c.Concluido).ToColumn("concluido");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
