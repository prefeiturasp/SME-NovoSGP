using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicadoTipoEscola : IRepositorioComunicadoTipoEscola
    {
        protected readonly ISgpContext database;

        public RepositorioComunicadoTipoEscola(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public virtual async Task<long> SalvarAsync(ComunicadoTipoEscola comunicadoTipoEscola)
        {
            var sqlQuery = new StringBuilder();

            if (comunicadoTipoEscola.Id > 0)
                await database.Conexao.UpdateAsync(comunicadoTipoEscola);
            else
            {
                sqlQuery.AppendLine("insert into comunicado_tipo_escola (comunicado_id, tipo_escola, excluido)");
                sqlQuery.AppendLine("values (@comunicadoId, @tipoEscola, @excluido)");
                sqlQuery.AppendLine("returning id;");

                comunicadoTipoEscola.Id = (long)await database.Conexao
                    .ExecuteScalarAsync(sqlQuery.ToString(),
                    new
                    {
                        comunicadoId = comunicadoTipoEscola.ComunicadoId,
                        tipoEscola = comunicadoTipoEscola.TipoEscola,
                        excluido = false
                    });
            }

            return comunicadoTipoEscola.Id;
        }
        public async Task<bool> ExcluirPorIdComunicado(long id)
        {
            var query = "update comunicado_tipo_escola set excluido=true WHERE comunicado_id = @id";
            return await database.Conexao.ExecuteAsync(query, new { id }) != 0;
        }
        public async Task<IEnumerable<int>> ObterTiposEscolasPorComunicadoId(long id)
        {
            var sql = @"select tipo_escola 
                          from comunicado_tipo_escola cm
                         where cm.comunicado_id = @id
                           and not excluido";
            var parametros = new { id };
            return await database.QueryAsync<int>(sql, parametros);
        }
    }
}
