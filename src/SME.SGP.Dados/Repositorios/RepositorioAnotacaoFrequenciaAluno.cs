using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFrequenciaAluno : RepositorioBase<AnotacaoFrequenciaAluno>, IRepositorioAnotacaoFrequenciaAluno
    {
        public RepositorioAnotacaoFrequenciaAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<bool> ExcluirAnotacoesDaAula(long aulaId)
        {
            var command = "update anotacao_frequencia_aluno set excluido = true where not excluido and aula_id = @aulaId";
            await database.ExecuteAsync(command, new { aulaId });
            return true;
        }

        public async Task<IEnumerable<string>> ListarAlunosComAnotacaoFrequenciaNaAula(long aulaId)
        {
            var query = "select codigo_aluno from anotacao_frequencia_aluno where not excluido and aula_id = @aulaId";

            return await database.Conexao.QueryAsync<string>(query, new { aulaId });
        }

        public async Task<AnotacaoFrequenciaAluno> ObterPorAlunoAula(string codigoAluno, long aulaId)
        {
            var query = "select * from anotacao_frequencia_aluno where not excluido and codigo_aluno = @codigoAluno and aula_id = @aulaId";

            return await database.Conexao.QueryFirstOrDefaultAsync<AnotacaoFrequenciaAluno>(query, new { codigoAluno, aulaId });
        }

        public async Task<IEnumerable<JustificativaAlunoDto>> ObterPorTurmaAlunoComponenteCurricular(long turmaId, long codigoAluno, long componenteCurricularId)
        {
            var query = @"select n.* from (
                            (
                            select an.id,case when ma.descricao is not null then ma.descricao else an.anotacao end as Motivo, a.data_aula DataAnotacao 
                            from anotacao_frequencia_aluno an
                            left join motivo_ausencia ma on an.motivo_ausencia_id = ma.id  
                            inner join aula a on a.id = an.aula_id 
                            inner join turma t on t.turma_id = a.turma_id
                            where not an.excluido 
                            and t.id = @turmaId 
                            and an.codigo_aluno = @codigoAluno
                            and a.disciplina_id = @componenteCurricularId
                            )
                            union
                            (
                            select 0 as id, '' as Motivo, a.data_aula as DataAnotacao 
                            from registro_ausencia_aluno raa 
                            inner join registro_frequencia rf on rf.id = raa.registro_frequencia_id 
                            inner join aula a on a.id = rf.aula_id 
                            left join anotacao_frequencia_aluno an on an.aula_id = a.id and an.codigo_aluno = @codigoAluno
                            left join turma t on t.turma_id = a.turma_id 
                            where t.id = @turmaId
                            and raa.codigo_aluno = @codigoAluno
                            and a.disciplina_id = @componenteCurricularId
                            and not a.excluido 
                            and not raa.excluido
                            and an.id is null
                            )
                            ) n
                            order by n.DataAnotacao desc";

            return await database.Conexao.QueryAsync<JustificativaAlunoDto>(query, new
            {
                turmaId,
                codigoAluno = codigoAluno.ToString(),
                componenteCurricularId = componenteCurricularId.ToString()
            });
        }
    }
}