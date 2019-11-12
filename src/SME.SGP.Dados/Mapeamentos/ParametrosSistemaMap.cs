using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ParametrosSistemaMap : BaseMap<ParametrosSistema>
    {
        public ParametrosSistemaMap()
        {
            ToTable("disciplina_plano");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Valor).ToColumn("valor");
            Map(c => c.Ano).ToColumn("ano");
        }
    }
}