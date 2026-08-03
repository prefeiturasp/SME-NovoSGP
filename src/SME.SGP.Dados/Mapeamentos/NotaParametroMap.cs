using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaParametroMap : BaseMap<NotaParametro>
    {
        public NotaParametroMap()
        {
            ToTable("notas_parametros");
            Map(nameof(NotaParametro.Ativo), "ativo");
            Map(nameof(NotaParametro.FimVigencia), "fim_vigencia");
            Map(nameof(NotaParametro.Incremento), "incremento");
            Map(nameof(NotaParametro.InicioVigencia), "inicio_vigencia");
            Map(nameof(NotaParametro.Maxima), "valor_maximo");
            Map(nameof(NotaParametro.Media), "valor_medio");
            Map(nameof(NotaParametro.Minima), "valor_minimo");
        }
    }
}