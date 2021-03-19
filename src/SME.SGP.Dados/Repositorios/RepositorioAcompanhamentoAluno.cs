using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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
    }
}
