using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioSemestralAluno : RepositorioBase<RelatorioSemestralAluno>, IRepositorioRelatorioSemestralAluno
    {
        public RepositorioRelatorioSemestralAluno(ISgpContext database) : base(database)
        {
        }

        public async Task<RelatorioSemestralAluno> ObterCompletoPorIdAsync(long relatorioSemestralAlunoId)
        {
            var query = @"select ra.*, rs.*, s.* from relatorio_semestral_aluno ra 
                    inner join relatorio_semestral_aluno_secao rs on rs.relatorio_semestral_aluno_id = ra.id
                    inner join secao_relatorio_semestral s on s.id = rs.secao_relatorio_semestral_id 
                    where ra.id = @relatorioSemestralAlunoId";

            RelatorioSemestralAluno relatorioAluno = null;
            await database.Conexao.QueryAsync<RelatorioSemestralAluno, RelatorioSemestralAlunoSecao, SecaoRelatorioSemestral, RelatorioSemestralAluno>(
                    query, (relatorioSemestralAluno, relatorioSemestralAlunoSecao, secaoRelatorioSemestral) =>
                    {
                        if (relatorioAluno == null)
                            relatorioAluno = relatorioSemestralAluno;

                        relatorioSemestralAlunoSecao.SecaoRelatorioSemestral = secaoRelatorioSemestral;
                        relatorioSemestralAlunoSecao.RelatorioSemestralAluno = relatorioSemestralAluno;

                        relatorioAluno.Secoes.Add(relatorioSemestralAlunoSecao);

                        return relatorioSemestralAluno;
                    }, new { relatorioSemestralAlunoId });

            return relatorioAluno;
        }

        public Task<RelatorioSemestralAluno> ObterPorTurmaAlunoAsync(long relatorioSemestralId, string alunoCodigo)
        {
            throw new System.NotImplementedException();
        }
    }
}