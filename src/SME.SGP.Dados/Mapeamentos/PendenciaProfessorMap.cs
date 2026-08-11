using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class PendenciaProfessorMap : SimpleMap<PendenciaProfessor>
    {
        public PendenciaProfessorMap()
        {
            ToTable("pendencia_professor");
            Map(nameof(PendenciaProfessor.PendenciaId), "pendencia_id");
            Map(nameof(PendenciaProfessor.ComponenteCurricularId), "componente_curricular_id");
            Map(nameof(PendenciaProfessor.TurmaId), "turma_id");
            Map(nameof(PendenciaProfessor.PeriodoEscolarId), "periodo_escolar_id");
            Map(nameof(PendenciaProfessor.ProfessorRf), "professor_rf");
        }
    }
}