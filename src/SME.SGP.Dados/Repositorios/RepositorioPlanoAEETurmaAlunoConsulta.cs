using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEETurmaAlunoConsulta : IRepositorioPlanoAEETurmaAlunoConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPlanoAEETurmaAlunoConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public Task<IEnumerable<PlanoAEETurmaAluno>> ObterPlanoAEETurmaAlunoPorIdAsync(long planoAEEId)
        {
            var query = @"select id, plano_aee_id, turma_id, aluno_codigo
                          from plano_aee_turma_aluno
                          where plano_aee_id = @planoAEEId";

            return database.QueryAsync<PlanoAEETurmaAluno>(query, new { planoAEEId });
        }
    }
}
