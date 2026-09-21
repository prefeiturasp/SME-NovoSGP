using SME.SGP.Dominio;
using SME.SGP.Dados.Mapeamentos;
namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPASecaoMap : BaseMap<EncaminhamentoNAAPASecao>
    {
        public EncaminhamentoNAAPASecaoMap()
        {
            ToTable("encaminhamento_naapa_secao");
            Map(nameof(EncaminhamentoNAAPASecao.EncaminhamentoNAAPAId), "encaminhamento_naapa_id");
            Map(nameof(EncaminhamentoNAAPASecao.SecaoEncaminhamentoNAAPAId), "secao_encaminhamento_id");
            Map(nameof(EncaminhamentoNAAPASecao.Concluido), "concluido");
            Map(nameof(EncaminhamentoNAAPASecao.Excluido), "excluido");
        }
    }
}