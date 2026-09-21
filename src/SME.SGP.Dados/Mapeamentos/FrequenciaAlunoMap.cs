using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaAlunoMap : BaseMap<FrequenciaAluno>
    {
        public FrequenciaAlunoMap()
        {
            ToTable("frequencia_aluno");
            Map(nameof(FrequenciaAluno.Bimestre), "bimestre");
            Map(nameof(FrequenciaAluno.CodigoAluno), "codigo_aluno");
            Map(nameof(FrequenciaAluno.DisciplinaId), "disciplina_id");
            Map(nameof(FrequenciaAluno.PeriodoEscolarId), "periodo_escolar_id");
            Map(nameof(FrequenciaAluno.PeriodoFim), "periodo_fim");
            Map(nameof(FrequenciaAluno.PeriodoInicio), "periodo_inicio");
            Map(nameof(FrequenciaAluno.Tipo), "tipo");
            Map(nameof(FrequenciaAluno.TotalAulas), "total_aulas");
            Map(nameof(FrequenciaAluno.TotalAusencias), "total_ausencias");
            Map(nameof(FrequenciaAluno.TotalRemotos), "total_remotos");
            Map(nameof(FrequenciaAluno.TotalPresencas), "total_presencas");
            Map(nameof(FrequenciaAluno.TotalCompensacoes), "total_compensacoes");
            Map(nameof(FrequenciaAluno.TurmaId), "turma_id");
            Map(nameof(FrequenciaAluno.Professor), "professor_rf");
        }
    }
}