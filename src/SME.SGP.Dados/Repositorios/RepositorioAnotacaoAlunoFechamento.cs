using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoAlunoFechamento : RepositorioBase<AnotacaoAlunoFechamento>, IRepositorioAnotacaoAlunoFechamento
    {
        public RepositorioAnotacaoAlunoFechamento(ISgpContext conexao): base(conexao)
        {
        }

        public async Task<AnotacaoAlunoFechamento> ObterAnotacaoAlunoPorFechamento(long fechamentoId, string codigoAluno)
        {
            var query = @"select * from anotacao_aluno_fechamento 
                        where not excluido 
                          and fechamento_turma_disciplina_id = @fechamentoId
                          and aluno_codigo = @codigoAluno";

            return await database.Conexao.QueryFirstOrDefaultAsync<AnotacaoAlunoFechamento>(query, new { fechamentoId, codigoAluno });
        }
    }
}
