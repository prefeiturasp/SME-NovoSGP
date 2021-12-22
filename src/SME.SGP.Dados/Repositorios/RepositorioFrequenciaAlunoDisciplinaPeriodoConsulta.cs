using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta : IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta
    {
        private readonly ISgpContextConsultas database;

        public RepositorioFrequenciaAlunoDisciplinaPeriodoConsulta(ISgpContextConsultas database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<FrequenciaAluno>> ObterPorAlunos(IEnumerable<string> alunosCodigo, IEnumerable<long?> periodosEscolaresId, string[] turmasId)
        {
            var query = new StringBuilder(@"select
	                        *
                        from
	                        frequencia_aluno
                        where
	                        codigo_aluno = any(@alunosCodigo)	                        	                        
                            and turma_id = any(@turmasId) ");
            if (periodosEscolaresId != null && periodosEscolaresId.AsList().Count > 0)
            {
                query.AppendLine("and periodo_escolar_id = any(@periodosEscolaresId)");
            }

            return await database.QueryAsync<FrequenciaAluno>(query.ToString(), new
            {
                alunosCodigo,
                periodosEscolaresId,
                turmasId
            });
        }
    }
}
