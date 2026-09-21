using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaMap : BaseMap<Aula>
    {
        public AulaMap()
        {
            ToTable("aula");
            Map(nameof(Aula.AulaCJ), "aula_cj");
            Map(nameof(Aula.AulaPaiId), "aula_pai_id");
            Map(nameof(Aula.DataAula), "data_aula");
            Map(nameof(Aula.DisciplinaCompartilhadaId), "disciplina_compartilhada_id");
            Map(nameof(Aula.DisciplinaId), "disciplina_id");
            Map(nameof(Aula.Excluido), "excluido");
            Map(nameof(Aula.Migrado), "migrado");
            Map(nameof(Aula.ProfessorRf), "professor_rf");
            Map(nameof(Aula.Quantidade), "quantidade");
            Map(nameof(Aula.RecorrenciaAula), "recorrencia_aula");
            Map(nameof(Aula.Status), "status");
            Map(nameof(Aula.TipoAula), "tipo_aula");
            Map(nameof(Aula.TipoCalendarioId), "tipo_calendario_id");
            Map(nameof(Aula.TurmaId), "turma_id");
            Map(nameof(Aula.UeId), "ue_id");
            Map(nameof(Aula.WorkflowAprovacaoId), "wf_aprovacao_id");
        }
    }
}