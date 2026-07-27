using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoNotaMap : SimpleEntityMap<ConselhoClasseConsolidadoTurmaAlunoNota>
    {
        public ConselhoClasseConsolidadoTurmaAlunoNotaMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma_nota");
            Map(nameof(ConselhoClasseConsolidadoTurmaAlunoNota.ConselhoClasseConsolidadoTurmaAlunoId), "consolidado_conselho_classe_aluno_turma_id");
            Map(nameof(ConselhoClasseConsolidadoTurmaAlunoNota.Bimestre), "bimestre");
            Map(nameof(ConselhoClasseConsolidadoTurmaAlunoNota.Nota), "nota");
            Map(nameof(ConselhoClasseConsolidadoTurmaAlunoNota.ConceitoId), "conceito_id");
            Map(nameof(ConselhoClasseConsolidadoTurmaAlunoNota.ComponenteCurricularId), "componente_curricular_id");
        }
    }
}
