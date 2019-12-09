using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaTipoValorMap : BaseMap<NotaTipoValor>
    {
        public NotaTipoValorMap()
        {
            ToTable("notas_tipo_valor");
            Map(n => n.TipoNota).ToColumn("tipo_nota");
            Map(n => n.InicioVigencia).ToColumn("inicio_vigencia");
            Map(n => n.FimVigencia).ToColumn("fim_vigencia");
        }
    }
}