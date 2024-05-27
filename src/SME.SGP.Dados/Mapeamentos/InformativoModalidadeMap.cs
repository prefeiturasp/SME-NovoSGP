using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class InformativoModalidadeMap : BaseMap<InformativoModalidade>
    {
        public InformativoModalidadeMap()
        {
            ToTable("informativo");
            Map(c => c.InformativoId).ToColumn("informativo_id");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
        }
    }
}
