using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class RegistroAcaoBuscaAtivaSecaoMap : BaseMap<RegistroAcaoBuscaAtivaSecao>
    {
        public RegistroAcaoBuscaAtivaSecaoMap()
        {
            ToTable("registro_acao_busca_ativa_secao");
            Map(nameof(RegistroAcaoBuscaAtivaSecao.RegistroAcaoBuscaAtivaId), "registro_acao_busca_ativa_id");
            Map(nameof(RegistroAcaoBuscaAtivaSecao.SecaoRegistroAcaoBuscaAtivaId), "secao_registro_acao_id");
            Map(nameof(RegistroAcaoBuscaAtivaSecao.Concluido), "concluido");
            Map(nameof(RegistroAcaoBuscaAtivaSecao.Excluido), "excluido");
        }
    }
}