using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAcompanhamentoAluno : RepositorioBase<AcompanhamentoAluno>, IRepositorioAcompanhamentoAluno
    {
        public RepositorioAcompanhamentoAluno(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AcompanhamentoAlunoDto>> ObterAcompanhamentoPorTurmaAlunoESemestre(string turmaCodigo, string alunoCodigo, int semestre)
        {
            try
            {

            var query = @"select 
	                        aas.acompanhamento_aluno_id as AcompanhamentoAlunoId,
	                        aas.observacoes as observacoes,
	                        aa.turma_id as TurmaId,
	                        aa.aluno_codigo as AlunoCodigo,
	                        aas.semestre
                        from acompanhamento_aluno_semestre aas
                            inner join acompanhamento_aluno aa on aa.id = aas.acompanhamento_aluno_id
                        where aa.turma_id = @turmaCodigo
                            and aa.aluno_codigo = @alunoCodigo
                            and aas.semestre = @semestre 
                            and aas.excluido = false ";

            return await database.Conexao.QueryAsync<AcompanhamentoAlunoDto>(query, new { turmaCodigo, alunoCodigo, semestre });
            }
            catch (System.Exception ex )
            {
                throw ex;
            }

        }
    }
}
