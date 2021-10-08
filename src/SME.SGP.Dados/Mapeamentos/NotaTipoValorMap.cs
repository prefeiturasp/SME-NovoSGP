using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaTipoValorMap : BaseMap<NotaTipoValor>
    {
        public NotaTipoValorMap()
        {
            ToTable("notas_tipo_valor");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.TipoNota).ToColumn("tipo_nota");
        }
    }
}