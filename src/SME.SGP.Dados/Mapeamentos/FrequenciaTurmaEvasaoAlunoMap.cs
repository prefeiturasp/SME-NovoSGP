using Dapper.FluentMap.Dommel.Mapping;
using SME.SGP.Dominio;

namespace SME.SGP.Dados.Mapeamentos
{
    public class FrequenciaTurmaEvasaoAlunoMap : DommelEntityMap<FrequenciaTurmaEvasaoAluno>
    {
        public FrequenciaTurmaEvasaoAlunoMap()
        {
            ToTable("frequencia_turma_evasao_aluno");
            Map(c => c.Id).ToColumn("id").IsIdentity().IsKey();
            Map(c => c.FrequenciaTurmaEvasaoId).ToColumn("frequencia_turma_evasao_id");
            Map(c => c.AlunoCodigo).ToColumn("codigo_aluno");
            Map(c => c.AlunoNome).ToColumn("nome_aluno");
            Map(c => c.PercentualFrequencia).ToColumn("percentual_frequencia");
        }
    }
}
