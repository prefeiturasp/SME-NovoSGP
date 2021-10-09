using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class AulaMap : BaseMap<Aula>
    {
        public AulaMap()
        {
            ToTable("aula");
            Map(c => c.AulaCJ).ToColumn("aula_cj");
            Map(c => c.AulaPaiId).ToColumn("aula_pai_id");
            Map(a => a.ComponenteCurricularEol).Ignore();
            Map(c => c.DataAula).ToColumn("data_aula");
            Map(c => c.DisciplinaCompartilhadaId).ToColumn("disciplina_compartilhada_id");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(a => a.DisciplinaNome).Ignore();
            Map(c => c.Excluido).ToColumn("excluido");
            Map(c => c.Migrado).ToColumn("migrado");
            Map(c => c.ProfessorRf).ToColumn("professor_rf");
            Map(c => c.Quantidade).ToColumn("quantidade");
            Map(c => c.RecorrenciaAula).ToColumn("recorrencia_aula");
            Map(c => c.Status).ToColumn("status");
            Map(c => c.TipoAula).ToColumn("tipo_aula");
            Map(c => c.TipoCalendarioId).ToColumn("tipo_calendario_id");
            Map(a => a.Turma).Ignore();
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.WorkflowAprovacaoId).ToColumn("wf_aprovacao_id");
            Map(c => c.EhAEE).Ignore();
            Map(c => c.EhAEEContraturno).Ignore();
            Map(c => c.EhAulaCompartilhada).Ignore();
            Map(c => c.EhRecuperacaoParalela).Ignore();
            Map(c => c.EhTecnologiaAprendizagem).Ignore();
            Map(c => c.EhDataSelecionadaFutura).Ignore();
            Map(c => c.PermiteSubstituicaoFrequencia).Ignore();
        }
    }
}