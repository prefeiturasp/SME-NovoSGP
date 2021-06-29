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

        public async Task<int> ObterTotalAlunosComAcompanhamentoPorTurmaAnoLetivoESemestre(long turmaId, int anoLetivo, int semestre)
        {
                var query = @"select count(distinct rfa.codigo_aluno) as quantidadeComAcompanhamento
                              from registro_frequencia_aluno rfa
                                inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id and not rf.excluido
                                inner join aula a on a.id = rf.aula_id and not a.excluido
                                inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim
                                inner join turma t on t.turma_id = a.turma_id
                              where not rfa.excluido
                              and t.id = @turmaId
                              and t.ano_letivo = @anoLetivo
                              and t.semestre  = @semestre ";

                return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, anoLetivo, semestre });
        }

        public async Task<int> ObterTotalAlunosTurmaAnoLetivoESemestre(long turmaId, int anoLetivo, int semestre)
        {
            var query = @"select count(distinct rfa.codigo_aluno)
                              from registro_frequencia_aluno rfa
                             inner join registro_frequencia rf on rf.id = rfa.registro_frequencia_id and not rf.excluido
                             inner join aula a on a.id = rf.aula_id and not a.excluido
                             inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and a.data_aula between pe.periodo_inicio and pe.periodo_fim
                             inner join turma t on t.turma_id = a.turma_id
                             where not rfa.excluido
                               and t.id = @turmaId
                              and t.ano_letivo = @anoLetivo
                              and t.semestre  = @semestre ";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, anoLetivo, semestre });
        }
    }
}
