using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaAlunoMap : BaseMap<FrequenciaAluno>
    {
        public FrequenciaAlunoMap()
        {
            ToTable("frequencia_aluno");
            Map(a => a.CodigoAluno).ToColumn("codigo_aluno");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.DisciplinaId).ToColumn("disciplina_id");
            Map(a => a.PeriodoEscolarId).ToColumn("periodo_escolar_id");
            Map(a => a.PeriodoInicio).ToColumn("periodo_inicio");
            Map(a => a.PeriodoFim).ToColumn("periodo_fim");
            Map(a => a.TotalAulas).ToColumn("total_aulas");
            Map(a => a.TotalAusencias).ToColumn("total_ausencias");
            Map(a => a.TotalCompensacoes).ToColumn("total_compensacoes");
            Map(a => a.Tipo).ToColumn("tipo");
            Map(a => a.Bimestre).ToColumn("bimestre");
            Map(a => a.PercentualFrequencia).Ignore();
            Map(a => a.NumeroFaltasNaoCompensadas).Ignore();
            Map(a => a.TotalRemotos).ToColumn("total_remotos");
            Map(a => a.TotalPresencas).ToColumn("total_presencas");
        }
    }
}