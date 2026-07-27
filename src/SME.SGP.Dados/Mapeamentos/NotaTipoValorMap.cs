using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class NotaTipoValorMap : BaseEntityMap<NotaTipoValor>
    {
        public NotaTipoValorMap()
        {
            ToTable("notas_tipo_valor");
            Map(nameof(NotaTipoValor.Ativo), "ativo");
            Map(nameof(NotaTipoValor.Descricao), "descricao");
            Map(nameof(NotaTipoValor.FimVigencia), "fim_vigencia");
            Map(nameof(NotaTipoValor.InicioVigencia), "inicio_vigencia");
            Map(nameof(NotaTipoValor.TipoNota), "tipo_nota");
        }
    }
}