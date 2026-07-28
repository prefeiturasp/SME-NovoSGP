using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class SecaoEncaminhamentoNAAPAModalidadeMap : BaseEntityMap<SecaoEncaminhamentoNAAPAModalidade>
    {
        public SecaoEncaminhamentoNAAPAModalidadeMap()
        {
            ToTable("secao_encaminhamento_naapa_modalidade");

            Map(nameof(SecaoEncaminhamentoNAAPAModalidade.SecaoEncaminhamentoNAAPAId), "secao_encaminhamento_id");
            Map(nameof(SecaoEncaminhamentoNAAPAModalidade.Modalidade), "modalidade_codigo");
            Map(nameof(SecaoEncaminhamentoNAAPAModalidade.Excluido), "excluido");
        }
    }
}