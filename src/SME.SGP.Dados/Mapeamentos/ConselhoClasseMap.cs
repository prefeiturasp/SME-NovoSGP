using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class ConselhoClasseMap : BaseEntityMap<ConselhoClasse>
    {
        public ConselhoClasseMap()
        {
            ToTable("conselho_classe");
            Map(nameof(ConselhoClasse.FechamentoTurmaId), "fechamento_turma_id");
            Map(nameof(ConselhoClasse.Situacao), "situacao");
            Map(nameof(ConselhoClasse.Excluido), "excluido");
            Map(nameof(ConselhoClasse.Migrado), "migrado");
        }
    }
}