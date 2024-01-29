using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class SecaoEncaminhamentoNAAPAModalidadeMap : BaseMap<SecaoEncaminhamentoNAAPAModalidade>
    {
        public SecaoEncaminhamentoNAAPAModalidadeMap()
        {
            ToTable("secao_encaminhamento_naapa_modalidade");
            Map(c => c.SecaoEncaminhamentoNAAPAId).ToColumn("secao_encaminhamento_id");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
