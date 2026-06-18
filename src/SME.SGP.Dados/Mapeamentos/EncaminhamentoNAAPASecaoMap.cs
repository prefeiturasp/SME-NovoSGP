using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class EncaminhamentoNAAPASecaoMap : BaseMap<EncaminhamentoNAAPASecao>
    {
        public EncaminhamentoNAAPASecaoMap()
        {
            ToTable("encaminhamento_naapa_secao");
            Map(c => c.EncaminhamentoNAAPAId).ToColumn("encaminhamento_naapa_id");
            Map(c => c.SecaoEncaminhamentoNAAPAId).ToColumn("secao_encaminhamento_id");
            Map(c => c.Concluido).ToColumn("concluido");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
