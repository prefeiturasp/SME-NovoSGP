using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ParametrosSistemaMap : BaseMap<ParametrosSistema>
    {
        public ParametrosSistemaMap()
        {
            ToTable("parametros_sistema");
            Map(nameof(ParametrosSistema.Ano), "ano");
            Map(nameof(ParametrosSistema.Ativo), "ativo");
            Map(nameof(ParametrosSistema.Descricao), "descricao");
            Map(nameof(ParametrosSistema.Nome), "nome");
            Map(nameof(ParametrosSistema.Tipo), "tipo");
            Map(nameof(ParametrosSistema.Valor), "valor");
        }
    }
}