using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoReuniaoNAAPAMap : BaseMap<TipoReuniaoNAAPA>
    {
        public TipoReuniaoNAAPAMap()
        {
            ToTable("tipo_reuniao_naapa");

            Map(nameof(TipoReuniaoNAAPA.Titulo), "titulo");
            Map(nameof(TipoReuniaoNAAPA.Excluido), "excluido");
        }
    }
}