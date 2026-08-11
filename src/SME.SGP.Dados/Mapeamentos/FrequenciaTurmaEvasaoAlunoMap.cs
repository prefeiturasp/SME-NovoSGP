using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaTurmaEvasaoAlunoMap : SimpleMap<FrequenciaTurmaEvasaoAluno>
    {
        public FrequenciaTurmaEvasaoAlunoMap()
        {
            ToTable("frequencia_turma_evasao_aluno");
            Map(nameof(FrequenciaTurmaEvasaoAluno.FrequenciaTurmaEvasaoId), "frequencia_turma_evasao_id");
            Map(nameof(FrequenciaTurmaEvasaoAluno.AlunoCodigo), "codigo_aluno");
            Map(nameof(FrequenciaTurmaEvasaoAluno.AlunoNome), "nome_aluno");
            Map(nameof(FrequenciaTurmaEvasaoAluno.PercentualFrequencia), "percentual_frequencia");
        }
    }
}