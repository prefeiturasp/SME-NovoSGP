using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAluno : RepositorioBase<AcompanhamentoAluno>, IRepositorioAcompanhamentoAluno
    {
        public RepositorioAcompanhamentoAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoPorTurmaAlunoESemestre(long turmaId, string alunoCodigo, int semestre)
        {
            try
            {
                var query = @"select aas.*
                        from acompanhamento_aluno_semestre aas
                            inner join acompanhamento_aluno aa on aa.id = aas.acompanhamento_aluno_id
                        where aa.turma_id = @turmaId
                            and aa.aluno_codigo = @alunoCodigo
                            and aas.semestre = @semestre 
                            and not aas.excluido ";

                return await database.Conexao.QueryFirstOrDefaultAsync<AcompanhamentoAlunoSemestre>(query, new { turmaId, alunoCodigo, semestre });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        public async Task<long> ObterPorTurmaEAluno(long turmaId, string alunoCodigo)
        {
            var query = @"select id from acompanhamento_aluno where not excluido and turma_id = @turmaId and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { turmaId, alunoCodigo });
        }

        public async Task<int> ObterTotalAlunosComAcompanhamentoPorTurmaSemestre(long turmaId, int semestre)
        {
            var query = $@"select count(distinct aa.aluno_codigo)
                              from acompanhamento_aluno_semestre aas
                             inner join acompanhamento_aluno aa on aa.id = aas.acompanhamento_aluno_id
                             inner join turma t on t.id = aa.turma_id
                             where not aas.excluido
                               and not aa.excluido
                               and aas.semestre = @semestre
                               and t.id = @turmaId ";

                return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, semestre });
        }

        public async Task<int> ObterTotalAlunosTurmaSemestre(long turmaId, int semestre)
        {

            var whereBimestre = semestre == 1 ? " and pe.bimestre in (1,2) " : "and pe.bimestre in (3, 4)";

            var query = $@"select count(distinct rfa.codigo_aluno)
                              from registro_frequencia_aluno rfa
                             inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id and not rf.excluido
                             inner join aula a on a.id = rf.aula_id and not a.excluido
                             inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and a.data_aula 
                             between pe.periodo_inicio and pe.periodo_fim
                             inner join turma t on t.turma_id = a.turma_id
                             where not rfa.excluido
                               and t.id = @turmaId
                              {whereBimestre}";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, semestre });
        }
    }
}
