using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public  class FechamentoConsolidadoComponenteTurmaMap : BaseMap<FechamentoConsolidadoComponenteTurma>
    {
        public FechamentoConsolidadoComponenteTurmaMap()
        {
            ToTable("consolidado_fechamento_componente_turma");
            Map(a => a.DataAtualizacao).ToColumn("dt_atualizacao");
            Map(a => a.ComponenteCurricularCodigo).ToColumn("componente_curricular_id");
            Map(a => a.ProfessorNome).ToColumn("professor_nome");
            Map(a => a.ProfessorRf).ToColumn("professor_rf");
            Map(a => a.TurmaId).ToColumn("turma_id");
        }
    }
}
