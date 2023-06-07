using SME.SGP.Dominio;
namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPAObservacaoMap : BaseMap<EncaminhamentoNAAPAObservacao>
    {
        public EncaminhamentoNAAPAObservacaoMap()
        {
            ToTable("encaminhamento_naapa_observacao");
            Map(c => c.EncaminhamentoNAAPAId).ToColumn("encaminhamento_naapa_id");
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Observacao).ToColumn("observacao");
        }
    }
}