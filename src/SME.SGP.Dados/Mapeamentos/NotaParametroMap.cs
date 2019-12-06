using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaParametroMap : BaseMap<NotaParametro>
    {
        public NotaParametroMap()
        {
            ToTable("notas_parametros");
            Map(x => x.Minima).ToColumn("valor_minimo");
            Map(x => x.Media).ToColumn("valor_medio");
            Map(x => x.Maxima).ToColumn("valor_maximo");
            Map(x => x.InicioVigencia).ToColumn("inicio_vigencia");
            Map(x => x.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}