using SME.SGP.Dominio.Entidades;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseParecerMap : BaseMap<ConselhoClasseParecer>
    {
        public ConselhoClasseParecerMap()
        {
            ToTable("conselho_classe_parecer");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.Aprovado).ToColumn("aprovado");
            Map(c => c.Frequencia).ToColumn("frequencia");
            Map(c => c.Conselho).ToColumn("conselho");
            Map(c => c.InicioVigencia).ToColumn("inicio_vigencia");
            Map(c => c.FimVigencia).ToColumn("fim_vigencia");
            Map(c => c.Nota).ToColumn("nota");
        }
    }
}