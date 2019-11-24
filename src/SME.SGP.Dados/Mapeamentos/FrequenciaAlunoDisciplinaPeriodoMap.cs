using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaAlunoDisciplinaPeriodoMap : BaseMap<FrequenciaAlunoDisciplinaPeriodo>
    {
        public FrequenciaAlunoDisciplinaPeriodoMap()
        {
            ToTable("frequencia_aluno_disciplina");
            Map(a => a.CodigoAluno).ToColumn("codigo_aluno");
            Map(a => a.DisciplinaId).ToColumn("disciplina_id");
            Map(a => a.PeriodoInicio).ToColumn("periodo_inicio");
            Map(a => a.PeriodoFim).ToColumn("periodo_fim");
            Map(a => a.TotalAulas).ToColumn("total_aulas");
            Map(a => a.TotalAusencias).ToColumn("total_ausencias");
            Map(a => a.PercentualFrequencia).Ignore();
        }
    }
}