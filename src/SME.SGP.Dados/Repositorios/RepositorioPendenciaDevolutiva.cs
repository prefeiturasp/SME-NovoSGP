using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDevolutiva : IRepositorioPendenciaDevolutiva
    {
		private readonly ISgpContextConsultas database;

		public RepositorioPendenciaDevolutiva(ISgpContextConsultas database)
        {
			this.database = database ?? throw new ArgumentNullException(nameof(database));
		}

        public async Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorPendencia(long pendenciaId)
        {
            var query = $@"SELECT
							pd.*, 
							p.*,
							cc.*,
							t.*
						FROM
							pendencia_devolutiva pd
						INNER JOIN pendencia p ON
							pd.pendencia_Id = p.id
						INNER JOIN componente_curricular cc ON
							pd.componente_curricular_id = cc.id
						INNER JOIN turma t ON
							pd.turma_id = t.id
						WHERE
							pd.pendencia_Id = @pendenciaId ";

            return await database.Conexao.QueryAsync<PendenciaDevolutiva>(query, new { pendenciaId});
        }

        public async Task<IEnumerable<PendenciaDevolutiva>> ObterPendenciasDevolutivaPorTurmaComponente(long turmaId, long componenteId)
        {
			var query = @"SELECT
							pd.*, 
							p.*,
							cc.*,
							t.*
						FROM
							pendencia_devolutiva pd
						INNER JOIN pendencia p ON
							pd.pendencia_Id = p.id
						INNER JOIN componente_curricular cc ON
							pd.componente_curricular_id = cc.id
						INNER JOIN turma t ON
							pd.turma_id = t.id
						WHERE
							pd.pendencia_Id = @pendenciaId ";

			return await database.Conexao.QueryAsync<PendenciaDevolutiva>(query,new { turmaId, componenteId });
		}
    }
}
