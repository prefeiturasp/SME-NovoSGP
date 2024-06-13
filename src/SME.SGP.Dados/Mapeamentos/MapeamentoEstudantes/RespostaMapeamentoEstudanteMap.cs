using SME.SGP.Dominio;
namespace SME.SGP.Dados
{
    public class RespostaMapeamentoEstudanteMap : BaseMap<RespostaMapeamentoEstudante>
    {
        public RespostaMapeamentoEstudanteMap()
        {
            ToTable("mapeamento_estudante_resposta");
            Map(c => c.QuestaoMapeamentoEstudanteId).ToColumn("questao_mapeamento_estudante_id");
            Map(c => c.RespostaId).ToColumn("resposta_id");
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.Texto).ToColumn("texto");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
