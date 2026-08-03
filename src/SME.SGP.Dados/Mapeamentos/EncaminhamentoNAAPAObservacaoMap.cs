using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;

namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPAObservacaoMap : BaseMap<EncaminhamentoNAAPAObservacao>
    {
        public EncaminhamentoNAAPAObservacaoMap()
        {
            ToTable("encaminhamento_naapa_observacao");
            Map(nameof(EncaminhamentoNAAPAObservacao.EncaminhamentoNAAPAId), "encaminhamento_naapa_id");
            Map(nameof(EncaminhamentoNAAPAObservacao.Excluido), "excluido");
            Map(nameof(EncaminhamentoNAAPAObservacao.Observacao), "observacao");
        }
    }
}