using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnotacaoFechamentoAluno : RepositorioBase<AnotacaoFechamentoAluno>, IRepositorioAnotacaoFechamentoAluno
    {
        public RepositorioAnotacaoFechamentoAluno(ISgpContext database) : base(database)
        {
        }

        public Task<AnotacaoFechamentoAluno> ObterPorFechamentoEAluno(long fechamentoTurmaDisciplinaId, string alunoCodigo)
        {
            var query = @"select aaf.* 
                          from anotacao_fechamento_aluno aaf 
                         inner join fechamento_aluno fa on fa.id = aaf.fechamento_aluno_id
                         where not fa.excluido 
                           and fa.fechamento_turma_disciplina_id = @fechamentoTurmaDisciplinaId
                           and fa.aluno_codigo = @alunoCodigo";

            return database.Conexao.QueryFirstOrDefaultAsync<AnotacaoFechamentoAluno>(query, new { fechamentoTurmaDisciplinaId, alunoCodigo });
        }
    }
}
