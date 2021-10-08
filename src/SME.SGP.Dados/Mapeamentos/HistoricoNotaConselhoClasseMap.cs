using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaConselhoClasseMap : DommelEntityMap<HistoricoNotaConselhoClasse>
    {
        public HistoricoNotaConselhoClasseMap()
        {
            ToTable("historico_nota_conselho_classe");
            Map(c => c.Id).ToColumn("id");
            Map(c => c.HistoricoNotaId).ToColumn("historico_nota_id");
            Map(c => c.ConselhoClasseNotaId).ToColumn("conselho_classe_nota_id");
        }
    }
}
