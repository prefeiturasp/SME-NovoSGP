using SME.SGP.Dominio;

namespace SME.SGP.Dados
{
    public class ConselhoClasseConsolidadoTurmaAlunoMap : BaseEntityMap<ConselhoClasseConsolidadoTurmaAluno>
    {
        public ConselhoClasseConsolidadoTurmaAlunoMap()
        {
            ToTable("consolidado_conselho_classe_aluno_turma");
            Map(nameof(ConselhoClasseConsolidadoTurmaAluno.DataAtualizacao), "dt_atualizacao");
            Map(nameof(ConselhoClasseConsolidadoTurmaAluno.Status), "status");
            Map(nameof(ConselhoClasseConsolidadoTurmaAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(ConselhoClasseConsolidadoTurmaAluno.ParecerConclusivoId), "parecer_conclusivo_id");
            Map(nameof(ConselhoClasseConsolidadoTurmaAluno.TurmaId), "turma_id");
        }
    }
}