using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class QuestaoRegistroAcaoBuscaAtivaMap : BaseMap<QuestaoRegistroAcaoBuscaAtiva>
    {
        public QuestaoRegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa_questao");
            Map(nameof(QuestaoRegistroAcaoBuscaAtiva.RegistroAcaoBuscaAtivaSecaoId), "registro_acao_busca_ativa_secao_id");
            Map(nameof(QuestaoRegistroAcaoBuscaAtiva.QuestaoId), "questao_id");
            Map(nameof(QuestaoRegistroAcaoBuscaAtiva.Excluido), "excluido");
        }
    }
}