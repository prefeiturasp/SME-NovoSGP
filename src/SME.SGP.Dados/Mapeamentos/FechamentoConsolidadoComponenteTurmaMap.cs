using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoConsolidadoComponenteTurmaMap : BaseMap<FechamentoConsolidadoComponenteTurma>
    {
        public FechamentoConsolidadoComponenteTurmaMap()
        {
            ToTable("consolidado_fechamento_componente_turma");
            Map(nameof(FechamentoConsolidadoComponenteTurma.DataAtualizacao), "dt_atualizacao");
            Map(nameof(FechamentoConsolidadoComponenteTurma.Status), "status");
            Map(nameof(FechamentoConsolidadoComponenteTurma.ComponenteCurricularCodigo), "componente_curricular_id");
            Map(nameof(FechamentoConsolidadoComponenteTurma.ProfessorRf), "professor_rf");
            Map(nameof(FechamentoConsolidadoComponenteTurma.ProfessorNome), "professor_nome");
            Map(nameof(FechamentoConsolidadoComponenteTurma.TurmaId), "turma_id");
            Map(nameof(FechamentoConsolidadoComponenteTurma.Bimestre), "bimestre");
        }
    }
}