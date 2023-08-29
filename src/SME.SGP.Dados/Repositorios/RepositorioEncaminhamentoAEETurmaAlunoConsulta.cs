using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEncaminhamentoAEETurmaAlunoConsulta : IRepositorioEncaminhamentoAEETurmaAlunoConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioEncaminhamentoAEETurmaAlunoConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public Task<IEnumerable<EncaminhamentoAEETurmaAluno>> ObterEncaminhamentoAEETurmaAlunoPorIdAsync(long encaminhamentoAEEId)
        {
            var query = @"select id, encaminhamento_aee_id, turma_id, aluno_codigo
                          from encaminhamento_aee_turma_aluno
                          where encaminhamento_aee_id = @encaminhamentoAEEId";

            return database.QueryAsync<EncaminhamentoAEETurmaAluno>(query, new { encaminhamentoAEEId });
        }
    }
}
