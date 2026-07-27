using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoTurmaDisciplinaMap : BaseEntityMap<FechamentoTurmaDisciplina>
    {
        public FechamentoTurmaDisciplinaMap()
        {
            ToTable("fechamento_turma_disciplina");
            Map(nameof(FechamentoTurmaDisciplina.FechamentoTurmaId), "fechamento_turma_id");
            Map(nameof(FechamentoTurmaDisciplina.DisciplinaId), "disciplina_id");
            Map(nameof(FechamentoTurmaDisciplina.Situacao), "situacao");
            Map(nameof(FechamentoTurmaDisciplina.Justificativa), "justificativa");
            Map(nameof(FechamentoTurmaDisciplina.Migrado), "migrado");
            Map(nameof(FechamentoTurmaDisciplina.Excluido), "excluido");
        }
    }
}