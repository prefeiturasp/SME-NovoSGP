using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaMap : BaseMap<Aula>
    {
        public AulaMap()
        {
            ToTable("aula");
            Map(a => a.UeId).ToColumn("ue_id");
            Map(a => a.DisciplinaId).ToColumn("disciplina_id");
            Map(a => a.DisciplinaCompartilhadaId).ToColumn("disciplina_compartilhada_id");
            Map(a => a.TurmaId).ToColumn("turma_id");
            Map(a => a.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(a => a.ProfessorRf).ToColumn("professor_rf");
            Map(a => a.Quantidade).ToColumn("quantidade");
            Map(a => a.DataAula).ToColumn("data_aula");
            Map(a => a.RecorrenciaAula).ToColumn("recorrencia_aula");
            Map(a => a.TipoAula).ToColumn("tipo_aula");
            Map(a => a.Excluido).ToColumn("excluido");
            Map(a => a.Migrado).ToColumn("migrado");
            Map(a => a.AulaPaiId).ToColumn("aula_pai_id");
            Map(a => a.WorkflowAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(a => a.AulaCJ).ToColumn("aula_cj");
            Map(a => a.DisciplinaNome).Ignore();
        }
    }
}