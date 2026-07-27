using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FechamentoAlunoMap : BaseEntityMap<FechamentoAluno>
    {
        public FechamentoAlunoMap()
        {
            ToTable("fechamento_aluno");
            Map(nameof(FechamentoAluno.FechamentoTurmaDisciplinaId), "fechamento_turma_disciplina_id");
            Map(nameof(FechamentoAluno.AlunoCodigo), "aluno_codigo");
            Map(nameof(FechamentoAluno.Excluido), "excluido");
        }
    }
}