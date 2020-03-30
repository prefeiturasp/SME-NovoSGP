using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotaConceitoBimestre : RepositorioBase<NotaConceitoBimestre>, IRepositorioNotaConceitoBimestre
    {
        const string queryPorFechamento = "select * from nota_conceito_bimestre where fechamento_turma_disciplina_id = @fechamentoId";
        public RepositorioNotaConceitoBimestre(ISgpContext database) : base(database)
        {
        }

        public async Task<AnotacaoAlunoCompletoDto> ObterAnotacaoAlunoPorFechamento(long fechamentoId, string codigoAluno)
        {
            var query = @"select criado_em as CriadoEm, criado_por as CriadoPor, alterado_em as AlteradoEm, alterado_por as AlteradoPor,
                                 alterado_rf as AlteradoRF, criado_rf as CriadoRF, anotacao 
                            from nota_conceito_bimestre nc
                           where fechamento_turma_disciplina_id = @fechamentoId 
                             and codigo_aluno = @codigoAluno";

            return await database.Conexao.QueryFirstOrDefaultAsync<AnotacaoAlunoCompletoDto>(query, new { fechamentoId, codigoAluno });
        }

        public async Task<NotaConceitoBimestre> ObterPorAlunoEFechamento(long fechamentoId, string codigoAluno)
        {
            var query = queryPorFechamento + " and codigo_aluno = @codigoAluno";

            return await database.Conexao.QueryFirstOrDefaultAsync<NotaConceitoBimestre>(query, new { fechamentoId, codigoAluno });
        }

        public async Task<IEnumerable<NotaConceitoBimestre>> ObterPorFechamentoTurma(long fechamentoId)
        {
            return await database.Conexao.QueryAsync<NotaConceitoBimestre>(queryPorFechamento, new { fechamentoId });
        }
    }
}