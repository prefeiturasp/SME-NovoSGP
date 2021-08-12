using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicadoAnoEscolar : IRepositorioComunicadoAnoEscolar
    {
        protected readonly ISgpContext database;

        public RepositorioComunicadoAnoEscolar(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public virtual async Task<long> SalvarAsync(ComunicadoAnoEscolar comunicadoAnoEscolar)
        {
            if (comunicadoAnoEscolar.Id > 0)
                await database.Conexao.UpdateAsync(comunicadoAnoEscolar);
            else
                comunicadoAnoEscolar.Id = (long)(await database.Conexao.InsertAsync(comunicadoAnoEscolar));

            return comunicadoAnoEscolar.Id;
        }
        public async Task<bool> ExcluirPorIdComunicado(long id)
        {
            var query = "update comunicado_ano_escolar set excluido=true WHERE comunicado_id = @id";
            return await database.Conexao.ExecuteAsync(query, new { id }) != 0;
        }
        public async Task<IEnumerable<int>> ObterAnosEscolaresPorComunicadoId(long id)
        {
            var sql = @"select ano_escolar 
                          from comunicado_ano_escolar cm
                         where cm.comunicado_id = @id
                           and not excluido";
            var parametros = new { id };
            return await database.QueryAsync<int>(sql, parametros);
        }
    }
}
