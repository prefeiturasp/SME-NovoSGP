using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class HistoricoNotaConselhoClasseMap : SimpleMap<HistoricoNotaConselhoClasse>
    {
        public HistoricoNotaConselhoClasseMap()
        {
            ToTable("historico_nota_conselho_classe");
            Map(nameof(HistoricoNotaConselhoClasse.HistoricoNotaId), "historico_nota_id");
            Map(nameof(HistoricoNotaConselhoClasse.ConselhoClasseNotaId), "conselho_classe_nota_id");
        }
    }
}