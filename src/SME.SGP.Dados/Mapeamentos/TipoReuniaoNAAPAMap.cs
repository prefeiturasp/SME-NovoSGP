using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class TipoReuniaoNAAPAMap : BaseEntityMap<TipoReuniaoNAAPA>
    {
        public TipoReuniaoNAAPAMap()
        {
            ToTable("tipo_reuniao_naapa");

            Map(nameof(TipoReuniaoNAAPA.Titulo), "titulo");
            Map(nameof(TipoReuniaoNAAPA.Excluido), "excluido");
        }
    }
}