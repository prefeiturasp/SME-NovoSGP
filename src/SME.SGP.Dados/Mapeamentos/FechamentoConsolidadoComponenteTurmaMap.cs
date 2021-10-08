using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public  class FechamentoConsolidadoComponenteTurmaMap : BaseMap<FechamentoConsolidadoComponenteTurma>
    {
        public FechamentoConsolidadoComponenteTurmaMap()
        {
            ToTable("consolidado_fechamento_componente_turma");
            Map(c => c.DataAtualizacao).ToColumn("dt_atualizacao");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.ComponenteCurricularCodigo).ToColumn("componente_curricular_id");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
            Map(c => c.ProfessorNome).ToColumn("professor_nome");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Bimestre).ToColumn("bimestre");
        }
    }
}
