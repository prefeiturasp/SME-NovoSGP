using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class RespostaRegistroAcaoBuscaAtivaMap : BaseEntityMap<RespostaRegistroAcaoBuscaAtiva>
    {
        public RespostaRegistroAcaoBuscaAtivaMap()
        {
            ToTable("registro_acao_busca_ativa_resposta");

            Map(nameof(RespostaRegistroAcaoBuscaAtiva.QuestaoRegistroAcaoBuscaAtivaId), "questao_registro_acao_id");
            Map(nameof(RespostaRegistroAcaoBuscaAtiva.RespostaId), "resposta_id");
            Map(nameof(RespostaRegistroAcaoBuscaAtiva.ArquivoId), "arquivo_id");
            Map(nameof(RespostaRegistroAcaoBuscaAtiva.Texto), "texto");
            Map(nameof(RespostaRegistroAcaoBuscaAtiva.Excluido), "excluido");
        }
    }
}