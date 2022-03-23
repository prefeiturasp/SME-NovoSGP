using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ParametrosSistemaMap : BaseMap<ParametrosSistema>
    {
        public ParametrosSistemaMap()
        {
            ToTable("parametros_sistema");
            Map(c => c.Ano).ToColumn("ano");
            Map(c => c.Ativo).ToColumn("ativo");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.Valor).ToColumn("valor");
        }
    }
}