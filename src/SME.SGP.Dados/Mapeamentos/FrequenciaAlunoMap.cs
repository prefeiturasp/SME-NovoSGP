using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaAlunoMap : BaseMap<FrequenciaAluno>
    {
        public FrequenciaAlunoMap()
        {
            ToTable("frequencia_aluno");
            Map(a => a.PercentualFrequencia).Ignore();
            Map(a => a.PercentualFrequenciaFormatado).Ignore();
            Map(a => a.PercentualFrequenciaFinal).Ignore();
            Map(a => a.PercentualFrequenciaFinalFormatado).Ignore();
            Map(a => a.NumeroFaltasNaoCompensadas).Ignore();
            Map(c => c.Bimestre).ToColumn("bimestre");
            Map(c => c.CodigoAluno).ToColumn("codigo_aluno");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(c => c.PeriodoFim).ToColumn("periodo_fim");
            Map(c => c.PeriodoInicio).ToColumn("periodo_inicio");
            Map(c => c.Tipo).ToColumn("tipo");
            Map(c => c.TotalAulas).ToColumn("total_aulas");
            Map(c => c.TotalAusencias).ToColumn("total_ausencias");
            Map(c => c.TotalRemotos).ToColumn("total_remotos");
            Map(c => c.TotalPresencas).ToColumn("total_presencas");
            Map(c => c.TotalCompensacoes).ToColumn("total_compensacoes");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Professor).ToColumn("professor_rf");
        }
    }
}
