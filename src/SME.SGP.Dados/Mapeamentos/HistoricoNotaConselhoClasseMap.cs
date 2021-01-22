using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaConselhoClasseMap : DommelEntityMap<HistoricoNotaConselhoClasse>
    {
        public HistoricoNotaConselhoClasseMap()
        {
            ToTable("historico_nota_conselho_classe");
            Map(e => e.HistoricoNotaId).ToColumn("historico_nota_id");
            Map(e => e.ConselhoClasseNotaId).ToColumn("conselho_classe_nota_id");
        }
    }
}
