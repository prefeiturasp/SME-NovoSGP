using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAlunoConsulta : RepositorioBase<AcompanhamentoAluno>, IRepositorioAcompanhamentoAlunoConsulta
    {
        public RepositorioAcompanhamentoAlunoConsulta(ISgpContext conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoPorTurmaAlunoESemestre(long turmaId, string alunoCodigo, int semestre)
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

        public async Task<long> ObterPorTurmaEAluno(long turmaId, string alunoCodigo)
        {
            var query = @"select id from acompanhamento_aluno where not excluido and turma_id = @turmaId and aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<long>(query, new { turmaId, alunoCodigo });
        }

        public async Task<int> ObterTotalAlunosComAcompanhamentoPorTurmaSemestre(long turmaId, int semestre, string[] codigosAlunos)
        {
            var query = $@"select count(distinct aa.aluno_codigo)
                              from acompanhamento_aluno_semestre aas
                             inner join acompanhamento_aluno aa on aa.id = aas.acompanhamento_aluno_id
                             inner join turma t on t.id = aa.turma_id
                             where not aas.excluido
                               and not aa.excluido
                               and aas.semestre = @semestre
                               and t.id = @turmaId
                               and aa.aluno_codigo  = ANY(@codigosAlunos)";

                return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, semestre, codigosAlunos });
        }

        public async Task<int> ObterTotalAlunosTurmaSemestre(long turmaId, int semestre)
        {

            var whereBimestre = semestre == 1 ? " and pe.bimestre in (1,2) " : "and pe.bimestre in (3, 4)";

            var query = $@"select count(distinct rfa.codigo_aluno)
                              from registro_frequencia_aluno rfa
                             inner join aula a on a.id = rfa.aula_id and not a.excluido
                             inner join periodo_escolar pe on pe.tipo_calendario_id = a.tipo_calendario_id and a.data_aula 
                             between pe.periodo_inicio and pe.periodo_fim
                             inner join turma t on t.turma_id = a.turma_id
                             where not rfa.excluido
                               and t.id = @turmaId
                              {whereBimestre}";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(query, new { turmaId, semestre });
        }

        public async Task<int> ObterUltimoSemestreAcompanhamentoGerado(string alunoCodigo)
        {
            var sql = $@"SELECT aas.semestre
                            FROM acompanhamento_aluno aa
                            INNER JOIN acompanhamento_aluno_semestre aas ON aa.id = aas.acompanhamento_aluno_id
                            WHERE aa.criado_em =
                                (SELECT max(aa2.criado_em) AS DataCriacao
                                 FROM acompanhamento_aluno aa2
                                 INNER JOIN acompanhamento_aluno_semestre aas2 ON aa2.id = aas2.acompanhamento_aluno_id
                                 WHERE aa2.aluno_codigo = @alunoCodigo)
                              AND aa.aluno_codigo = @alunoCodigo";

            return await database.Conexao.QueryFirstOrDefaultAsync<int>(sql, new { alunoCodigo });
        }

        public async Task<IEnumerable<AcompanhamentoAluno>> ObterAlunosQueContemPercursoIndividalPreenchido(long turmaId, int semestre)
        {
            var query = $@"select aa.aluno_codigo
                              from acompanhamento_aluno_semestre aas
                             inner join acompanhamento_aluno aa on aa.id = aas.acompanhamento_aluno_id
                             where not aas.excluido
                               and not aa.excluido
                               and aas.semestre = @semestre
                               and aa.turma_id = @turmaId
                               and not aas.percurso_individual isnull 
                               and aas.percurso_individual <> ''";

            return await database.Conexao.QueryAsync<AcompanhamentoAluno>(query, new { turmaId, semestre });
        }
    }
}
