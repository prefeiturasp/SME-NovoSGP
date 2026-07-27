using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EncaminhamentoAEESecaoMap : BaseEntityMap<EncaminhamentoAEESecao>
    {
        public EncaminhamentoAEESecaoMap()
        {
            ToTable("encaminhamento_aee_secao");
            Map(nameof(EncaminhamentoAEESecao.EncaminhamentoAEEId), "encaminhamento_aee_id");
            Map(nameof(EncaminhamentoAEESecao.SecaoEncaminhamentoAEEId), "secao_encaminhamento_id");
            Map(nameof(EncaminhamentoAEESecao.Concluido), "concluido");
            Map(nameof(EncaminhamentoAEESecao.Excluido), "excluido");
        }
    }
}