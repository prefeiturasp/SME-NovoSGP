using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoModalidadeMap : BaseMap<InformativoModalidade>
    {
        public InformativoModalidadeMap()
        {
            ToTable("informativo_modalidade");
            Map(nameof(InformativoModalidade.InformativoId), "informativo_id");
            Map(nameof(InformativoModalidade.Modalidade), "modalidade_codigo");
        }
    }
}