using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaParametroMap : BaseMap<NotaParametro>
    {
        public NotaParametroMap()
        {
            ToTable("notas_parametros");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.Incremento).ToColumn("incremento");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.Maxima).ToColumn("valor_maximo");
            Map(c => c.Media).ToColumn("valor_medio");
            Map(c => c.Minima).ToColumn("valor_minimo");
        }
    }
}