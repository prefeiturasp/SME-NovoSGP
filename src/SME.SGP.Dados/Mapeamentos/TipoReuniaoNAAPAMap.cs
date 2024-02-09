using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoReuniaoNAAPAMap : BaseMap<TipoReuniaoNAAPA>
    {
        public TipoReuniaoNAAPAMap()
        {
            ToTable("tipo_reuniao_naapa");
            Map(c => c.Titulo).ToColumn("titulo");
            Map(c => c.Excluido).ToColumn("excluido");
        }
    }
}
